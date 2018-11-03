using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cup : Pickupable {

    public enum Liquid { Milk, Water, None }
    public Liquid currentLiquid = Liquid.None;

    public override bool CanInteractWith(Pickupable heldItem)
    {
        if (heldItem)
        {
            MilkCarton milk = heldItem.GetComponent<MilkCarton>();
            // Get water

            if (milk)
            {
                return true;
            }
            // else if water, return true
            // else, return false, i.e. should not be able to add other objects to cup
        }
        else
        {
            return true;
        }

        return false; // Otherwise, return false by default
    }

    public override void Interact(Pickupable heldItem)
    {
        if (CanInteractWith(heldItem))
        {
            base.Interact(heldItem);

            if (heldItem)
            {
                FillCup(heldItem);
            }
        }
    }

    void FillCup(Pickupable heldItem)
    {
        if (heldItem.GetComponent<MilkCarton>())
        {
            // modify model to have milk inside of it
            Debug.Log("Cup filled with Milk");
            currentLiquid = Liquid.Milk;
        }
    }

    // call this from the tree object if the wrong liquid is given to the tree
    public void EmptyCup()
    {
        // modify model to be empty
        currentLiquid = Liquid.None;
    }
}
