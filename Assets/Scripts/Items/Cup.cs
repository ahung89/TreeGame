﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cup : Pickupable {

    public enum Liquid { Milk, Water, None }
    public Liquid currentLiquid = Liquid.None;

    public override bool CanInteractWith(Pickupable heldItem)
    {
        if (heldItem)
        {
            if (heldItem.GetComponent<Milk>())
            {
                return true;
            }
            else if (heldItem.GetComponent<Water>())
            {
                return true;
            }
            else
            {
                return false; // otherwise, player should not be able to add other objects to cup
            }
        }
        else
        {
            return true; // when nothing is held, the player should be able to pick up the cup
        }
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
        if (heldItem.GetComponent<Milk>())
        {
            // modify model to have milk inside of it
            Debug.Log("Cup filled with Milk");
            currentLiquid = Liquid.Milk;
        }
        else if (heldItem.GetComponent<Water>())
        {
            // modify model to have water inside of it
            Debug.Log("Cup filled with Water");
            currentLiquid = Liquid.Water;
        }
    }

    // call this from the tree object if the wrong liquid is given to the tree
    public void EmptyCup()
    {
        // modify model to be empty
        currentLiquid = Liquid.None;
    }
}
