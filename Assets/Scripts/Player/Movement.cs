using UnityEngine;

public class Movement : MonoBehaviour {

    public float speed;

    Vector3 movementVector;

    Rigidbody rb;

	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	void FixedUpdate () {
        rb.velocity = transform.forward * movementVector.z * speed + transform.right * movementVector.x * speed; ;
    }

    public void Move(Vector3 movementVector)
    {
        this.movementVector = movementVector;
    }

    public void AddToYaw(float yaw)
    {
        transform.rotation *= Quaternion.AngleAxis(yaw, Vector3.up);
    }
}
