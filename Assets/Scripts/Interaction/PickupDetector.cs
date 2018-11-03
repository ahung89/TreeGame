using System.Collections.Generic;
using UnityEngine;

public class PickupDetector : MonoBehaviour {

    private HashSet<Pickupable> pickupsInRange = new HashSet<Pickupable>();

    private void OnTriggerEnter(Collider other)
    {
        Pickupable pickup = other.GetComponent<Pickupable>();
        if (pickup != null)
        {
            pickupsInRange.Add(pickup);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Pickupable pickup = other.GetComponent<Pickupable>();
        if (pickupsInRange.Contains(pickup))
        {
            pickupsInRange.Remove(pickup);
        }
    }

    public HashSet<Pickupable> GetPickupsInRange()
    {
        return pickupsInRange;
    }
}
