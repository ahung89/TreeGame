﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawer : Interactable {

    public Vector3 openExtendVector;
    public float openCloseDuration;

    bool isOpen;
    Vector3 closedPosition;
    Vector3 openPosition;
    Rigidbody rb;

    private void Awake()
    {
        closedPosition = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // put this in update so we can tweak it in inspector
        openPosition = closedPosition + openExtendVector;
    }

    IEnumerator Open()
    {
        float totalDistance = openExtendVector.magnitude;
        float movedDistance = 0;

        while (movedDistance < totalDistance && isOpen)
        {
            float accumDistance = Mathf.Min((openPosition - transform.position).magnitude,
                openExtendVector.magnitude * Time.deltaTime / openCloseDuration);
            rb.MovePosition(transform.position + accumDistance * openExtendVector.normalized);
            movedDistance += accumDistance;
            yield return null;
        }
    }

    IEnumerator Close()
    {
        float totalDistance = openExtendVector.magnitude;
        float accumDistance = 0;

        while (accumDistance < totalDistance && !isOpen)
        {
            float incrDistance = Mathf.Min((openPosition - rb.position).magnitude,
                openExtendVector.magnitude * Time.deltaTime / openCloseDuration);
            rb.MovePosition(rb.position - incrDistance * openExtendVector.normalized);
            accumDistance += incrDistance;
            yield return null;
        }
    }

    public override void Interact(Pickupable heldItem)
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
    }
}
