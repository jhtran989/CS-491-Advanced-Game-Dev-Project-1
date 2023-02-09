using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horde : MonoBehaviour
{

    public float speed;

    void Update()
    {
        transform.position += Vector3.right * Time.deltaTime * speed;
    }
}
