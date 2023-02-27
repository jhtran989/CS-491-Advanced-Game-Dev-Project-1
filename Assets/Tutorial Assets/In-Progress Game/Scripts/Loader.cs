using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{

    public GameObject gameManager;
    public GameObject timerMain;
    public GameObject canvasMain;

    void Awake()
    {
        // FIXME: initial instance of GameManager is being destroyed for some reason 
        // if (GameManager.instance == null)
        //     Instantiate(gameManager);
        
        // FIXME: timer doesn't need loader, unlike game manager
        // add same thing to the timer
        // if (Timer.timerInstance == null)
        // {
        //     Instantiate(timerMain);
        // }
        
        // FIXME: Canvas needs a loader (will only be created once)
        // if (CanvasManager.canvasManagerInstance == null)
        // {
        //     Instantiate(canvasMain);
        // }
    }
}
