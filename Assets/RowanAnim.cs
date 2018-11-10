using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowanAnim : MonoBehaviour {

    Animator anim;

	void Start () {
        anim = GetComponent<Animator>();		
	}
	
	public void Chill()
    {
        Debug.Log("Idle");
        anim.SetTrigger("Idle");
    }
}
