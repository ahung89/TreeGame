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
    bool milkConsumed = false;
    bool teddyBearProvided = false;
    bool bookRead = false;
    bool fireOut = false;
    // bool flutePlayed = false; // Will this be necessary? Maybe initiate a Coroutine on successfully playing the flute
}
