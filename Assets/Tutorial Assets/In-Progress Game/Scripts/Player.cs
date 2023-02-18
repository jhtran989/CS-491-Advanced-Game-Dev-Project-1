using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// FIXME: changed MonoBehavior to MovingObject
// TODO: can enemies affect the items of the player (like the bloodpacks)? - LoseFood()

// Player inherits from MovingObject, our base class for objects that can move, Enemy also inherits from this.
public class Player : MonoBehaviour
{
    // in-game stats
    public float speed = 4;
    public int wallDamage = 1;
    public int pointsPerBloodpack = 10; // pointsPerFood
    public int pointsPerInfusion = 5; // pointsPerSoda
    
    // FIXME: reduced from 1f (takes too long)
    public float restartLevelDelay = 0.5f;

    public int playerBloodMax = 50;

    public BloodBar bloodBar;

    private Animator animator;
    private int bloodLevel; // food

    public Rigidbody2D moverBody;

    // TODO: private variables are supposed to have a "_" in front of the variable name
    // forgot about the GameManager instance
    // private GameManager _gameManager;
    private int _scoreMultiplier = 10;
    
    // keep track of time
    public Timer timer;

    void Start()
    {
        // Get a component reference to the Player's animator component
        animator = GetComponent<Animator>();

        // Get the current food point total stored in GameManager.instance between levels.
        bloodLevel = GameManager.instance.playerBloodLevel;

        bloodBar = GameObject.Find("BloodBar").GetComponent<BloodBar>();
        bloodBar.SetMaxBloodLevel(playerBloodMax);
        
        // need to set current blood level, carrying over from levels...
        bloodBar.SetBloodLevel(GameManager.instance.playerBloodLevel);
        
        InvokeRepeating("LoseBlood", 1f, 1f);

    }

    // This function is called when the behaviour becomes disabled or inactive.
    private void OnDisable()
    {
        // When Player object is disabled, store the current local food total in the GameManager so it can be re-loaded in next level.
        GameManager.instance.playerBloodLevel = bloodLevel;
    }
    
    private Vector2 GetInput()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");
        return new Vector2(inputX, inputY).normalized;
    }

    // TODO: changed from tutorial (player can destroy walls?)
    protected void Update()
    {
        int horizontal = 0;      // Used to store the horizontal move direction.
        int vertical = 0;        // Used to store the vertical move direction.
        
        // Get input from the input manager, round it to an integer and store in horizontal to set x axis move direction
        horizontal = (int) (Input.GetAxisRaw ("Horizontal"));

        // Get input from the input manager, round it to an integer and store in vertical to set y axis move direction
        vertical = (int) (Input.GetAxisRaw ("Vertical"));
        
        Vector2 movementNormalized = GetInput();
        Move(movementNormalized * speed);
    }

    void LoseBlood()
    {
        bloodLevel--;
        bloodBar.SetBloodLevel(bloodLevel);
        CheckIfGameOver();
    }

    void GainBlood(int amount)
    {
        bloodLevel = Mathf.Min(bloodLevel + amount, playerBloodMax);
        bloodBar.SetBloodLevel(bloodLevel);
        
        // update score in GameManager
        //_gameManager.UpdateScore(amount * _scoreMultiplier);
        GameManager.instance.UpdateScore(amount * _scoreMultiplier);
    }

    // OnTriggerEnter2D is sent when another object enters a trigger collider attached to this object (2D physics only).
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("other tag: " + other.tag);
        
        // Check if the tag of the trigger collided with is Exit.
        // NOTE: need to use updated tags, like "Infusion" and "Bloodpack"
        if (other.tag == "Exit")
        {
            // Invoke the Restart function to start the next level with a delay of restartLevelDelay (default 1 second).
            Invoke("Restart", restartLevelDelay);
            
            // stop tracking time
            Timer.timerInstance.StopTime();
            
            // FIXME: need to destroy BloodBar each time...which is the first child under Canvas
            // FIXME: need another object to delete the BloodBar, like the Player
            // Destroy(GameObject.Find("Canvas").transform.GetChild(0).gameObject);
            Destroy(bloodBar.gameObject);

            // Disable the player object since level is over.
            enabled = false;
        }
        
        // Check if the tag of the trigger collided with is Food.
        else if (other.tag == "Infusion")
        {
            // TODO: keep track of the number of items?
            GainBlood(pointsPerInfusion);
            // Disable the infusion object the player collided with.
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Bloodpack")
        {
            // TODO: keep track of the number of items?
            GainBlood(pointsPerBloodpack);
            //Disable the bloodpack object the player collided with.
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Enemy")
        {
            // Call the GameOver function of GameManager.
            GameManager.instance.GameOver("The horde got you...\n" + 
                                          "Blood Level: " + bloodLevel);

            // make sure to destroy player
            Destroy(gameObject);
        }
    }
    
    // Restart reloads the scene when called.
    private void Restart()
    {
        // Load the last scene loaded, in this case Main, the only scene in the game.
        SceneManager.LoadScene(0);
    }
    
    // CheckIfGameOver checks if the player is out of food points and if so, ends the game.
    private void CheckIfGameOver()
    {
        if (bloodLevel < 0)
            GameManager.instance.GameOver("Your cravings were not satisfied...");
    }

    private void Move(Vector2 velocity)
    {
        moverBody.velocity = velocity;
    }
}
