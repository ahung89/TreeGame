using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour {

    public Camera playerCamera;
    public InteractionDetector interactionDetector;
    public PickupHolder pickupHolder;
    public float lookSpeedMultiplier;

    public UnityEvent lookRotationEvent;

    Movement movement;
    float cameraRotation = 0;

	void Start () {
        movement = GetComponent<Movement>();
	}
	
	void Update () {
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

        if (interact)
        {
            interactionDetector.PerformInteractions();
        }

        if (pickup)
        {
            pickupHolder.TryPickup();
        }

        if (lookY > 0 || lookX > 0)
        {
            lookRotationEvent.Invoke();
        }

        movement.AddToYaw(lookX);
	}
}
