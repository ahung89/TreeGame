using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable {

    public float rotationDuration;
    public float rotationAmount = 90;
    public bool isVerticalHinge = true;

    bool isOpen = false;
    float closedAngle;
    float openAngle;
    float currentAngle;
    float velocity;

    Vector3 RotationAxis { get { return isVerticalHinge ? Vector3.up : Vector3.right; } }

    void Awake()
    {
        currentAngle = Vector3.Dot(RotationAxis, transform.rotation.eulerAngles);
        closedAngle = currentAngle;
        openAngle = closedAngle + rotationAmount;
    }

    void Update()
    {
        currentAngle = Mathf.SmoothDamp(currentAngle, isOpen ? openAngle : closedAngle, ref velocity, rotationDuration);
        transform.rotation = Quaternion.AngleAxis(currentAngle, RotationAxis);
    }

    public override bool Interact(Pickupable heldItem)
    {
        base.Interact(heldItem);
        isOpen = !isOpen;

        StopInteracting(); // this interaction is instantaneous

        return false; // No need to try picking up or dropping after this interation
    }
}
