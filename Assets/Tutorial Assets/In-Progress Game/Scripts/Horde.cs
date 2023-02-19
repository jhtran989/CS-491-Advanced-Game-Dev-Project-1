using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horde : MonoBehaviour
{
    private Rigidbody2D _hordeRigidbody2D;
    public float speed;
    private PlayerAwarenessController _playerAwarenessController;
    private Vector2 _targetDirection;
    
    private void Awake()
    {
        _hordeRigidbody2D = GetComponent<Rigidbody2D>();
        _playerAwarenessController = GetComponent<PlayerAwarenessController>();
    }

    private void FixedUpdate()
    {
         UpdateTargetDirection();
         SetVelocity();
    }

    // void Update()
    // {
    //     // transform.position += Vector3.right * (Time.deltaTime * speed);
    //
    //     Vector3 moveDelta = Vector3.right;
    //     // hordeRigidbody2D.MovePosition(transform.position 
    //     //                               + moveDelta * (Time.deltaTime * speed));
    //     // hordeRigidbody2D.AddRelativeForce(Vector2.down);
    //     // hordeRigidbody2D.velocity = Vector2.right * (Time.deltaTime * speed);
    //     // hordeRigidbody2D.AddRelativeForce(Vector2.right * (Time.deltaTime * speed));
    //     hordeRigidbody2D.velocity = Vector2.right * speed;
    // }
    
    private void UpdateTargetDirection()
    {
        if (_playerAwarenessController.Aware)
        {
            _targetDirection = _playerAwarenessController.DirectionToPlayer;
        } 
        else 
        {
            _targetDirection = Vector2.zero;
        }
    }

    private void SetVelocity()
    {
        _hordeRigidbody2D.velocity = _targetDirection * speed;
    }

    // private void OnTriggerEnter2D(Collider2D col)
    // {
    //     if (col.tag == "Player")
    //     {
    //         Player player = col.GetComponent<Player>();
    //         player.LoseBloodHorde();
    //     }
    // }
}
