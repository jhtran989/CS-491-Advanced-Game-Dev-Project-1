using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    public AudioSource sourceA;
    public AudioSource sourceB;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(sourceA);
        DontDestroyOnLoad(sourceB);
    }

    public void SwitchTrack(bool toBat)
    {
        if(toBat)
        {
            sourceA.volume = 0;
            sourceB.volume = 1;
        }
        else 
        {
            sourceA.volume = 1;
            sourceB.volume = 0;
        }
    }
}
