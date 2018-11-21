using UnityEngine;
using System.Collections;

public class Sink : Interactable
{
    public float waterStreamDuration = 2f;
    private float waterStreamElapsed = -1f;
    private bool waterStreamRunning = false;

    public GameObject waterStream;
    public GameObject splashEffect;

    public AudioSource waterRunningAudioSource;

    public FaucetHandleTurn leftHandleTurn;
    public FaucetHandleTurn rightHandleTurn;

    private void Awake()
    {
        waterStream.SetActive(false);
    }

    public override bool CanInteractWith(Pickupable heldItem)
    {
        return heldItem == null || heldItem.GetComponent<Cup>();
    }

    public override bool Interact(Pickupable heldItem)
    {
        Debug.Log("Interact with Sink");
        if (CanInteractWith(heldItem))
        {
            if (!waterStreamRunning)
            {
                Debug.Log("Start Water Running");
                StartWaterRunning();
            }

            if (waterStreamRunning)
            {
                if (heldItem)
                {
                    Cup cup = heldItem.GetComponent<Cup>();
                    if (cup)
                    {
                        cup.FillCup(Cup.Liquid.Water);
                        if (!cup.IsPlayingClip())
                        {
                            cup.PlayClip(cup.cupFillClip);
                        }
                    }
                }
            }
        }
        return false; // No need to try picking up or dropping after this interation
    }

    private void Update()
    {
        if (waterStreamRunning && waterStreamElapsed >= 0f)
        {
            waterStreamElapsed += Time.deltaTime;
            
            if (waterStreamElapsed >= waterStreamDuration)
            {
                Debug.Log("Stop Water Running");
                StartCoroutine(StopWaterRunning());
                waterStreamElapsed = -1f; // Coroutine does not immediately flip waterStreamRunning, so we will use -1f to show that it should not be incremented
            }
        }
    }

    private void StartWaterRunning()
    {
        if (!waterStreamRunning)
        {
            leftHandleTurn.StartTurnOn();
            rightHandleTurn.StartTurnOn();

            waterStream.SetActive(true);
            waterStreamRunning = true;
            waterStreamElapsed = 0f;

            // Play water running sound
            if (waterRunningAudioSource && waterRunningAudioSource.clip) // && !waterRunningAudioSource.isPlaying)
            {
                waterRunningAudioSource.Stop();
                waterRunningAudioSource.Play();
            }

            ParticleSystem[] particleSystems = splashEffect.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem particleSystem in particleSystems)
            {
                particleSystem.Play();
            }
        }
    }

    IEnumerator StopWaterRunning()
    {
        leftHandleTurn.StartTurnOff();
        rightHandleTurn.StartTurnOff();

        yield return new WaitForSeconds(0.75f); // Give handle turning a bit of a head start

        ParticleSystem[] particleSystems = splashEffect.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem particleSystem in particleSystems)
        {
            particleSystem.Stop();
        }

        yield return new WaitForSeconds(0.25f); // Last particles need some time to die
        
        waterStream.SetActive(false);

        yield return new WaitForSeconds(0.25f); // Allow time for handles to finish turning off

        waterStreamRunning = false;
    }
}
