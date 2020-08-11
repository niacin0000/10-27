using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FireCannon : MonoBehaviourPunCallbacks
{

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;

        if (photonView.IsMine)
        {
            if (Input.GetMouseButtonDown(1))
            {
                int actorNumver = photonView.Owner.ActorNumber;
                photonView.RPC("Fire", RpcTarget.Others, actorNumver);
                Fire();
                //Invoke("EndFireRPC", 3f);
                Debug.Log("fire");
            }


        }
    }



    [PunRPC]
    void Fire()
    {
        //Instantiate(cannon, firePos.position, firePos.rotation);
        animator.SetBool("IsAttack", true);
        Debug.Log(animator.GetBool("IsAttack"));
        Debug.Log("총쏨");
    }


    void EndFireRPC()
    {
        photonView.RPC("EndFire", RpcTarget.AllViaServer, null);
    }

    [PunRPC]
    void EndFire()
    {
        animator.SetBool("IsAttack", false);
    }
}
