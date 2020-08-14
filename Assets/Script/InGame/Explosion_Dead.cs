using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion_Dead : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("dead", 0.8f);
    }

    public void dead()
    {
        Destroy(gameObject);
    }
}
