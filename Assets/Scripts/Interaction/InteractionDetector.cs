using System.Collections.Generic;
using UnityEngine;

public class InteractionDetector : MonoBehaviour {
    public PickupHolder pickupHolder;
    public GameObject player;

    List<Interactable> nearbyInteractibles = new List<Interactable>();

    private void OnTriggerEnter(Collider other)
    {
        Interactable interactable = other.GetComponent<Interactable>();
        
        if (interactable)
        {
            nearbyInteractibles.Add(interactable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Interactable interactable = other.GetComponent<Interactable>();

        if (interactable)
        {
            nearbyInteractibles.Remove(interactable);

            if (interactable.IsInteracting())
            {
                interactable.StopInteracting();
            }
        }
    }

    public void HandleMouseTurn()
    {
        Utils.SortByAngleFromPlayer(nearbyInteractibles, player);
    }

    public void PerformInteractions()
    {
        //SortInteractables();
        Utils.SortByAngleFromPlayer(nearbyInteractibles, player);

        if (nearbyInteractibles.Count > 0)
        {
            foreach (Interactable interactable in nearbyInteractibles)
            {
                if (interactable.IsInteracting())
                {
                    interactable.StopInteracting();
                }
                else if (interactable == nearbyInteractibles[0])
                {
                    interactable.Interact(pickupHolder.GetHeldItem());
                }
            }
        }
    }
}
