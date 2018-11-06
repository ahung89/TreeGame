using UnityEngine;
using System.Collections;

public class PotPanShake : MonoBehaviour
{
    public int shakeCount = 1;
    public float shakeTime = 1f;
    public float maxRotation = 5f;
    public float randomRange = 0.2f;

    private float shakeTimeRemaining = 0f;
    private Vector3 defaultRotation;

    private void Awake()
    {
        defaultRotation = transform.localRotation.eulerAngles;

        // Randomize shaking of each pan by slightly increasing or decreasing the shake duration
        shakeTime += (Random.Range(-randomRange / 2f, randomRange / 2f));
    }

    private void Update()
    {
        if (shakeTimeRemaining > 0f)
        {
            float dyanmicAmplitude = maxRotation * (shakeTimeRemaining / shakeTime);
            float newZRotation = defaultRotation.z + Mathf.Sin(Mathf.PI * (shakeTime - shakeTimeRemaining) * (shakeCount / shakeTime)) * dyanmicAmplitude;
            this.transform.localRotation = Quaternion.Euler(defaultRotation.x, defaultRotation.y, newZRotation);
            shakeTimeRemaining -= Time.deltaTime;
        }
    }

    public void StartShake()
    {
        shakeTimeRemaining = shakeTime;
    }
}
