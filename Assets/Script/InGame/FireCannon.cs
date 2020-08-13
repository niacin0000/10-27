using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UIElements;

public class FireCannon : MonoBehaviourPunCallbacks
{

    public Animator animator;
    public bool attacking = false;
    public GameObject firevector, fireball, explosion;
    private GameObject fired_fireball;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
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
                Invoke("Explosion", 0.95f);
                Debug.Log("fire");
            }


        }
    }



    [PunRPC]
    void Fire()
    {
        animator.SetBool("IsAttack", true);
        Instantiate(fireball, firevector.transform.position, firevector.transform.rotation);

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



    public void Explosion()
    {

        photonView.RPC("CreateExplosion", RpcTarget.AllViaServer, null);
    }

    [PunRPC]
    public void CreateExplosion()
    {
        fired_fireball = GameObject.Find("Magic fire pro orange(Clone)").gameObject;

        Instantiate(explosion, fired_fireball.transform.position, fired_fireball.transform.rotation);
    }

}
