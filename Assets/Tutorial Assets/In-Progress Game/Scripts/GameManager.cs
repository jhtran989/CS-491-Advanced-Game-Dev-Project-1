using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Make this class a singleton
    public static GameManager instance = null;

    // keeps track of the score
    public TextMeshProUGUI scoreText;
    private int _score = 0;
    
    // keep track of time
    public Timer timer;

    // FIXME: reduced from 1f (takes too long)
    public float levelStartDelay = 0.1f; 
    
    // FIXME: should be the same as the max blood level initially (else, the blood bar would start at max and suddenly jump to playerBloodLevel) 
    // SOLVED: create some Utilities file with global variables -> in Constants.cs file
    // initially set player blood level to the full bar
    public int playerBloodLevel = Constants.playerBloodMax;

    public float hordeSpeedScaling = 10f; // Larger is slower scaling 

    public BoardManager boardScript;
    private int boardNum = 0;

    public TextMeshProUGUI levelText; 
    public GameObject levelImage;
    public int level = 0;
    private bool enemiesMoving;
    private bool doingSetup = true;
    
    // instantiate a Canvas from a prefab (because of DontDestroyOnLoad)
    public CanvasManager canvasManager;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Debug.Log("GameManager initially created");
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        
        Debug.Log("Awake: level " + level);
        
        // FIXME: replace with mananger via extension method
        // DontDestroyOnLoad(gameObject);
        gameObject.DontDestroyOnLoad();
        
        // FIXME: replace with mananger via extension method
        GameObject.Find("Canvas").gameObject.DontDestroyOnLoad();

        //enemies = new List<Enemy>();
        boardScript = gameObject.GetComponent<BoardManager>();
        
        // load the CanvasManager
        canvasManager = gameObject.GetComponent<CanvasManager>();
        // GameObject.Find("Canvas").gameObject.SetActive(true);

        // don't clear the score from the previous level
        //DontDestroyOnLoad(scoreText);

        // // NOTE: Old scene load was deprecated
        // Debug.Log("GameManage OnLoadCallback");
        // SceneManager.sceneLoaded -= this.OnLoadCallback;
        // SceneManager.sceneLoaded += this.OnLoadCallback;
    }

    private void OnEnable()
    {
        // NOTE: Old scene load was deprecated
        Debug.Log("GameManager OnLoadCallback ENABLE");
        SceneManager.sceneLoaded += this.OnLoadCallback;
    }

    private void OnDisable()
    {
        // NOTE: Old scene load was deprecated
        Debug.Log("GameManager OnLoadCallback DISABLE");
        SceneManager.sceneLoaded -= this.OnLoadCallback;
    }

    // [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    // public static void GameManagerSceneLoad()
    // {
    //     SceneManager.sceneLoaded += instance.OnLoadCallback;
    // }

    // NOTE: Old scene load was deprecated
    void OnLoadCallback(Scene scene, LoadSceneMode sceneMode)
    {
        // first load the canvas
        //canvasManager.LoadCanvas();
        
        level++;
        SpeedUpHorde();
        KeepScoreOnLoad();
        InitGame();
        
        // want to start timer AFTER initializing the game (with the level delay)
        KeepTimerOnLoad();
        
        // FIXME: moved setup scene (AFTER the timer)
        // FIXME: a brand new board object gets created on the SECOND level...
        Debug.Log("Board setup " + boardNum);
        boardNum += 1;
        
        boardScript.SetupScene(level); 
    }

    void SpeedUpHorde()
    {
        // FIXME: changed out horde with the no smoke version
        // Horde horde = GameObject.Find("Horde").GetComponent<Horde>();
        Horde horde = GameObject.Find(Constants.horde).GetComponent<Horde>();
        
        horde.speed = Mathf.Log(level, hordeSpeedScaling) + 1;
    }

    void KeepScoreOnLoad()
    {
        // update the score
        // TODO: need to get the score text object each time
        scoreText = GameObject.Find("Score Text")
            .GetComponent<TextMeshProUGUI>();
        
        // since _score is kept across levels, just need to update the display with the current score (no additional points added to the score)
        UpdateScore(0);
    }

    void KeepTimerOnLoad()
    {
        // start tracking time
        Timer.timerInstance.StartTime();
    }

    void InitGame() 
    {
        doingSetup = true;

        levelImage = GameObject.Find("LevelImage").gameObject;
        levelText = GameObject.Find("LevelText").GetComponent<TextMeshProUGUI>();

        levelText.SetText("Level " + level);
        levelImage.SetActive(true);

        Invoke("HideLevelImage", levelStartDelay);
        
        // initialize score
        // EDIT: should not set to 0 on every level...
        //score = 0;
    }


    void HideLevelImage()
    {
        if(levelImage != null)
        {
            levelImage.SetActive(false);
        }

        doingSetup = false;
    }

    // FIXME: need to update game over screen (not just level screen)
    // GameOver is called when the player reaches 0 food points
    public void GameOver()
    {
        Debug.Log("Got to game over");
        // update the game over screen with some stats
        levelText.text = "Your cravings were not satisfied...\n" + 
                         "Level: " + level + "\n" + 
                         "Score: " + _score;

        //levelImage = GameObject.Find("LevelImage");
        levelImage.SetActive(true);
        
        // destroy the score text
        scoreText.SetText("");
        Destroy(scoreText);
        
        Debug.Log("Game Over -- stopping timer...");

        // stop tracking time and destroy text

        Timer.timerInstance.StopTime();
        Timer.timerInstance.TimeText.SetText("");
        Destroy(Timer.timerInstance);


        // Enable black background image gameObject.

        // Disable this GameManager.
        enabled = false;
    }

    // update the score
    public void UpdateScore(int scoreToAdd)
    {
        // update score
        _score += scoreToAdd;
        scoreText.text = "Score: " + _score;
        
        Debug.Log("Updated score: " + _score);
    }
}
