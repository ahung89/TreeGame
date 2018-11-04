using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : Interactable
{
    public Vector3 bearPlacement = Vector3.zero;

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
                    return false; // No need to try picking up or dropping after this interation
                }
                else
                {
                    Debug.Log("Uh uh... (Already had milk)");
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
                    heldItem.transform.position = bearPlacement;
                    return true; // The Teddy Bear SHOULD be dropped after this interation
                }
                else if (SequenceTracker.Instance.teddyBearProvided)
                {
                    // Should this even happen? Bear could be made non-interactable if already placed
                    Debug.Log("No thanks! (Already received bear)");
                    return false; // No need to try picking up or dropping after this interation
                }
                else
                {
                    Debug.Log("Nah, not yet");
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
                    return false; // No need to try picking up or dropping after this interation
                }
                else if (SequenceTracker.Instance.teddyBearProvided)
                {
                    // Should this even happen? Bear could be made non-interactable if already placed
                    Debug.Log("Been there, done that (already had book read");
                    return false; // No need to try picking up or dropping after this interation
                }
                else
                {
                    Debug.Log("Nah, not yet");
                    return false; // No need to try picking up or dropping after this interation
                }
            }

            // Fire interaction is handled at the fireplace

            // Handle interactions with the Flute
            // if (heldItem.GetComponent<Flute>())
        }

        Debug.Log("Nope, don't want that");
        return false; // No need to try picking up or dropping after this interation
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(bearPlacement, Vector3.one * 0.25f);
    }
}
