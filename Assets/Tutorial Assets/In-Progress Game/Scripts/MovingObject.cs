using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{

    public float moveTime = 0.1f;
    public LayerMask blockingLayer;

    private BoxCollider2D boxCollider;
    private Rigidbody2D moverBody;
    private float inverseMoveTime;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        moverBody = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f /moveTime;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
