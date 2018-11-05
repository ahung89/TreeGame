using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cup : Pickupable {

    public enum Liquid { Milk, Water, None }
    public Liquid currentLiquid = Liquid.None;
    public Renderer fillRenderer;
    public AudioClip cupFillClip;

    public float fillAmount = .46f;
    public Color milkColor = Color.white;
    public Color waterColor = Color.blue;

    public override bool CanInteractWith(Pickupable heldItem)
    {
        if (SequenceTracker.Instance.milkConsumed) return false;

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

    // Returns whether to TryPickUp or Drop after this action
    public override bool Interact(Pickupable heldItem)
    {
        // Debug.Log("Interact with : " + heldItem);
        if (CanInteractWith(heldItem))
        {
            base.Interact(heldItem);

            if (heldItem)
            {
                bool fillCup = false;
                Liquid liquid = Liquid.None;

                if (heldItem.GetComponent<Milk>())
                {
                    fillCup = true;
                    liquid = Liquid.Milk;
                }
                else if (heldItem.GetComponent<Water>())
                {
                    fillCup = true;
                    liquid = Liquid.Water;
                }

                if (fillCup)
                {
                    FillCup(liquid);
                    if (!IsPlayingClip())
                    {
                        PlayClip(cupFillClip);
                    }
                    return false;
                }
            }
        }
        return true;
    }

    public void FillCup(Liquid liquid)
    {
        if (liquid == Liquid.Milk)
        {
            // modify model to have milk inside of it
            Debug.Log("Cup filled with Milk");
            currentLiquid = Liquid.Milk;
            FillCup(milkColor);
        }
        else if (liquid == Liquid.Water)
        {
            // modify model to have water inside of it
            Debug.Log("Cup filled with Water");
            currentLiquid = Liquid.Water;
            FillCup(waterColor);
        }
    }

    void FillCup(Color col)
    {
        fillRenderer.material.SetColor("_TopColor", col);
        fillRenderer.material.SetFloat("_FillAmount", fillAmount);
    }

    // call this from the tree object if the wrong liquid is given to the tree
    public void EmptyCup()
    {
        // modify model to be empty
        currentLiquid = Liquid.None;
        fillRenderer.material.SetFloat("_FillAmount", 1);
    }
}
