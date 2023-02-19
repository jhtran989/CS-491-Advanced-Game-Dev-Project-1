using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    public Transform trackedObject;
    public float updateSpeed = 3;
    public Vector2 trackingOffset;
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = (Vector3) trackingOffset;
        
        // IMPORTANT - need to add a constant amount so z-offset is 0 and doesn't keep increasing/decreasing 
        // EDIT - don't need to complicate things...just set to 0 for 2D
        // EDIT 2 - used position of transform instead of trackedObject for target in MoveTowards below...
        // offset.z = transform.position.z - trackedObject.position.z + 10;
        // offset.z = 0;

        // check if the tracked object is already destroyed
        if (trackedObject.IsDestroyed())
        {
            enabled = false;
        }
        
        // offset is different depending on current and tracked objects (for our case, the offset is 10 between tracker and player
        offset.z = transform.position.z - trackedObject.position.z;
    }

    // Update is called once per frame
    // void Update()
    // {
    //     
    // }

    private void LateUpdate()
    {
        // check if the tracked object is already destroyed
        if (trackedObject.IsDestroyed())
        {
            enabled = false;
            return;
        }
        
        transform.position =
            Vector3.MoveTowards(transform.position, 
                trackedObject.position + offset, 
                updateSpeed * Time.deltaTime);
    }
}
