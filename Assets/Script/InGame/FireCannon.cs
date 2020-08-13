using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FireCannon : MonoBehaviourPunCallbacks
{

    public Animator animator;
    public GameObject parent;
    public bool attacking = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator = parent.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (!photonView.IsMine) return;

        if (photonView.IsMine)
        {
            if (Input.GetMouseButtonDown(1))
            {
                photonView.RPC("Fire", RpcTarget.AllViaServer, null);
                attacking = true;
                Invoke("EndFireRPC", 3f);
                Debug.Log("fire");
            }


        }
    }



    [PunRPC]
    void Fire()
    {
        //Instantiate(cannon, firePos.position, firePos.rotation);
        animator.SetBool("IsAttack", true);
        //Debug.Log(animator.GetBool("IsAttack"));
        //Debug.Log("총쏨");

    }


    void EndFireRPC()
    {
        photonView.RPC("EndFire", RpcTarget.AllViaServer, null);
    }

    [PunRPC]
    void EndFire()
    {
        animator.SetBool("IsAttack", false);

        attacking = false;
    }
}
