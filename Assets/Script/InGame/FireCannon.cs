using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FireCannon : MonoBehaviourPunCallbacks
{
    public Transform firePos;
    public GameObject cannon;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;

        if (photonView.IsMine)
        {
            if (Input.GetMouseButtonDown(1))
            {
                photonView.RPC("Fire", RpcTarget.AllViaServer, null);
            }
        }
    }

    [PunRPC]
    void Fire()
    {
        Instantiate(cannon, firePos.position, firePos.rotation);

    }
}
