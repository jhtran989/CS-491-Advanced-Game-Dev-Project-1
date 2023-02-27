using System;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public Canvas canvas;
    public static CanvasManager canvasManagerInstance = null;

    private void Awake()
    {
        if (canvasManagerInstance == null)
        {
            canvasManagerInstance = this;
            Debug.Log("GameManager initially created");
        }
        else if (canvasManagerInstance != this)
        {
            Destroy(gameObject);
        }
        
        gameObject.DontDestroyOnLoad();
    }

    public void LoadCanvas()
    {
        Instantiate(canvas);
    }
}