using UnityEngine;

public class Interactable : MonoBehaviour {

    MeshRenderer rend;
    bool isInteracting = false;

    private void Awake()
    {
        rend = GetComponent<MeshRenderer>();
    }

    public bool IsInteracting()
    {
        return isInteracting;
    }

    public virtual void StartInteracting(Pickupable heldItem)
    {
        isInteracting = true;
    }

    public virtual void StopInteracting()
    {
        isInteracting = false;
    }
}
