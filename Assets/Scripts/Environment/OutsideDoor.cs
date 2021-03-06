﻿using UnityEngine;
using System.Collections;

public class OutsideDoor : Interactable
{
    AudioSource doorRattle;
    DoorShake shakeEffect;

    void Awake()
    {
        doorRattle = gameObject.GetComponent<AudioSource>();
        shakeEffect = gameObject.GetComponent<DoorShake>();
    }

    public override bool CanInteractWith(Pickupable heldItem)
    {
        return !heldItem;
    }

    public override bool Interact(Pickupable heldItem)
    {
        if (!doorRattle.isPlaying)
        {
            doorRattle.Play();

            if (shakeEffect)
            {
                shakeEffect.StartShake();
            }
        }
        return false;
    }
}
