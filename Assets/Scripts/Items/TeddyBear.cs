using UnityEngine;
using System.Collections;

public class TeddyBear : Pickupable
{
    public override bool CanInteractWith(Pickupable heldItem)
    {
        return !heldItem && !SequenceTracker.Instance.teddyBearProvided;
    }


}
