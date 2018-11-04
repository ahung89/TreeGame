using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour {

    public static MusicManager Instance;

    public AudioMixer mixer;
    public float fadeInTime = 4.0f;
    public float masterVolumeFadeTime = 2.0f;
    public bool musicOn = true;

    public bool testing = false;

    private int layer = 0;
    private float lerp = 1.0f;
    private float fadeStartTime = 0;
    
    private float masterFadeStartTime = 0;

    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
    }

    void Update () {
        if (lerp < 1 && layer < 5)
        {
            lerp = (Time.time - fadeStartTime) / fadeInTime;
            mixer.SetFloat("Vol" + layer, Mathf.Lerp(-80.0f, 0.0f, lerp));
        }
        
        var masterVolume = Mathf.SmoothStep(musicOn ? -80 : 0, musicOn ? 0 : -80, (Time.time - masterFadeStartTime) / masterVolumeFadeTime);
        mixer.SetFloat("MasterVol", masterVolume);

        if (testing)
        {
            mixer.SetFloat("Vol1", 0.0f);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ToggleMusic();
            }
        }
	}

    public void ToggleMusic()
    {
        masterFadeStartTime = Time.time;
        musicOn = !musicOn;
    }

    public void AddNextLayer()
    {
        layer++;
        lerp = 0.0f;
        fadeStartTime = Time.time;
    }
}
