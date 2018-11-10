using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour {

    public static MusicManager Instance;

    public AudioSource finaleSource;

    public AudioMixer mixer;
    public float fadeInTime = 4.0f;
    public float masterVolumeFadeTime = 2.0f;
    public bool gameLoopOn = true;

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
        
        var masterVolume = Mathf.SmoothStep(gameLoopOn ? -80 : 0, gameLoopOn ? 0 : -80, (Time.time - masterFadeStartTime) / masterVolumeFadeTime);
        mixer.SetFloat("GameLoopVol", masterVolume);

        if (testing)
        {
            mixer.SetFloat("Vol1", 0.0f);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PlayFinale();
            }
        }
	}
    
    public void ToggleGameLoop()
    {
        masterFadeStartTime = Time.time;
        gameLoopOn = !gameLoopOn;
    }

    public void AddNextLayer()
    {
        layer++;
        lerp = 0.0f;
        fadeStartTime = Time.time;
    }

    public void PlayFinale()
    {
        gameLoopOn = false; // ensure loop is off (but it already should be)
        finaleSource.Play();
    }

    public void ResetMusicLayers()
    {
        for (int i = 1; i < 5; i++)
        {
            mixer.SetFloat("Vol" + i, -80.0f);
        }
    }
}
