using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  
using UnityEngine.SceneManagement;
using TMPro;

public static class DontDestroyOnLoadManager
{
    private static List<GameObject> _ddolGameObjects = new List<GameObject>();
    private static List<AudioSource> _ddolAudioSources = new List<AudioSource>();
    public static GameObject canvasObject = null;
    private static bool initialDestroy = false;

    public static void DontDestroyOnLoad(this GameObject go) {
        UnityEngine.Object.DontDestroyOnLoad(go);
        _ddolGameObjects.Add(go);
    }
    
    public static void DontDestroyOnLoad(this AudioSource go) {
        UnityEngine.Object.DontDestroyOnLoad(go);
        _ddolAudioSources.Add(go);
    }

    public static void DestroyAll() {
        foreach (var go in _ddolGameObjects)
        {
            if (go != null)
            {
                if (go.name != Constants.canvasGameObject)
                {
                    UnityEngine.Object.Destroy(go);
                }
                else
                {
                    if (!initialDestroy)
                    {
                        go.SetActive(false);
                        // go.SetActive(true);
                        canvasObject = go;
                        initialDestroy = true;
                    }
                    else
                    {
                        canvasObject.SetActive(false);
                        Debug.Log("Canvas inactive after FIRST");
                    }
                }
            }
        }

        foreach(var go in _ddolAudioSources)
            if(go != null)
                UnityEngine.Object.Destroy(go);

        _ddolGameObjects.Clear();
        _ddolAudioSources.Clear();
    }
}