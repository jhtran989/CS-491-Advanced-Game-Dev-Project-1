using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MovingObject
{
    [SerializeField]
    private float _speed;

    private Rigidbody2D _rigidbody;
    private PlayerAwarenessController _playerAwarenessController;
    private Vector2 _targetDirection;

    void Start()
    {
        base.Start();
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerAwarenessController = GetComponent<PlayerAwarenessController>();
    }

    private void FixedUpdate()
    {
        UpdateTargetDirection();
        SetVelocity();
    }

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
        _rigidbody.velocity = _targetDirection * _speed;
    }


    protected override void OnCantMove<T>(T component)
    {
        throw new System.NotImplementedException();
    }
}
