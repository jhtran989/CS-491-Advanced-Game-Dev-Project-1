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

    public float levelStartDelay = 2f; 
    public int playerBloodLevel = 0;
    public float enemyTurnDelay = 0.1f;

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

        // NOTE: Old scene load was depricated
        SceneManager.sceneLoaded += this.OnLoadCallback;
    }

    // NOTE: Old scene load was depricated
    void OnLoadCallback(Scene scene, LoadSceneMode sceneMode)
    {
        level++;
        InitGame();
    }

    void InitGame() 
    {
        doingSetup = true;

        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<TextMeshProUGUI>();

        levelText.SetText("Level " + level);
        levelImage.SetActive(true);

        Invoke("HideLevelImage", levelStartDelay);

        enemies.Clear();

        boardScript.SetupScene(level);    
    }


    void HideLevelImage()
    {
        levelImage.SetActive(false);

        doingSetup = false;
    }

    void Update()
    {
        if(enemiesMoving || doingSetup)
            return;
        StartCoroutine (MoveEnemies ());
    }

    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }

    public void GameOver()
    {
        //levelText.SetText("Game Over");
        levelImage.SetActive(true);

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
}
