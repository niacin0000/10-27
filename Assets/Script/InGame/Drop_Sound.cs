using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop_Sound : MonoBehaviour
{
    private bool one = true;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("GROUND") && one)
        {
            Debug.Log("Drop " + this.name);
            one = false;
        }
    }
}
