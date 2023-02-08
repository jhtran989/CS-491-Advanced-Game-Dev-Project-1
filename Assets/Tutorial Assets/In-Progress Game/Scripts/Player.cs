using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// FIXME: changed MonoBehavior to MovingObject
// TODO: can enemies affect the items of the player (like the bloodpacks)? - LoseFood()

public class Player : MovingObject
{
    // in-game stats
    public float speed;
    public int wallDamage = 1;
    public int pointsPerBloodpack = 10;
    public int pointsPerInfusion = 20;
    public float restartLevelDelay = 1f;


    private Animator animator;
    private int bloodLevel;

    public Rigidbody2D moverBody;
    
    void Start()
    {
        animator = GetComponent<Animator>();

        bloodLevel = GameManager.instance.playerBloodLevel;

        base.Start();
    }

    private void OnDisable()
    {
        GameManager.instance.playerBloodLevel = bloodLevel;
    }

    private void CheckIfGameOver()
    {
        if (bloodLevel <= 0)
        {
            GameManager.instance.GameOver();
        }
    }
    
    // FIXME: removed generic to set type as Wall (just for the player...)
    // EDIT: the inherited method needs the generic in the function definition...
    protected override void OnCantMove <T> (T component)
    {
        Wall hitWall = component as Wall;
        hitWall.DamageWall(wallDamage);
        
        // NOTE: the trigger has to be spelled EXACTLY the same (case sensitive) as the trigger in the player controller
        animator.SetTrigger("PlayerChop");
        
    }
    

    private void Restart()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // NOTE: need to use updated tags, like "Infusion" and "Bloodpack"
        if (other.tag == "Exit")
        {
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        }
        else if (other.tag == "Infusion")
        {
            // TODO: keep track of the number of items?
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Bloodpack")
        {
            // TODO: keep track of the number of items?
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Enemy")
        {
            Debug.Log("DIE");
        }
    }
    
    protected void Update()
    {
        Vector2 movementNormalized = GetInput();
        Move(movementNormalized * speed);
    }

    private Vector2 GetInput()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");
        return new Vector2(inputX, inputY).normalized;
    }

    private void Move(Vector2 velocity)
    {
        moverBody.velocity = velocity;
    }
}
