using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawer : Interactable {

    public Vector3 openExtendVector;
    public float openCloseDuration;

    bool isOpen;
    Vector3 closedPosition;
    Vector3 openedPosition;
    Rigidbody rb;

    private void Awake()
    {
        closedPosition = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // put this in update so we can tweak it in inspector
        openedPosition = closedPosition + openExtendVector;
    }

    public override void Interact(Pickupable heldItem)
    {
        base.Interact(heldItem);
        isOpen = !isOpen;

        if (isOpen)
        {
            rb.MovePosition(openedPosition);
        }
        else
        {
            rb.MovePosition(closedPosition);
        }
    }
}
