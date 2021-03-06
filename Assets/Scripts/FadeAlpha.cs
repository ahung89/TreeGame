﻿using UnityEngine;
using System.Collections;

public class FadeAlpha : MonoBehaviour
{
    public enum FadeType
    {
        SCurve,
        Linear
    }

    public FadeType fadetype = FadeType.SCurve; // Smooth the interpolation at start and end by default

    public bool startClear = false;

    public float maxAlpha = 0.75f;
    public float minAlpha = 0.25f;

    private Color defaultColor;

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
        // Get the default color and calculate min and max colors
        defaultColor = gameObject.GetComponent<MeshRenderer>().material.color;

        // Calculate fading per second
        fadeInRate = fadeInTime != 0f ? 1f / fadeInTime : 0f;
        fadeOutRate = fadeOutTime != 0f ? 1f / fadeOutTime : 0f;

        // Set to clear depending on flag
        if (startClear)
        {
            gameObject.GetComponent<MeshRenderer>().material.color = new Color(defaultColor.r, defaultColor.g, defaultColor.b, minAlpha);
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

            // Set new alpha given type of curve
            Color color = gameObject.GetComponent<MeshRenderer>().material.color;
            if (fadetype == FadeType.SCurve)
            {
                color.a = Mathf.SmoothStep(minAlpha, maxAlpha, currentWeight);
                gameObject.GetComponent<MeshRenderer>().material.color = color;
            }
            else if (fadetype == FadeType.Linear)
            {
                color.a = Mathf.Lerp(minAlpha, maxAlpha, currentWeight);
                gameObject.GetComponent<MeshRenderer>().material.color = color;
            }

            // Check if fading has completed
            if (isFadingIn && Mathf.Approximately(gameObject.GetComponent<MeshRenderer>().material.color.a, maxAlpha))
            {
                isFadingIn = false;
            }
            else if (isFadingOut && Mathf.Approximately(gameObject.GetComponent<MeshRenderer>().material.color.a, minAlpha))
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
