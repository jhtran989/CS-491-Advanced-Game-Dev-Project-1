using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Main Menu: Game Closed");
    }

    public void StartGame()
    {
        SceneManager.LoadScene(Constants.mainGameScene);
        // GameObject.Find("Canvas").gameObject.SetActive(true);
        
        if (DontDestroyOnLoadManager.canvasObject != null)
        {
            DontDestroyOnLoadManager.canvasObject.SetActive(true);
        }
    }
}
