﻿using UnityEngine;
using System.Collections;

public class Sink : Interactable
{
    public float waterStreamDuration = 2f;
    private float waterStreamElapsed = 0f;
    private bool waterStreamRunning = false;

    public GameObject waterStream;

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
            // No need to try picking up or dropping after this interation
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