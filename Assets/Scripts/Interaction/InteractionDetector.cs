using System.Collections.Generic;
using UnityEngine;

public class InteractionDetector : MonoBehaviour {
    public PickupHolder pickupHolder;
    public GameObject player;

    List<Interactable> interactablesInRange = new List<Interactable>();
    Interactable nearestInteractable;

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log("OnTriggerEnter: " + other);

        Interactable interactable = other.GetComponent<Interactable>();

        // Debug.Log("iteractable: " + interactable);
        // Debug.Log("CanInteractWith: " + interactable.CanInteractWith(pickupHolder.GetHeldItem()));

        if (interactable != null && interactable.CanInteractWith(pickupHolder.GetHeldItem()))
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

            Pickupable pickup = interactablesInRange[0].GetComponent<Pickupable>();

            // don't highlight pickupables if you're already holding something
            //if (!(pickup && pickupHolder.GetHeldItem()))
            //{
                OutlineManager.Instance.ApplyOutline(interactablesInRange[0].gameObject);
            //}

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
