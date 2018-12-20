using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider))]
public class CatchAndRelease : MonoBehaviour
{
    public List<Vector3> releaseLocations = new List<Vector3>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<Pickupable>())
        {
            Debug.Log("Detected Dropped Item");
            other.transform.position = GetNearestDropLocation(other.transform.position);
        }
    }

    private Vector3 GetNearestDropLocation(Vector3 droppedItemPos)
    {
        Vector3 releaseLocation = new Vector3(0f, 1f, 0f); // If all else fails, drop in middle of room
        float sqrDistanceToNearestReleaseLocation = (releaseLocation - droppedItemPos).sqrMagnitude;
        foreach (Vector3 location in releaseLocations)
        {
            float currentSqrDistance = (location - droppedItemPos).sqrMagnitude;
            if (currentSqrDistance < sqrDistanceToNearestReleaseLocation)
            {
                releaseLocation = location;
                sqrDistanceToNearestReleaseLocation = currentSqrDistance;
            }
        }
        return releaseLocation;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        foreach (Vector3 location in releaseLocations)
        {
            Gizmos.DrawWireCube(location, Vector3.one * 0.25f);
        }
    }
}
