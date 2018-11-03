using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour {

	public static void SortByAngleFromPlayer(IEnumerable<GameObject> collection, GameObject player)
    {
        //collection.Sort((a, b) =>
        //{
        //    float angleBetweenForwardA = Vector3.Angle(player.transform.forward, a.transform.position - player.transform.position);
        //    float angleBetweenForwardB = Vector3.Angle(player.transform.forward, b.transform.position - player.transform.position);
        //    return angleBetweenForwardA.CompareTo(angleBetweenForwardB);
        //});
    }
}
