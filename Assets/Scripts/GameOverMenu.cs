using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverMenu : MonoBehaviour
{

    public TextMeshProUGUI levelText; 
    public GameObject levelImage;
    public GameObject gameOverMenu;

    private static bool initialCreation = true;

    // Mimic deletion of objects created on new load (from using DontDestroyOnLoad) since the Canvas and ALL children are affected...including the Pause Menu
    private void Awake()
    {
        // FIXME: BIG BUG - need to find some way to remove the other instances of the GameOver Menu
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

    public void DisplayGameOver(int level, int score)
    {
        // update the game over screen with some stats
        levelText.text = "Your cravings were not satisfied...\n" + 
                         "Level: " + level + "\n" + 
                         "Score: " + score;

        levelImage.SetActive(true);
        
        gameOverMenu.SetActive(true);
    }
}
