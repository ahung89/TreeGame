using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : Interactable {

    Rigidbody rb;
    bool isHeld;

    public AudioClip treeInteractionClip;
    private AudioSource audioSource;

	void Awake () {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}

    public override bool CanInteractWith(Pickupable heldItem)
    {
        return !heldItem;
    }

    public void Pickup(Vector3 holdPoint, Transform parent)
    {
        isHeld = true;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        rb.transform.parent = parent;
        rb.transform.position = holdPoint;
        rb.transform.rotation = Quaternion.identity;
        rb.detectCollisions = false;
    }

    public void Drop()
    {
        isHeld = false;
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.None;
        rb.transform.parent = null;
        rb.isKinematic = false;
        rb.detectCollisions = true;
    }

    public bool IsHeld()
    {
        return isHeld;
    }

    public void PlayTreeInteractionClip()
    {
        PlayClip(treeInteractionClip);
    }

    public bool IsPlayingClip()
    {
        return audioSource.isPlaying;
    }

    protected void PlayClip(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}
