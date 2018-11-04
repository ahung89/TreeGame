﻿using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour {

    public Camera playerCamera;
    public InteractionDetector interactionDetector;
    public PickupHolder pickupHolder;
    public float lookSpeedMultiplier;

    public UnityEvent lookRotationEvent;

    public GameObject startUI;

    Movement movement;
    float cameraRotation = 0;
    bool gameStarted = false;

	void Start () {
        movement = GetComponent<Movement>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        cameraRotation = playerCamera.transform.rotation.eulerAngles.x;
    }
	
	void Update () {
        if (gameStarted)
        {
            float forward = Input.GetAxisRaw("Vertical");
            float strafe = Input.GetAxisRaw("Horizontal");

            float lookX = Input.GetAxis("Mouse X");
            float lookY = Input.GetAxis("Mouse Y");

            bool interact = Input.GetMouseButtonDown(0);
            bool pickup = Input.GetMouseButtonDown(0);

            cameraRotation = Mathf.Clamp(cameraRotation - lookY, -89, 89);
            playerCamera.transform.localRotation = Quaternion.AngleAxis(cameraRotation, Vector3.right);

            Vector3 moveVec = Vector3.forward * forward + Vector3.right * strafe;
            movement.Move(moveVec.normalized);

            bool tryPickup = true;

            if (interact)
            {
                tryPickup = interactionDetector.PerformInteractions();
            }

            if (pickup && tryPickup)
            {
                // Debug.Log("Now Try PickUp");
                pickupHolder.TryPickup();
            }

            if (lookY > 0 || lookX > 0)
            {
                lookRotationEvent.Invoke();
            }

            movement.AddToYaw(lookX);
        }
        else if (Input.GetMouseButtonDown(0))
        {
            startUI.SetActive(false);
            gameStarted = true;
        }
	}
}
