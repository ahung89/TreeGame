using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GasFireValve : Interactable
{
    public Light fireLight;
    public float noiseFrequency = 1f;
    public float noiseWeight = 1f;
    public float defaultIntensity = 1f;
    public float minIntensity = 0.1f;

    public Tree tree;

    public float dimmingDuration = 3f;
    private bool isDimming = false;
    private bool isDimmed = false;
    private float dimmingElapsed = 0f;

    private AudioSource audioSource;

    public GameObject fireParticleEffect;
    private List<float> particleStartSize = new List<float>();
    private float particleEmissionRate;

    private GasValveTurn turnEffect;

    public FadeAlpha trebleClefClue;

    private void Awake()
    {
        fireLight.intensity = defaultIntensity;
        audioSource = GetComponent<AudioSource>();

        for (int i = 0; i < fireParticleEffect.transform.childCount; i++)
        {
            ParticleSystem particles = fireParticleEffect.transform.GetChild(i).GetComponent<ParticleSystem>();
            if (particles.main.startSize.mode == ParticleSystemCurveMode.Constant)
            {
                particleStartSize.Add(particles.main.startSize.constant);
            }
            else
            {
                // particleStartSize.Add(particles.main.startSize.constantMax);
                particleEmissionRate = particles.emissionRate;
            }
        }

        turnEffect = GetComponent<GasValveTurn>();
    }

    public override bool CanInteractWith(Pickupable heldItem)
    {
        return !heldItem && !SequenceTracker.Instance.fireOut;
    }

    // Returns whether to TryPickUp or Drop after this action
    public override bool Interact(Pickupable heldItem)
    {
        if (SequenceTracker.Instance.milkConsumed &&
            SequenceTracker.Instance.teddyBearProvided &&
            SequenceTracker.Instance.bookRead &&
            !SequenceTracker.Instance.fireOut)
        {
            isDimming = true;
            if (trebleClefClue) trebleClefClue.StartFadeIn();
            SequenceTracker.Instance.fireOut = true;
            Debug.Log("Yaawwwnnnn... Getting sleepy.");
            // elicit a positive reaction
            turnEffect.StartFullTurn();
            StartCoroutine(PlayInteractionSounds());
            MusicManager.Instance.AddNextLayer();
            return false; // Do not try to pick up or drop item after this interaction
        }
        else if (SequenceTracker.Instance.fireOut)
        {
            Debug.Log("DARK... MUST HAVE DARK! (already put out fire)");
            tree.PlayClip(tree.hardNegativeReaction);
            return false; // Do not try to pick up or drop item after this interaction
        }
        else
        {
            Debug.Log("Nah, not yet");
            turnEffect.StartPartialTurn();
            tree.PlayClip(tree.softNegativeReaction);
            return false; // No need to try picking up or dropping after this interation
        }
    }

    private void Update()
    {
        // Calculate reduced fire light intensity and diminish particle effect if currently dimming
        float currentIntensity = defaultIntensity;
        if (isDimming)
        {
            dimmingElapsed = Mathf.Clamp(dimmingElapsed + Time.deltaTime, 0f, dimmingDuration);
            // currentIntensity = minIntensity + (defaultIntensity - minIntensity) * (1 - Mathf.Pow(dimmingElapsed / dimmingDuration, 2f));
            currentIntensity = minIntensity + (defaultIntensity - minIntensity) * Mathf.Cos((Mathf.PI * dimmingElapsed) / (dimmingDuration * 2f));

            // Handle diminishing the particle effect's start sizes
            for (int i = 0; i < fireParticleEffect.transform.childCount; i++)
            {
                ParticleSystem particles = fireParticleEffect.transform.GetChild(i).GetComponent<ParticleSystem>();
                if (particles.main.startSize.mode == ParticleSystemCurveMode.Constant)
                {
                    particles.startSize = particleStartSize[i] * Mathf.Clamp01(1 - (dimmingElapsed / dimmingDuration) + 0.15f);
                }
                else
                {
                    // particles.startSize = particleStartSize[i] * (1 - (dimmingElapsed / dimmingDuration));
                    particles.emissionRate = particleEmissionRate * Mathf.Clamp01(1 - (dimmingElapsed / dimmingDuration) + 0.15f);
                }
            }
        }
        else if (isDimmed)
        {
            currentIntensity = minIntensity;
        }

        // Add noise to the current fire light intensity
        float noise = 0f;
        for (int i = 0; i < 3; i++)
        {
            float oct = Mathf.PerlinNoise(Time.time * Mathf.Pow(noiseFrequency, (float)i), 0f) - 0.5f;
            noise += oct * Mathf.Pow(0.5f, (float)i);
        }
        fireLight.intensity = currentIntensity + (noiseWeight * (currentIntensity/defaultIntensity) * noise);

        // Mark dimming as complete if dimming time elapsed
        if (isDimming && dimmingElapsed >= dimmingDuration)
        {
            isDimmed = true;
            isDimming = false;
        }
    }

    IEnumerator PlayInteractionSounds()
    {
        audioSource.Play();
        while (audioSource.isPlaying)
        {
            yield return null;
        }
        tree.anim.SetTrigger("Positive");
        tree.PlayClip(tree.positiveReactionLight);
    }
}
