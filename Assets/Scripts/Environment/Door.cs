using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable {

    public float rotationDuration;

    bool isOpen;
    Quaternion closedRotation;
    Quaternion openRotation;
    Rigidbody rb;

    private void Awake()
    {
        closedRotation = transform.rotation;
    }

    IEnumerator Open()
    {
        yield return null;
    }

    IEnumerator Close()
    {
        yield return null;
    }

    public override void Interact(Pickupable heldItem)
    {
        base.Interact(heldItem);
        isOpen = !isOpen;

        if (isOpen)
        {

        }
        else
        {

        }
    }
}
