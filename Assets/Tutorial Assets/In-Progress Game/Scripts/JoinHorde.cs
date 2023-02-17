using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinHorde : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            Destroy(gameObject);
            
            // TODO: maybe increase horde speed
        }
    }
}
