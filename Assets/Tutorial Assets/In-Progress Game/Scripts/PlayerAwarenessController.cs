using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAwarenessController : MonoBehaviour
{

    public bool Aware { get; private set; }

    public Vector2 DirectionToPlayer { get; private set; }

    [SerializeField]
    private float _playerAwarenessDistance;

    private Transform _player;

    // TODO: might not work with procedural gen
    private void Awake()
    {
        _player = FindObjectOfType<Player>().transform;
    }


    void Update()
    {
        Vector2 enemyToPlayerVector = _player.position - transform.position;
        DirectionToPlayer = enemyToPlayerVector.normalized;

        Aware = (enemyToPlayerVector.magnitude <= _playerAwarenessDistance);
    }
}
