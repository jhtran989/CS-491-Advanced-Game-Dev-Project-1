using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float timeRemaining = 10;
    public float timeElapsed = 0f;
    public bool timerIsRunning = false;
    
    // keep track of text in game
    public TextMeshProUGUI TimeText { get; set; }
    
    public static Timer timerInstance = null;
    
    void Awake()
    {
        if (timerInstance == null)
        {
            timerInstance = this;
            
            Debug.Log("initial timer");
        }
        else if (timerInstance != this)
        {
            Destroy(gameObject);
            Debug.Log("destroyed timer");
            
            // FIXME: need to destroy BloodBar each time...which is the first child under Canvas
            // FIXME: need another object to delete the BloodBar, like the Player
            // Destroy(GameObject.Find("Canvas").transform.GetChild(0).gameObject);
        }

        // TODO: can only call DontDestroyOnLoad() on root objects...which is the Canvas in this case
        DontDestroyOnLoad(GameObject.Find("Canvas"));

        TimeText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        // FIXME: for countdown timer
        // Starts the timer automatically
        // timerIsRunning = true;
        
        // find the time object
        //TimeText = GetComponent<TextMeshProUGUI>();
    }
    
    void Update()
    {
        timeElapsed += Time.deltaTime;
        DisplayTime(timeElapsed);
        
        // FIXME: for countdown timer below
        // if (timerIsRunning)
        // {
        //     if (timeRemaining > 0)
        //     {
        //         timeRemaining -= Time.deltaTime;
        //         DisplayTime(timeRemaining);
        //     }
        //     else
        //     {
        //         Debug.Log("Time has run out!");
        //         timeRemaining = 0;
        //         timerIsRunning = false;
        //     }
        // }
    }
    
    void DisplayTime(float timeToDisplay)
    {
        //timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        TimeText.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
    }

    public void StopTime()
    {
        enabled = false;
    }

    public void StartTime()
    {
        enabled = true;
    }
}