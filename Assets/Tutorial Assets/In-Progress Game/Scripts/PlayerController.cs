using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;

    public Rigidbody2D moverBody;
    
    void Update()
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
