using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Tree : Interactable
{
    public Vector3 bearPlacement = new Vector3(1.16f, 1.12f, 3.21f);
    public Vector3 cupPlacement = new Vector3(0.81f, 1.12f, 3.255f);
    public Vector3 bookPlacement = new Vector3(1.23f, 1.12f, 2.85f);

    public Vector3 bearRotation = new Vector3(0f, 45f, 0f);
    public Vector3 cupRotation = new Vector3(0f, 0f, 0f);
    public Vector3 bookRotation = new Vector3(0f, 45f, 0f);

    public AudioClip positiveReactionMilk;
    public AudioClip positiveReactionTeddy;
    public AudioClip positiveReactionBook;
    public AudioClip positiveReactionLight;
    public AudioClip positiveReactionFlute;

    public AudioClip softNegativeReaction;
    public AudioClip hardNegativeReaction;

    public GameObject closedMesh;
    public GameObject openMesh;

    public FadeVolume fireCrackling;

    public SurfaceClearer clearForCup;
    public SurfaceClearer clearForBear;
    public SurfaceClearer clearForBook;

    Renderer rend;
    AudioSource audioSource;
    [HideInInspector] public Animator anim;

    private bool currentlyReadingBook = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rend = GetComponent<Renderer>();
        anim = GetComponentInChildren<Animator>();
    }

    public void Chill()
    {
        anim.SetTrigger("Idle");
    }

    public override bool CanInteractWith(Pickupable heldItem)
    {
        if (SequenceTracker.Instance.flutePlayed)
        {
            return false;
        }
        else
        {
            return heldItem != null; // for now, let's highlight the tree any time you bring it an object
        }
    }

    public override bool Interact(Pickupable heldItem)
    {
        /// In order, the tree needs:
        ///     A Glass of Milk
        ///     A Stuffed Animal
        ///     Book Read Aloud
        ///     Fire Put Out
        ///     Flute Played
        
        if (heldItem)
        {
            // Handle interactions with the Cup
            Cup cup = heldItem.GetComponent<Cup>();
            if (cup && cup.currentLiquid == Cup.Liquid.Milk)
            {
                if (!SequenceTracker.Instance.milkConsumed)
                {
                    SequenceTracker.Instance.milkConsumed = true;
                    // elicit a positive reaction
                    Debug.Log("Mmm, Milk is good for strong branches!");
                    StartCoroutine(PlayInteractionSounds(heldItem, positiveReactionMilk));
                    MusicManager.Instance.AddNextLayer();
                    cup.EmptyCup();
                    if (clearForCup) clearForCup.ClearArea();
                    heldItem.transform.position = cupPlacement;
                    heldItem.transform.rotation = Quaternion.Euler(cupRotation);
                    return true; // The Cup SHOULD be dropped after this interation
                }
                else
                {
                    Debug.Log("Uh uh... (Already had milk)");
                    StartCoroutine(PlayInteractionSounds(null, hardNegativeReaction));
                    return false; // No need to try picking up or dropping after this interation
                }
            }
            else if (cup && cup.currentLiquid != Cup.Liquid.Milk)
            {
                Debug.Log("Nope, don't want that");
                StartCoroutine(PlayInteractionSounds(null, hardNegativeReaction));
                return false; // No need to try picking up or dropping after this interation
            }

            // Handle interactions with the Teddy Bear
            if (heldItem.GetComponent<TeddyBear>())
            {
                if (SequenceTracker.Instance.milkConsumed && 
                    !SequenceTracker.Instance.teddyBearProvided)
                {
                    SequenceTracker.Instance.teddyBearProvided = true;
                    Debug.Log("Aww, snuggle time!");
                    // elicit a positive reaction
                    StartCoroutine(PlayInteractionSounds(heldItem, positiveReactionTeddy));
                    MusicManager.Instance.AddNextLayer();
                    if (clearForBear) clearForBear.ClearArea();
                    heldItem.transform.position = bearPlacement;
                    heldItem.transform.rotation = Quaternion.Euler(bearRotation);
                    return true; // The Teddy Bear SHOULD be dropped after this interation
                }
                else if (SequenceTracker.Instance.teddyBearProvided)
                {
                    // Should this even happen? Bear could be made non-interactable if already placed
                    Debug.Log("No thanks! (Already received bear)");
                    StartCoroutine(PlayInteractionSounds(null, hardNegativeReaction));
                    return false; // No need to try picking up or dropping after this interation
                }
                else
                {
                    Debug.Log("Nah, not yet");
                    StartCoroutine(PlayInteractionSounds(null, softNegativeReaction));
                    return false; // No need to try picking up or dropping after this interation
                }
            }

            // Handle interactions with the Childen's Book
            if (heldItem.GetComponent <Book>())
            {
                // Do not do anything if book animation is currently playing
                if (currentlyReadingBook)
                {
                    return false; // PickupHolder should NOT try picking up or dropping during this interation
                }

                if (SequenceTracker.Instance.milkConsumed && 
                    SequenceTracker.Instance.teddyBearProvided &&
                    !SequenceTracker.Instance.bookRead)
                {
                    SequenceTracker.Instance.bookRead = true;
                    Debug.Log("Ahh, what a lovely story");
                    // elicit a positive reaction
                    StartCoroutine(HandleReadingBook(heldItem.GetComponent<Book>()));
                    // StartCoroutine(PlayInteractionSounds(heldItem, positiveReactionBook));
                    // MusicManager.Instance.AddNextLayer();
                    // heldItem.transform.position = bookPlacement;
                    return false; // PickupHolder should NOT try picking up or dropping after this interation
                }
                else if (SequenceTracker.Instance.bookRead)
                {
                    // Should this even happen? Bear could be made non-interactable if already placed
                    Debug.Log("Been there, done that (already had book read");
                    StartCoroutine(PlayInteractionSounds(null, hardNegativeReaction));
                    return false; // No need to try picking up or dropping after this interation
                }
                else
                {
                    Debug.Log("Nah, not yet");
                    StartCoroutine(PlayInteractionSounds(null, softNegativeReaction));
                    return false; // No need to try picking up or dropping after this interation
                }
            }

            // Fire interaction is handled at the fireplace

            // Handle interactions with the Flute
            if (heldItem.GetComponent<Flute>())
            {
                if (SequenceTracker.Instance.milkConsumed &&
                    SequenceTracker.Instance.teddyBearProvided &&
                    SequenceTracker.Instance.bookRead && 
                    SequenceTracker.Instance.fireOut && 
                    !SequenceTracker.Instance.flutePlayed)
                {
                    SequenceTracker.Instance.flutePlayed = true;
                    Debug.Log("ZZZZZzzzzzzzzz......");
                    // elicit a positive reaction
                    // heldItem.transform.position = flutePlacement;
                    heldItem.gameObject.SetActive(false); // Make flute disappear from view
                    StartCoroutine(TriggerEndCinematic(heldItem, positiveReactionFlute));
                    return true; // The Book SHOULD be dropped after this interation
                }
                else if (SequenceTracker.Instance.flutePlayed)
                {
                    // Could this even happen? Cinematic will have already taken place
                    Debug.Log("Deja vu all over again...");
                    StartCoroutine(PlayInteractionSounds(null, hardNegativeReaction));
                    return false; // No need to try picking up or dropping after this interation
                }
                else
                {
                    Debug.Log("Nah, not yet");
                    StartCoroutine(PlayInteractionSounds(heldItem, softNegativeReaction));
                    return false; // No need to try picking up or dropping after this interation
                }
            }
        }
        else
        {
            // If no item is held, nothing should happen (this can occur if held item just given and interaction is still available)
            return false; // PickupHolder should NOT try picking up or dropping anything
        }

        Debug.Log("Nope, don't want that");
        // StartCoroutine(PlayInteractionSounds(heldItem, hardNegativeReaction));
        StartCoroutine(PlayInteractionSounds(null, hardNegativeReaction));

        return false; // No need to try picking up or dropping after this interation
    }

    IEnumerator HandleReadingBook(Book book)
    {
        currentlyReadingBook = true;

        book.StartBookAnimation();

        // book.PlayTreeInteractionClip();
        while (book.IsAnimationPlaying())
        {
            yield return null;
        }
        yield return new WaitForSeconds(1f);

        anim.SetTrigger("Positive");

        // TODO: Handle playing of book sounds in Book script based on phase of animation
        audioSource.clip = positiveReactionBook;
        audioSource.Play();

        MusicManager.Instance.AddNextLayer();
        FindObjectOfType<PickupHolder>().TryPickup(); // When item is currently held, this will drop the item, i.e. the book
        if (clearForBook) clearForBook.ClearArea();
        book.transform.position = bookPlacement;
        book.transform.rotation = Quaternion.Euler(bookRotation);

        currentlyReadingBook = false;
    }

    IEnumerator PlayInteractionSounds(Pickupable pickup, AudioClip reactionSound)
    {
        if (pickup)
        {
            pickup.PlayTreeInteractionClip();
            while (pickup.IsPlayingClip())
            {
                yield return null;
            }
        }

        if (reactionSound == hardNegativeReaction)
        {
            anim.SetTrigger("Hard Negative");
        }
        else if (reactionSound == softNegativeReaction)
        {
            anim.SetTrigger("Soft Negative");
            yield return new WaitForSeconds(0.25f);
        }
        else if (reactionSound == positiveReactionMilk ||
                reactionSound == positiveReactionTeddy) // The other reactions get triggered elsewhere (i.e. HandleReadingBook(), TriggerEndCinematic(), and and GasFireValve.cs)
        {
            anim.SetTrigger("Positive");
        }

        audioSource.clip = reactionSound;
        audioSource.Play();
    }

    IEnumerator TriggerEndCinematic(Pickupable pickup, AudioClip reactionSound)
    {
        FindObjectOfType<InteractionDetector>().TurnOffHighlighting(); // This is pretty hacky but... should work for now
        MusicManager.Instance.ToggleGameLoop();
        // pickup.PlayTreeInteractionClip();
        while (pickup.IsPlayingClip())
        {
            yield return null;
        }

        anim.SetTrigger("Positive");

        audioSource.clip = reactionSound;
        audioSource.Play();
        while (audioSource.isPlaying)
        {
            yield return null;
        }

        fireCrackling.StartFadeOut();
        MusicManager.Instance.PlayFinale();

        CinematicController.Instance.StartCinematic();
        closedMesh.SetActive(false);
        openMesh.SetActive(true);
        yield return new WaitForSeconds(90f);
        PlayerInput playerInput = FindObjectOfType<PlayerInput>(); // Hacky way to do this, this should really be done in its own GameManager script
        playerInput.Restart();
    }

    public void PlayClip(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(bearPlacement, Vector3.one * 0.25f);
        Gizmos.DrawWireCube(cupPlacement, Vector3.one * 0.12f);
        Gizmos.DrawWireCube(bookPlacement, Vector3.one * 0.25f);
    }
}
