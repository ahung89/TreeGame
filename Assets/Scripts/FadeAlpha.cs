using UnityEngine;
using System.Collections;

public class FadeAlpha : MonoBehaviour
{
    public bool startClear = false;

    [Range(0f, Mathf.Infinity)] // fade in time should not be negative
    public float fadeInTime = 5f;
    [Range(0f, Mathf.Infinity)]
    public float fadeOutTime = 5f; // fade out time should not be negative

    private float fadeInRate;
    private float fadeOutRate;

    private Color fullAlphaColor;
    private Color zeroAlphaColor;

    private bool isFadingIn = false;
    private bool isFadingOut = false;

    private void Awake()
    {
        // Get the default color and clear color
        fullAlphaColor = gameObject.GetComponent<MeshRenderer>().material.color;
        zeroAlphaColor = fullAlphaColor;
        zeroAlphaColor.a = 0f;

        // Calculate fading per second
        fadeInRate = fadeInTime != 0f ? 1f / fadeInTime : 0f;
        fadeOutRate = fadeInTime != 0f ? 1f / fadeInTime : 0f;

        // Set to clear depending on flag
        if (startClear)
        {
            gameObject.GetComponent<MeshRenderer>().material.color = zeroAlphaColor;
        }
    }

    private void Update()
    {
        if (isFadingIn && fadeInRate > 0f) // Lerp to Full Alpha
        {
            Color currentColor = gameObject.GetComponent<MeshRenderer>().material.color;
            gameObject.GetComponent<MeshRenderer>().material.color = Color.Lerp(currentColor, fullAlphaColor, fadeInRate * Time.deltaTime);

            if (Mathf.Approximately(currentColor.a, fullAlphaColor.a))
            {
                isFadingIn = false;
            }
        }
        else if (isFadingOut && fadeInRate > 0f) // Lerp to Zero Alpha
        {
            Color currentColor = gameObject.GetComponent<MeshRenderer>().material.color;
            gameObject.GetComponent<MeshRenderer>().material.color = Color.Lerp(currentColor, zeroAlphaColor, fadeOutRate * Time.deltaTime);
            
            if (Mathf.Approximately(currentColor.a, zeroAlphaColor.a))
            {
                isFadingOut = false;
            }
        }
    }

    public void FadeIn()
    {
        isFadingOut = false;
        isFadingIn = true;
    }

    public void FadeOut()
    {
        isFadingIn = false;
        isFadingOut = true;
    }
}
