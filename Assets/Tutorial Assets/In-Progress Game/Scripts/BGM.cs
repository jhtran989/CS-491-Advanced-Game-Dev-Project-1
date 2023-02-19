using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    public AudioSource sourceA;
    public AudioSource sourceB;
    
    // need to store the previous values of the pitch BEFORE pausing the game to restore the pitch levels
    private float _sourceVolumeA;
    private float _sourceVolumeB;
    private bool _updateVolume = false;

    public static BGM instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(sourceA);
        DontDestroyOnLoad(sourceB);
    }

    private void Update()
    {
        // Debug.Log("BGM - start");
        
        // update music on pause
        if (PauseMenu.gameIsPaused)
        {
            if (!_updateVolume)
            {
                _sourceVolumeA = sourceA.volume;
                _sourceVolumeB = sourceB.volume;
                
                if (_sourceVolumeA != 0)
                {
                    sourceA.volume = 0.25f;
                }
                else if (_sourceVolumeB != 0)
                {
                    sourceB.volume = 0.25f;
                }
                
                _updateVolume = true;
                
                Debug.Log("BEFORE source A volume: " + _sourceVolumeA);
                Debug.Log("BEFORE source B volume: " + _sourceVolumeB);
            }
        }
        else
        {
            // have conditional so values aren't set each time
            if (!_updateVolume) return;
            
            Debug.Log("AFTER source A volume: " + _sourceVolumeA);
            Debug.Log("AFTER source B volume: " + _sourceVolumeB);
            
            sourceA.volume = _sourceVolumeA;
            sourceB.volume = _sourceVolumeB;

            _updateVolume = false;
        }
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
