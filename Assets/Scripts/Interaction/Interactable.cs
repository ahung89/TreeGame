using UnityEngine;

public class Interactable : MonoBehaviour {

    bool isInteracting = false;

    public bool IsInteracting()
    {
        return isInteracting;
    }

    public virtual void Interact(Pickupable heldItem)
    {
        //Debug.Log("interacting with " + gameObject.name + " using pickupable " + heldItem.name);
        isInteracting = true;
    }

    public virtual void StopInteracting()
    {
        isInteracting = false;
    }
}
