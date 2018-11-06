using UnityEngine;
using System.Collections;

public class DoorShake : MonoBehaviour
{
    public int shakeCount = 1;
    public float shakeTime = 1f;
    public float maxRotation = 5f;

    private float shakeTimeRemaining = 0f;
    private Vector3 defaultRotation;

    private void Awake()
    {
        defaultRotation = transform.localRotation.eulerAngles;
    }

    private void Update()
    {
        if (shakeTimeRemaining > 0f)
        {
            float newXRotation = defaultRotation.x + Mathf.Sin(Mathf.PI * (shakeTime - shakeTimeRemaining) * (shakeCount / shakeTime)) * maxRotation;
            this.transform.localRotation = Quaternion.Euler(newXRotation, defaultRotation.y, defaultRotation.z);
            shakeTimeRemaining -= Time.deltaTime;
        }
    }

    public void StartDoorShake()
    {
        shakeTimeRemaining = shakeTime;
    }
}
