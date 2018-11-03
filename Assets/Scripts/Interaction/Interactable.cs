using UnityEngine;

public class Interactable : MonoBehaviour {

    bool isInteracting = false;

    public bool IsInteracting()
    {
        return isInteracting;
    }

    public virtual void Interact(Pickupable heldItem)
    {
        isInteracting = true;
    }

    public virtual void StopInteracting()
    {
        isInteracting = false;
    }
}
