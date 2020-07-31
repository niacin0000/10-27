using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviourPunCallbacks
{
    bool buff_on = false;
    float cooltime = 10f;


    public void Update()
    {

        if (Input.GetKeyDown(KeyCode.V) && buff_on == false)
        {
            if (this.gameObject == GameObject.Find("Player(Clone)"))
                photonView.RPC("setBuff", RpcTarget.AllViaServer, null);
        }
    }

    [PunRPC]
    void setBuff()
    {
        GetComponent<MoveCtrl>().speed += 10;
        buff_on = true;
        Invoke("ResetBuff", 5f);
    }

    void ResetBuff()
    {
        GetComponent<MoveCtrl>().speed -= 10;
        Invoke("cool", cooltime);
    }

    void cool()
    {
        buff_on = false;
        Debug.Log("CoolTime");
    }


}
