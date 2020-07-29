using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_Wall : MonoBehaviour
{
    Rigidbody rb;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //Physics.gravity = new Vector3(0, -100, 0);
    }

    void Update()
    {
        rb.AddForce(Vector3.down * 1000f);
        
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("FLOOR"))
        {
            //rb.constraints = RigidbodyConstraints.FreezeAll;
            rb.isKinematic = true;
        }
    }

}
