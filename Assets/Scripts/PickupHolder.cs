using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHolder : MonoBehaviour {

    public Transform holdPoint;
    public PickupDetector pickupDetector;
    public InteractionDetector interactionDetector;

    private Pickupable heldItem;

	public void TryPickup()
    {
        if (heldItem != null)
        {
            DropItem();
        }
        else
        {
            PickupItem();
        }
    }

    public Pickupable GetHeldItem()
    {
        return heldItem;
    }

    void PickupItem()
    {
        foreach (Pickupable pickup in pickupDetector.GetPickupsInRange())
        {
            if (!pickup.IsHeld())
            {
                DoPickup(pickup);
                return;
            }
        }
    }

    void DoPickup(Pickupable pickupable)
    {
        pickupable.Pickup(holdPoint.position, transform);
        heldItem = pickupable;
    }

    void DropItem()
    {
        heldItem.Drop();
        heldItem = null;
    }
}
