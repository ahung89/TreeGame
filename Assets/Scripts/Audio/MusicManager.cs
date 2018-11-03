using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour {

    public static MusicManager Instance;

    public AudioMixer mixer;
    public float fadeInTime = 2.0f;

    private int layer = 1;
    private float lerp = 0.0f;
    private float fadeStartTime = 0;

    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
    }

    void Start()
    {
        AddNextLayer();
    }

    void Update () {
        mixer.SetFloat("Vol" + layer, Mathf.Lerp(-80.0f, 0.0f, (Time.time - fadeStartTime) / fadeInTime));
	}

    public void AddNextLayer()
    {
        layer++;
        lerp = 0.0f;
        fadeStartTime = Time.time;
    }
}
