using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceTracker : MonoBehaviour
{
    // Singleton pattern
    public static SequenceTracker Instance;
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;

        }
    }
    // Singleton pattern 

    // These flags indicate whether actions in the desired sequence have been successfully completed
    public bool milkConsumed = false;
    public bool teddyBearProvided = false;
    public bool bookRead = false;
    public bool fireOut = false;
    // public bool flutePlayed = false; // Will this be necessary? Maybe initiate a Coroutine on successfully playing the flute
}
