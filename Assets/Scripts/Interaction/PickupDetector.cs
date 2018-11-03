using System.Collections.Generic;
using UnityEngine;

public class PickupDetector : MonoBehaviour {

    public GameObject player;
    private List<Pickupable> pickupsInRange = new List<Pickupable>();

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

    public void HandleMouseTurn()
    {
        Utils.SortByAngleFromPlayer(pickupsInRange, player);
    }

    public List<Pickupable> GetPickupsInRange()
    {
        return pickupsInRange;
    }
}
