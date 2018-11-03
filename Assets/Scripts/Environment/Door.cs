using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable {

    public float rotationDuration;
    public float rotationAmount = 90;

    bool isOpen = false;
    Quaternion closedRotation;
    Quaternion openRotation;
    Rigidbody rb;

    private void Awake()
    {
        closedRotation = transform.rotation;
        openRotation = closedRotation * Quaternion.AngleAxis(rotationAmount, Vector3.up);
        rb = GetComponent<Rigidbody>();
    }

    IEnumerator Open()
    {
        float accumAngle = 0;

        while (accumAngle < rotationAmount && isOpen)
        {
            float incrAngle = Mathf.Min(Quaternion.Angle(rb.rotation, openRotation),
                rotationAmount * Time.deltaTime / rotationDuration);
            rb.MoveRotation(rb.rotation * Quaternion.AngleAxis(incrAngle, Vector3.up));
            accumAngle += incrAngle;
            yield return null;
        }
    }

    IEnumerator Close()
    {
        float accumAngle = 0;

        while (accumAngle < rotationAmount && !isOpen)
        {
            float incrAngle = Mathf.Min(Quaternion.Angle(rb.rotation, closedRotation),
                rotationAmount * Time.deltaTime / rotationDuration);
            rb.MoveRotation(rb.rotation * Quaternion.AngleAxis(-incrAngle, Vector3.up));
            accumAngle += incrAngle;
            yield return null;
        }
    }

    public override bool Interact(Pickupable heldItem)
    {
        base.Interact(heldItem);
        isOpen = !isOpen;

        if (isOpen)
        {
            StartCoroutine(Open());
        }
        else
        {
            StartCoroutine(Close());
        }

        StopInteracting(); // this interaction is instantaneous

        return false; // No need to try picking up or dropping after this interation
    }
}
