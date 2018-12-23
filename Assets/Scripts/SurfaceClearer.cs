using UnityEngine;
using System.Collections;

public class SurfaceClearer : MonoBehaviour
{
    public Vector3 clearAreaPosition = Vector3.zero;
    public Vector3 clearAreaSize = Vector3.one;
    public float boxCastDistance = 0.5f;

    public Vector3 dropLocation = Vector3.one;

    public void ClearArea()
    {
        RaycastHit[] hits = Physics.BoxCastAll(clearAreaPosition, clearAreaSize * 0.5f, Vector3.down, Quaternion.identity, boxCastDistance);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.GetComponent<Pickupable>())
            {
                hit.transform.position = dropLocation;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(clearAreaPosition, clearAreaSize);
        Gizmos.DrawLine(clearAreaPosition, clearAreaPosition + (Vector3.down * boxCastDistance));
        Gizmos.DrawWireCube(clearAreaPosition + (Vector3.down * boxCastDistance), clearAreaSize);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(dropLocation, 0.125f);
    }
}
