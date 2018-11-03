using UnityEngine;

public class Interactable : MonoBehaviour {

    bool isInteracting = false;

    public bool IsInteracting()
    {
        return isInteracting;
    }

    public virtual bool CanInteractWith(Pickupable heldItem)
    {
        // By default, the object should not be interactable while the player is already holding something
        // Some objects may only be capable of being interacted with when the correct item is being held
        return heldItem == null;
    }

    public virtual bool Interact(Pickupable heldItem)
    {
        //Debug.Log("interacting with " + gameObject.name + " using pickupable " + heldItem.name);
        // isInteracting = true; // For now, let's ignore the isInteracting variable
        return true; // the returned bool indicates whether PlayerInput should call PickupHolder.TryPickup() following this interaction
    }

    public virtual void StopInteracting()
    {
        isInteracting = false;
    }
}
