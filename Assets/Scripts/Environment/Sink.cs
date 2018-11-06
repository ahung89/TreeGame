using UnityEngine;
using System.Collections;

public class Sink : Interactable
{
    public float waterStreamDuration = 2f;
    private float waterStreamElapsed = 0f;
    private bool waterStreamRunning = false;

    public GameObject waterStream;

    public AudioSource watterRunningAudioSource;

    private void Awake()
    {
        waterStream.SetActive(false);
    }

    public override bool CanInteractWith(Pickupable heldItem)
    {
        return heldItem == null || heldItem.GetComponent<Cup>();
    }

    public override bool Interact(Pickupable heldItem)
    {
        Debug.Log("Interact with Sink");
        if (CanInteractWith(heldItem))
        {
            if (!waterStreamRunning)
            {
                waterStream.SetActive(true);
                waterStreamRunning = true;
                waterStreamElapsed = 0f;
            }

            // Play water running sound
            if (watterRunningAudioSource && watterRunningAudioSource.clip && !watterRunningAudioSource.isPlaying)
            {
                watterRunningAudioSource.Play();
            }

            if (heldItem)
            {
                Cup cup = heldItem.GetComponent<Cup>();
                if (cup)
                {
                    cup.FillCup(Cup.Liquid.Water);
                    if (!cup.IsPlayingClip())
                    {
                        cup.PlayClip(cup.cupFillClip);
                    }
                }
            }
        }
        return false; // No need to try picking up or dropping after this interation
    }

    private void Update()
    {
        if (waterStreamRunning)
        {
            waterStreamElapsed += Time.deltaTime;
            
            if (waterStreamElapsed >= waterStreamDuration)
            {
                waterStreamRunning = false;
                waterStream.SetActive(false);
            }
        }
    }
}
