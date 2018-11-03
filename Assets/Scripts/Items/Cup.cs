using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cup : Pickupable {

    public enum Liquid { Milk, Water, None }
    public Liquid currentLiquid = Liquid.None;

    public override void Interact(Pickupable heldItem)
    {
        base.Interact(heldItem);
        FillCup(heldItem);
    }

    void FillCup(Pickupable heldItem)
    {
        if (heldItem.GetComponent<MilkCarton>())
        {
            // modify model to have milk inside of it

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
