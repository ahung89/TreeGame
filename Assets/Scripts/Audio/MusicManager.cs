using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour {

    public static MusicManager Instance;

    public AudioMixer mixer;
    public float fadeInTime = 2.0f;

    public bool testing = false;

    private int layer = 1;
    private float lerp = 1.0f;
    private float fadeStartTime = 0;

    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
    }

    void Update () {
        if (lerp < 1)
        {
            lerp = (Time.time - fadeStartTime) / fadeInTime;
            mixer.SetFloat("Vol" + layer, Mathf.Lerp(-80.0f, 0.0f, lerp));
        }

        if (Input.GetKeyDown(KeyCode.Space) && testing) {
            AddNextLayer();
        }
	}

    public void AddNextLayer()
    {
        layer++;
        lerp = 0.0f;
        fadeStartTime = Time.time;
    }
}
