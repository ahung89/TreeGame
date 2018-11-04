using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Environment
{
    class SoundInteraction : Interactable
    {
        AudioSource source;

        void Awake()
        {
            source = gameObject.GetComponent<AudioSource>();
        }

        public override bool CanInteractWith(Pickupable heldItem)
        {
            return !heldItem;
        }

        public override bool Interact(Pickupable heldItem)
        {
            if (!source.isPlaying)
            {
                source.Play();
            }
            return false;
        }
    }
}
