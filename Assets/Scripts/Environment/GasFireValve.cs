using UnityEngine;
using System.Collections;

public class GasFireValve : Interactable
{
    public Light fireLight;
    public float noiseFrequency = 1f;
    public float noiseWeight = 1f;
    public float defaultIntensity = 1f;
    public float minIntensity = 0.1f;

    public float dimmingDuration = 3f;
    private bool isDimming = false;
    private bool isDimmed = false;
    private float dimmingElapsed = 0f;

    private void Awake()
    {
        fireLight.intensity = defaultIntensity;
    }

    public override bool CanInteractWith(Pickupable heldItem)
    {
        return !heldItem && !SequenceTracker.Instance.fireOut;
    }

    // Returns whether to TryPickUp or Drop after this action
    public override bool Interact(Pickupable heldItem)
    {
        if (SequenceTracker.Instance.milkConsumed &&
            SequenceTracker.Instance.teddyBearProvided &&
            SequenceTracker.Instance.bookRead &&
            !SequenceTracker.Instance.fireOut)
        {
            isDimming = true;
            Debug.Log("Yaawwwnnnn... Getting sleepy.");
            // elicit a positive reaction
            return false; // Do not try to pick up or drop item after this interaction
        }
        else if (SequenceTracker.Instance.fireOut)
        {
            Debug.Log("DARK... MUST HAVE DARK! (already put out fire)");
            return false; // Do not try to pick up or drop item after this interaction
        }
        else
        {
            Debug.Log("Nah, not yet");
            return false; // No need to try picking up or dropping after this interation
        }
    }

    private void Update()
    {
        float currentIntensity = defaultIntensity;
        if (isDimming)
        {
            dimmingElapsed = Mathf.Clamp(dimmingElapsed + Time.deltaTime, 0f, dimmingDuration);
            // currentIntensity = minIntensity + (defaultIntensity - minIntensity) * (1 - Mathf.Pow(dimmingElapsed / dimmingDuration, 2f));
            currentIntensity = minIntensity + (defaultIntensity - minIntensity) * Mathf.Cos((Mathf.PI * dimmingElapsed) / (dimmingDuration * 2f));
        }
        else if (isDimmed)
        {
            currentIntensity = minIntensity;
        }

        float noise = 0f;
        for (int i = 0; i < 3; i++)
        {
            float oct = Mathf.PerlinNoise(Time.time * Mathf.Pow(noiseFrequency, (float)i), 0f) - 0.5f;
            noise += oct * Mathf.Pow(0.5f, (float)i);
        }
        fireLight.intensity = currentIntensity + (noiseWeight * (currentIntensity/defaultIntensity) * noise);

        if (isDimming && dimmingElapsed >= dimmingDuration)
        {
            isDimmed = true;
            isDimming = false;
        }
    }


}
