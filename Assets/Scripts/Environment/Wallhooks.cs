using UnityEngine;
using System.Collections;

public class Wallhooks : Interactable
{
    AudioSource pansRattle;
    PotPanShake[] shakeEffects;

    void Awake()
    {
        pansRattle = gameObject.GetComponent<AudioSource>();
        shakeEffects = gameObject.GetComponentsInChildren<PotPanShake>();
    }

    public override bool CanInteractWith(Pickupable heldItem)
    {
        return !heldItem;
    }

    public override bool Interact(Pickupable heldItem)
    {
        if (!pansRattle.isPlaying)
        {
            pansRattle.Play();

            foreach (PotPanShake shakeEffect in shakeEffects)
            {
                shakeEffect.StartShake();
            }
        }
        return false;
    }
}
