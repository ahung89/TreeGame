using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : Interactable
{
    public Vector3 bearPlacement = Vector3.zero;

    public AudioClip positiveReactionMilk;
    public AudioClip positiveReactionTeddy;
    public AudioClip positiveReactionBook;
    public AudioClip positiveReactionLight;
    public AudioClip positiveReactionFlute;

    public AudioClip softNegativeReaction;
    public AudioClip hardNegativeReaction;

    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public override bool CanInteractWith(Pickupable heldItem)
    {
        return heldItem != null; // for now, let's highlight the tree any time you bring it an object
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
                    return false; // No need to try picking up or dropping after this interation
                }
                else
                {
                    Debug.Log("Uh uh... (Already had milk)");
                    StartCoroutine(PlayInteractionSounds(heldItem, hardNegativeReaction));
                    return false; // No need to try picking up or dropping after this interation
                }
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
                    heldItem.transform.position = bearPlacement;
                    return true; // The Teddy Bear SHOULD be dropped after this interation
                }
                else if (SequenceTracker.Instance.teddyBearProvided)
                {
                    // Should this even happen? Bear could be made non-interactable if already placed
                    Debug.Log("No thanks! (Already received bear)");
                    StartCoroutine(PlayInteractionSounds(heldItem, hardNegativeReaction));
                    return false; // No need to try picking up or dropping after this interation
                }
                else
                {
                    Debug.Log("Nah, not yet");
                    StartCoroutine(PlayInteractionSounds(heldItem, softNegativeReaction));
                    return false; // No need to try picking up or dropping after this interation
                }
            }

            // Handle interactions with the Childen's Book
            if (heldItem.GetComponent <Book>())
            {
                if (SequenceTracker.Instance.milkConsumed && 
                    SequenceTracker.Instance.teddyBearProvided &&
                    !SequenceTracker.Instance.bookRead)
                {
                    SequenceTracker.Instance.bookRead = true;
                    Debug.Log("Ahh, what a lovely story");
                    // elicit a positive reaction
                    StartCoroutine(PlayInteractionSounds(heldItem, positiveReactionBook));
                    MusicManager.Instance.AddNextLayer();
                    return false; // No need to try picking up or dropping after this interation
                }
                else if (SequenceTracker.Instance.teddyBearProvided)
                {
                    // Should this even happen? Bear could be made non-interactable if already placed
                    Debug.Log("Been there, done that (already had book read");
                    StartCoroutine(PlayInteractionSounds(heldItem, hardNegativeReaction));
                    return false; // No need to try picking up or dropping after this interation
                }
                else
                {
                    Debug.Log("Nah, not yet");
                    StartCoroutine(PlayInteractionSounds(heldItem, softNegativeReaction));
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
                    MusicManager.Instance.ToggleMusic();
                    StartCoroutine(PlayInteractionSounds(heldItem, positiveReactionFlute));
                    return false; // No need to try picking up or dropping after this interation
                }
                else if (SequenceTracker.Instance.flutePlayed)
                {
                    // Could this even happen? Cinematic will have already taken place
                    Debug.Log("Deja vu all over again...");
                    StartCoroutine(PlayInteractionSounds(heldItem, hardNegativeReaction));
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

        Debug.Log("Nope, don't want that");
        StartCoroutine(PlayInteractionSounds(heldItem, hardNegativeReaction));
        return false; // No need to try picking up or dropping after this interation
    }

    IEnumerator PlayInteractionSounds(Pickupable pickup, AudioClip reactionSound)
    {
        pickup.PlayTreeInteractionClip();
        while (pickup.IsPlayingClip())
        {
            yield return null;
        }
        audioSource.clip = reactionSound;
        audioSource.Play();
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
    }
}
