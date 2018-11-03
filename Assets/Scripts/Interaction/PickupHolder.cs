using UnityEngine;

public class PickupHolder : MonoBehaviour {

    public Transform holdPoint;
    public InteractionDetector interactionDetector;

    private Pickupable heldItem;

	public void TryPickup()
    {
        // Debug.Log("TryPickup");
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
        if (interactionDetector.GetNearestInteractable())
        {
            Pickupable pickup = interactionDetector.GetNearestInteractable().GetComponent<Pickupable>();
            if (pickup && !pickup.IsHeld())
            {
                DoPickup(pickup);
            }
        }
    }

    void DoPickup(Pickupable pickupable)
    {
        pickupable.Pickup(holdPoint.position, transform);
        heldItem = pickupable;
        OutlineManager.Instance.UnapplyOutline(pickupable.gameObject);
    }

    void DropItem()
    {
        heldItem.Drop();
        heldItem = null;
    }
}
