using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// FIXME: changed MonoBehavior to MovingObject
// TODO: can enemies affect the items of the player (like the bloodpacks)? - LoseFood()

// Player inherits from MovingObject, our base class for objects that can move, Enemy also inherits from this.
public class Player : MovingObject
{
    // in-game stats
    public float speed;
    public int wallDamage = 1;
    public int pointsPerBloodpack = 10; // pointsPerFood
    public int pointsPerInfusion = 20; // pointsPerSoda
    public float restartLevelDelay = 1f;


    private Animator animator;
    private int bloodLevel; // food

    public Rigidbody2D moverBody;
    
    void Start()
    {
        // Get a component reference to the Player's animator component
        animator = GetComponent<Animator>();

        // Get the current food point total stored in GameManager.instance between levels.
        bloodLevel = GameManager.instance.playerBloodLevel;

        // Call the Start function of the MovingObject base class.
        base.Start();
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
        
        // FIXME: condition below freezes or disables the character for some reason... 
        // Check if we have a non-zero value for horizontal or vertical
        // if(horizontal != 0 || vertical != 0)
        // {
        //     // Call AttemptMove passing in the generic parameter Wall, since that is what Player may interact with if they encounter one (by attacking it) 
        //     // Pass in horizontal and vertical as parameters to specify the direction to move Player in.
        //     AttemptMove<Wall> (horizontal, vertical);
        // }
        
        // FIXME
        // Debug.Log("finished update");
        // Debug.Log("blood level: " + bloodLevel);
        
        Vector2 movementNormalized = GetInput();
        Move(movementNormalized * speed);
    }

    // AttemptMove overrides the AttemptMove function in the base class MovingObject
    // AttemptMove takes a generic parameter T which for Player will be of the type Wall, it also takes integers for x and y direction to move in.
    protected override void AttemptMove <T> (int xDir, int yDir)
    {
        // Every time player moves, subtract from blood level total.
        // FIXME: don't need to decrease blood level for now...
        //bloodLevel--;
        
        Debug.Log("In AttemptMove");

        // Call the AttemptMove method of the base class, passing in the component T (in this case Wall) and x and y direction to move.
        base.AttemptMove <T> (xDir, yDir);

        // Hit allows us to reference the result of the Linecast done in Move.
        RaycastHit2D hit;

        // If Move returns true, meaning Player was able to move into an empty space.
        if (Move (xDir, yDir, out hit)) 
        {
            // Call RandomizeSfx of SoundManager to play the move sound, passing in two audio clips to choose from.
        }

        // Since the player has moved and lost food points, check if the game has ended.
        CheckIfGameOver();

        // FIXME: not turns so commented this out
        // Set the playersTurn boolean of GameManager to false now that players turn is over.
        // GameManager.instance.playersTurn = false;
    }
    
    // OnCantMove overrides the abstract function OnCantMove in MovingObject.
    // It takes a generic parameter T which in the case of Player is a Wall which the player can attack and destroy. 
    // FIXME: removed generic to set type as Wall (just for the player...)
    // EDIT: the inherited method needs the generic in the function definition...
    protected override void OnCantMove <T> (T component)
    {
        // Set hitWall to equal the component passed in as a parameter.
        Wall hitWall = component as Wall;
        
        //Call the DamageWall function of the Wall we are hitting.
        hitWall.DamageWall(wallDamage);
        
        //Set the attack trigger of the player's animation controller in order to play the player's attack animation. 
        // NOTE: the trigger has to be spelled EXACTLY the same (case sensitive) as the trigger in the player controller
        animator.SetTrigger("PlayerChop");
    }

    // OnTriggerEnter2D is sent when another object enters a trigger collider attached to this object (2D physics only).
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the tag of the trigger collided with is Exit.
        // NOTE: need to use updated tags, like "Infusion" and "Bloodpack"
        if (other.tag == "Exit")
        {
            // Invoke the Restart function to start the next level with a delay of restartLevelDelay (default 1 second).
            Invoke("Restart", restartLevelDelay);
            
            // Disable the player object since level is over.
            enabled = false;
        }
        
        // Check if the tag of the trigger collided with is Food.
        else if (other.tag == "Infusion")
        {
            // TODO: keep track of the number of items?
            
            // Disable the infusion object the player collided with.
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Bloodpack")
        {
            // TODO: keep track of the number of items?
            
            //Disable the bloodpack object the player collided with.
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Enemy")
        {
            // TODO: need to activate game over screen here...
            
            Debug.Log("DIE");
            
            // Call the GameOver function of GameManager.
            GameManager.instance.GameOver();
        }
    }
    
    // Restart reloads the scene when called.
    private void Restart()
    {
        // Scene scene = SceneManager.GetActiveScene();
        // SceneManager.LoadScene(scene.name);
        
        // FIXME: used the updated code at the END of the video instead (fourth video)
        // Load the last scene loaded, in this case Main, the only scene in the game.
        SceneManager.LoadScene(0);
    }
    
    // CheckIfGameOver checks if the player is out of food points and if so, ends the game.
    private void CheckIfGameOver()
    {
        // FIXME: set condition to some negative value for testing
        if (bloodLevel <= -100)
        {
            //Call the GameOver function of GameManager.
            GameManager.instance.GameOver();
        }
    }

    private void Move(Vector2 velocity)
    {
        moverBody.velocity = velocity;
    }
}
