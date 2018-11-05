using UnityEngine;
using System.Collections;

public class Flute : Pickupable
{
    public override bool CanInteractWith(Pickupable heldItem)
    {
        return !heldItem && !SequenceTracker.Instance.flutePlayed;
    }
}
