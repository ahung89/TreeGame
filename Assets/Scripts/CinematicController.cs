using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine;

public class CinematicController : MonoBehaviour {

    public static CinematicController Instance;

    public GameObject cinematicDirector;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
    }

    void Update () {
        // test
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCinematic();
        }
	}

    public void StartCinematic()
    {
        cinematicDirector.SetActive(true);
    }
}
