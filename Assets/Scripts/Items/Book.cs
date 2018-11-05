using UnityEngine;
using System.Collections;

public class Book : Pickupable
{
    public override bool CanInteractWith(Pickupable heldItem)
    {
        return !heldItem && !SequenceTracker.Instance.bookRead;
    }
}
