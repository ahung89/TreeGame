using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeVolume : MonoBehaviour
{
    public enum FadeType
    {
        SCurve,
        Linear
    }

    public AudioSource audioSource;

    public FadeType fadetype = FadeType.SCurve; // Smooth the interpolation at start and end by default

    public bool startAtMinVolume = false;

    public float maxVolume = 0.75f;
    public float minVolume = 0.25f;

    [Range(0f, Mathf.Infinity)] // fade in time should not be negative
    public float fadeInTime = 5f;
    [Range(0f, Mathf.Infinity)]
    public float fadeOutTime = 5f; // fade out time should not be negative

    private float fadeInRate;
    private float fadeOutRate;

    private bool isFadingIn = false;
    private bool isFadingOut = false;

    private float currentWeight = 1f; // This is equivalent to the 't' variable in the interpolation functions

    private void Awake()
    {
        // Calculate fading per second
        fadeInRate = fadeInTime != 0f ? 1f / fadeInTime : 0f;
        fadeOutRate = fadeOutTime != 0f ? 1f / fadeOutTime : 0f;

        // Set to minimum volume at start depending on flag
        if (startAtMinVolume)
        {
            audioSource.volume = minVolume;
            currentWeight = 0f;
        }
    }

    private void Update()
    {
        // Only perform these calculations while fading and fade rates are non-zero
        if (isFadingIn && fadeInRate > 0f || isFadingOut && fadeInRate > 0f)
        {
            // Calculate the new weight value, i.e. the weight of the max value
            if (isFadingIn)
            {
                currentWeight = Mathf.Clamp01(currentWeight + (fadeInRate * Time.deltaTime));
            }
            else if (isFadingOut)
            {
                currentWeight = Mathf.Clamp01(currentWeight - (fadeOutRate * Time.deltaTime));
            }

            // Set new volume given type of curve
            if (fadetype == FadeType.SCurve)
            {
                audioSource.volume = Mathf.SmoothStep(minVolume, maxVolume, currentWeight);
            }
            else if (fadetype == FadeType.Linear)
            {
                audioSource.volume = Mathf.Lerp(minVolume, maxVolume, currentWeight);
            }

            // Check if fading has completed
            if (isFadingIn && Mathf.Approximately(audioSource.volume, maxVolume))
            {
                isFadingIn = false;
            }
            else if (isFadingOut && Mathf.Approximately(audioSource.volume, minVolume))
            {
                isFadingOut = false;
            }
        }
    }

    public void StartFadeIn()
    {
        isFadingOut = false;
        isFadingIn = true;
    }

    public void StartFadeOut()
    {
        isFadingIn = false;
        isFadingOut = true;
    }
}
