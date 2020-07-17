using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_S : MonoBehaviour
{ 

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("ROBO"))
        {
            Destroy(gameObject);
        }
    }
}
