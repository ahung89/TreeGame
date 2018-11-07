using UnityEngine;
using System.Collections;

public class Book : Pickupable
{
    public float orientBookTime = 0.1f; // Rotates book around Y axis to be readable right-side up
    public float rotateBookTime = 0.3f; // Turns the book vertical with spine facing down
    public float openBookTime = 0.8f; // Opens both covers of the book
    public float keepOpenTime = 0.8f; // Pauses keeping the book open
    public float closeBookTime = 0.5f; // Closes both coveres of the book
    public float rotateBackTime = 0.2f; // Rotates the book back flat

    public GameObject frontCover;
    public GameObject backCover;

    public AudioClip pageTurning;
    public AudioClip pagesFlipping;
    public AudioClip bookReading;
    public AudioClip bookClosing;

    private bool animationStarted = false;
    private bool orientBookComplete = false;
    private bool rotateBookComplete = false;
    private bool openBookComplete = false;
    private bool keepOpenComplete = false;
    private bool closeBookComplete = false;
    private bool rotateBackComplete = false;

    private Quaternion startingRotation;
    private Vector3 orientBookRotation = new Vector3(-45f, 0f, 0f);
    private Vector3 rotateBookRotation = new Vector3(-45f, 0f, 90f);
    private Vector3 openFrontRotation = new Vector3(0f, 0f, 90f);
    private Vector3 openBackRotation = new Vector3(0f, 0f, -90f);
    private float currentPhaseProgress = 0f; // A number from 0 to 1 indicating the progress of the current sub-animation from start to finish

    public override bool CanInteractWith(Pickupable heldItem)
    {
        return !heldItem && !SequenceTracker.Instance.bookRead;
    }

    private void Update()
    {
        if (animationStarted)
        {
            if (!orientBookComplete)
            {
                currentPhaseProgress += Time.deltaTime / orientBookTime;
                this.transform.localRotation = Quaternion.Slerp(startingRotation, Quaternion.Euler(orientBookRotation), Mathf.Clamp01(currentPhaseProgress));
                if (currentPhaseProgress >= 1f)
                {
                    orientBookComplete = true;
                    currentPhaseProgress -= 1f;

                    // Queue up SFX to play during next phase, i.e. Rotating the Book
                    // StartCoroutine(EnqueueAudio(pagesFlipping));
                }
            }
            else if (!rotateBookComplete)
            {
                currentPhaseProgress += Time.deltaTime / rotateBookTime;
                this.transform.localRotation = Quaternion.Slerp(Quaternion.Euler(orientBookRotation), Quaternion.Euler(rotateBookRotation), Mathf.Clamp01(currentPhaseProgress));
                if (currentPhaseProgress >= 1f)
                {
                    rotateBookComplete = true;
                    currentPhaseProgress -= 1f;

                    // Queue up SFX to play during next phase, i.e. Opening the Book
                    StartCoroutine(EnqueueAudio(pagesFlipping));
                }
            }
            else if (!openBookComplete)
            {
                // Debug.Log("Opening book: " + currentPhaseProgress);
                currentPhaseProgress += Time.deltaTime / openBookTime;
                frontCover.transform.localRotation = Quaternion.Slerp(Quaternion.identity, Quaternion.Euler(openFrontRotation), Mathf.Clamp01(currentPhaseProgress));
                backCover.transform.localRotation = Quaternion.Slerp(Quaternion.identity, Quaternion.Euler(openBackRotation), Mathf.Clamp01(currentPhaseProgress));
                if (currentPhaseProgress >= 1f)
                {
                    openBookComplete = true;
                    currentPhaseProgress -= 1f;

                    // Queue up SFX to play during next phase, i.e. Keep Book Open
                    // StartCoroutine(EnqueueAudio(bookReading));
                }
            }
            else if (!keepOpenComplete)
            {
                currentPhaseProgress += Time.deltaTime / keepOpenTime;
                // Do not rotate any part of the book during this phase
                if (currentPhaseProgress >= 1f)
                {
                    keepOpenComplete = true;
                    currentPhaseProgress -= 1f;

                    // Queue up SFX to play during next phase, i.e. Close Book
                    StartCoroutine(EnqueueAudio(bookClosing));
                }
            }
            else if (!closeBookComplete)
            {
                currentPhaseProgress += Time.deltaTime / closeBookTime;
                frontCover.transform.localRotation = Quaternion.Slerp(Quaternion.Euler(openFrontRotation), Quaternion.identity, Mathf.Clamp01(currentPhaseProgress));
                backCover.transform.localRotation = Quaternion.Slerp(Quaternion.Euler(openBackRotation), Quaternion.identity, Mathf.Clamp01(currentPhaseProgress));
                if (currentPhaseProgress >= 1f)
                {
                    closeBookComplete = true;
                    currentPhaseProgress -= 1f;
                }
            }
            else if (!rotateBackComplete)
            {
                currentPhaseProgress += Time.deltaTime / rotateBackTime;
                this.transform.localRotation = Quaternion.Slerp(Quaternion.Euler(rotateBookRotation), Quaternion.Euler(orientBookRotation), Mathf.Clamp01(currentPhaseProgress));
                if (currentPhaseProgress >= 1f)
                {
                    ResetAnimation();
                }
            }
        }
    }

    IEnumerator EnqueueAudio(AudioClip clip)
    {
        if (audioSource && clip)
        {
            while (audioSource.isPlaying)
            {
                yield return null;
            }
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    private void ResetAnimation()
    {
        animationStarted = false;
        orientBookComplete = false;
        rotateBookComplete = false;
        openBookComplete = false;
        keepOpenComplete = false;
        closeBookComplete = false;
        rotateBackComplete = false;
        currentPhaseProgress = 0f;
    }

    public void StartBookAnimation()
    {
        animationStarted = true;
        startingRotation = this.transform.localRotation;
    }

    public bool IsAnimationPlaying()
    {
        return animationStarted;
    }
}
