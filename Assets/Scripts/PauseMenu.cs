using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public bool checkGameIsPaused = gameIsPaused;

    public GameObject pauseMenuUI;
    private static bool initialCreation = true;
    
    // Mimic deletion of objects created on new load (from using DontDestroyOnLoad) since the Canvas and ALL children are affected...including the Pause Menu
    private void Awake()
    {
        // FIXME: BIG BUG - need to find some way to remove the other instances of the Pause Menu
        if (initialCreation)
        {
            initialCreation = false;
        }
        else
        {
            Destroy(gameObject);
        }
        
        Debug.Log("Pause Menu - Awaken");
    }

    // Update is called once per frame
    void Update()
    {
        // FIXME: cannot see static variables in Inspector so use a public variable that matches gameIsPaused
        checkGameIsPaused = gameIsPaused;
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
        
        //Debug.Log("pause - successful?");
    }

    public void LoadMenu()
    {
        // FIXME: uncomment when doing TODO below
        // Time.timeScale = 1f;
        Debug.Log("Check Menu Button");
        
        // FIXME: need to destroy the game scene...specifically, the game objects that call DontDestroyOnLoad()
        // https://gamedev.stackexchange.com/questions/140014/how-can-i-get-all-dontdestroyonload-gameobjects
        // FIXME: unsupported...
        // SceneManager.UnloadSceneAsync(Constants.mainGameScene);
        
        // need to resume the pause menu (so it doesn't appear on a restart of the game)
        Resume();
        
        // need to reset time
        Timer.timerInstance.ResetTime();
        
        // FIXME: now need to destroy all GameObjects and AudioSources that call DontDestroyOnLoad
        DontDestroyOnLoadManager.DestroyAll();

        SceneManager.LoadScene(Constants.mainMenuScene);
        // Canvas.ForceUpdateCanvases();
    }

    public void QuitGame()
    {
        Debug.Log("Check Quit Button");
        Application.Quit(); 
    }
}
