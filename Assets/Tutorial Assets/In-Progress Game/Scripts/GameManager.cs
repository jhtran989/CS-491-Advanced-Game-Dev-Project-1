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

    public float levelStartDelay = 1f; 
    public int playerBloodLevel;
    public float enemyTurnDelay = 0.1f;

    public float hordeSpeedScaling = 10f; // Larger is slower scaling 

    public BoardManager boardScript;

    private TextMeshProUGUI levelText; 
    private GameObject levelImage;
    public int level = 0;
    private bool enemiesMoving;
    private bool doingSetup = true;  
    private List<Enemy> enemies; 

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        enemies = new List<Enemy>();
        boardScript = GetComponent<BoardManager>();
        
        // don't clear the score from the previous level
        //DontDestroyOnLoad(scoreText);

        // NOTE: Old scene load was deprecated
        SceneManager.sceneLoaded += this.OnLoadCallback;
    }

    // NOTE: Old scene load was depricated
    void OnLoadCallback(Scene scene, LoadSceneMode sceneMode)
    {
        level++;
        SpeedUpHorde();
        KeepScoreOnLoad();
        InitGame();
    }

    void SpeedUpHorde()
    {
        Horde horde = GameObject.Find("Horde").GetComponent<Horde>();
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

    void InitGame() 
    {
        doingSetup = true;

        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<TextMeshProUGUI>();

        levelText.SetText("Level " + level);
        levelImage.SetActive(true);

        Invoke("HideLevelImage", levelStartDelay);
        
        // initialize score
        // EDIT: should not set to 0 on every level...
        //score = 0;

        enemies.Clear();

        boardScript.SetupScene(level);    
    }


    void HideLevelImage()
    {
        levelImage.SetActive(false);

        doingSetup = false;
    }

    // void Update()
    // {
    //     if(enemiesMoving || doingSetup)
    //         return;
    //     StartCoroutine (MoveEnemies ());
    // }

    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }

    // FIXME: need to update game over screen (not just level screen)
    // GameOver is called when the player reaches 0 food points
    public void GameOver(string gameOverText)
    {
        levelText.text = gameOverText;
        
        // Enable black background image gameObject.
        levelImage.SetActive(true);

        // Disable this GameManager.
        enabled = false;
    }

    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;

        yield return new WaitForSeconds(enemyTurnDelay);

        if (enemies.Count == 0) 
        {
            yield return new WaitForSeconds(enemyTurnDelay);
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].MoveEnemy ();

            yield return new WaitForSeconds(enemies[i].moveTime);
        }

        enemiesMoving = false;
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
