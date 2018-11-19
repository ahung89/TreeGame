using UnityEngine;
using System.Collections;

public class GasValveTurn : MonoBehaviour
{
    private float fullTurnCount = 0.5f;
    public float fullTurnTime = 1f;
    public float fullRotation = -180f;

    private int partialTurnCount = 1;
    public float partialTurnTime = 0.5f;
    public float partialRotation = -45f;

    private float fullTurnTimeRemaining = 0f;
    private float partialTurnTimeRemaining = 0f;
    private Vector3 defaultRotation;

    private void Awake()
    {
        defaultRotation = transform.localRotation.eulerAngles;
    }

    private void Update()
    {
        if (fullTurnTimeRemaining > 0f)
        {
            float newXRotation = defaultRotation.x + Mathf.Sin(Mathf.PI * (fullTurnTime - fullTurnTimeRemaining) * (fullTurnCount / fullTurnTime)) * fullRotation;
            this.transform.localRotation = Quaternion.Euler(newXRotation, defaultRotation.y, defaultRotation.z);
        }
        else if (partialTurnTimeRemaining > 0f)
        {
            float newXRotation = defaultRotation.x + Mathf.Sin(Mathf.PI * (partialTurnTime - partialTurnTimeRemaining) * (partialTurnCount / partialTurnTime)) * partialRotation;
            this.transform.localRotation = Quaternion.Euler(newXRotation, defaultRotation.y, defaultRotation.z);
        }

        if (fullTurnTimeRemaining > 0f)
        {
            fullTurnTimeRemaining -= Time.deltaTime;
        }

        if (partialTurnTimeRemaining > 0f)
        {
            partialTurnTimeRemaining -= Time.deltaTime;
        }
    }

    public void StartFullTurn()
    {
        fullTurnTimeRemaining = fullTurnTime;
    }

    public void StartPartialTurn()
    {
        partialTurnTimeRemaining = partialTurnTime;
    }
}
