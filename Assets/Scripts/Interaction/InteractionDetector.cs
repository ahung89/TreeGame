using System.Collections.Generic;
using UnityEngine;

public class InteractionDetector : MonoBehaviour {
    public PickupHolder pickupHolder;
    public GameObject player;

    List<Interactable> interactablesInRange = new List<Interactable>();
    Interactable nearestInteractable;

    private void OnTriggerEnter(Collider other)
    {
        Interactable interactable = other.GetComponent<Interactable>();
        
        if (interactable)
        {
            interactablesInRange.Add(interactable);
            UpdateNearestInteractable();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Interactable interactable = other.GetComponent<Interactable>();

        if (interactable)
        {
            interactablesInRange.Remove(interactable);
            OutlineManager.Instance.UnapplyOutline(interactable.gameObject);
            UpdateNearestInteractable();

            if (interactable.IsInteracting())
            {
                interactable.StopInteracting();
            }
        }
    }

    public Interactable GetNearestInteractable()
    {
        return nearestInteractable;
    }

    public void UpdateNearestInteractable()
    {
        Utils.SortByAngleFromPlayer(interactablesInRange, player);
    }

    private void Update()
    {
        if (interactablesInRange.Count == 0)
        {
            nearestInteractable = null;
            return;
        }

        if (nearestInteractable != interactablesInRange[0])
        {
            if (nearestInteractable && nearestInteractable != interactablesInRange[0])
            {
                OutlineManager.Instance.UnapplyOutline(nearestInteractable.gameObject);
            }

            OutlineManager.Instance.ApplyOutline(interactablesInRange[0].gameObject);
            nearestInteractable = interactablesInRange[0];
        }
    }

    public void PerformInteractions()
    {
        UpdateNearestInteractable();

        if (interactablesInRange.Count > 0)
        {
            foreach (Interactable interactable in interactablesInRange)
            {
                if (interactable.IsInteracting())
                {
                    interactable.StopInteracting();
                }
                else if (interactable == interactablesInRange[0])
                {
                    interactable.Interact(pickupHolder.GetHeldItem());
                }
            }
        }
    }
}
