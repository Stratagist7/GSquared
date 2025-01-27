using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreAudioPause : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioSource src = GetComponent<AudioSource>();
        src.ignoreListenerPause = true;
    }
}
