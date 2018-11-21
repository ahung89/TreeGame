using UnityEngine;
using System.Collections;

public class FaucetHandleTurn : MonoBehaviour
{
    private float turnOnCount = 0.5f;
    public float turnOnTime = 1f;
    public float onRotation = 50f;

    private float turnOffCount = 0.5f;
    public float turnOffTime = 1f;
    public float offRotation = 50f;

    private float turnOnTimeRemaining = 0f;
    private float turnOffTimeRemaining = 0f;
    private Vector3 defaultRotation;

    private void Awake()
    {
        defaultRotation = transform.localRotation.eulerAngles;
    }

    private void Update()
    {
        if (turnOnTimeRemaining > 0f)
        {
            float newYRotation = defaultRotation.y + Mathf.Sin(Mathf.PI * (turnOnTime - turnOnTimeRemaining) * (turnOnCount / turnOnTime)) * onRotation;
            this.transform.localRotation = Quaternion.Euler(defaultRotation.x, newYRotation, defaultRotation.z);
        }
        else if (turnOffTimeRemaining > 0f)
        {
            float newYRotation = defaultRotation.y + Mathf.Cos(Mathf.PI * (turnOffTime - turnOffTimeRemaining) * (turnOffCount / turnOffTime)) * offRotation;
            this.transform.localRotation = Quaternion.Euler(defaultRotation.x, newYRotation, defaultRotation.z);
        }

        if (turnOnTimeRemaining > 0f)
        {
            turnOnTimeRemaining -= Time.deltaTime;
        }

        if (turnOffTimeRemaining > 0f)
        {
            turnOffTimeRemaining -= Time.deltaTime;
        }
    }

    public void StartTurnOn()
    {
        turnOnTimeRemaining = turnOnTime;
    }

    public void StartTurnOff()
    {
        turnOffTimeRemaining = turnOffTime;
    }
}
