using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Make this class a singleton
    public static GameManager instance = null;

    public int playerBloodLevel = 0;

    public BoardManager boardScript;

    private int level = 3;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        boardScript = GetComponent<BoardManager>();
        InitGame();
    }

    void InitGame() 
    {
        boardScript.SetupScene(level);
    }

    public void GameOver()
    {
        enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
