using UnityEngine;
using System.Collections;

public class Drain : Interactable
{
    public override bool CanInteractWith(Pickupable heldItem)
    {
        // For the moment, let's not simply prevent Drain interaction
        return false;

        // Only allow interaction with cup to empty it
        if (heldItem)
        {
            Cup cup = heldItem.GetComponent<Cup>();
            if (cup && cup.currentLiquid != Cup.Liquid.None)
            {
                return true;
            }
        }
        return false;
    }

    public override bool Interact(Pickupable heldItem)
    {
        if (CanInteractWith(heldItem))
        {
            Cup cup = heldItem.GetComponent<Cup>();
            if (cup)
            {
                cup.EmptyCup();
                cup.PlayTreeInteractionClip();
            }
        }
        return false; // For all items, do not allow picking up or dropping held item while interacting with the Drain
    }
}
