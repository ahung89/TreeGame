using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawer : Interactable {

    public Vector3 openExtendVector;
    public float openCloseDuration;

    bool isOpen;
    Vector3 closedPosition;
    Vector3 openPosition;
    Vector3 velocity;

    private void Awake()
    {
        closedPosition = transform.position;
    }

    private void Update()
    {
        // put this in update so we can tweak it in inspector
        openPosition = closedPosition + openExtendVector;

        gameObject.transform.position = Vector3.SmoothDamp(gameObject.transform.position, isOpen ? openPosition : closedPosition, ref velocity, openCloseDuration);
    }

    public override bool Interact(Pickupable heldItem)
    {
        base.Interact(heldItem);
        isOpen = !isOpen;

        StopInteracting(); // this interaction is instantaneous

        return false; // No need to try picking up or dropping after this interation
    }
}
