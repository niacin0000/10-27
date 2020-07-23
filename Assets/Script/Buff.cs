using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviourPunCallbacks
{
    GameObject target;
    float distance, distance0, distance1, distance2, distance3;
    GameObject robo0, robo1, robo2, robo3;
    bool buff_on, buff_on_m = false;


    public void Update()
    {
        if (GameObject.Find("Player(Clone)"))
        {
            robo3 = GameObject.Find("Player(Clone)");
            distance3 = Vector3.Distance(robo3.transform.position, transform.position);
        }
        else if(GameObject.Find("Robo_J(Clone)"))
        {
            robo1 = GameObject.Find("Robo_J(Clone)");
            distance1 = Vector3.Distance(robo1.transform.position, transform.position);
        }
        else if (GameObject.Find("Robo(Clone)"))
        {
            robo0 = GameObject.Find("Robo(Clone)");
            distance0 = Vector3.Distance(robo0.transform.position, transform.position);
        }
        else if (GameObject.Find("Robo_D(Clone)"))
        {
            robo2 = GameObject.Find("Robo_D(Clone)");
            distance2 = Vector3.Distance(robo2.transform.position, transform.position);
        }
        else
            return;



        //distance = Mathf.Min(distance0, distance1, distance2,distance3);


        if (Input.GetKeyDown(KeyCode.C) && buff_on_m == false)
        {
            this.GetComponent<MoveCtrl>().speed += 10;
            buff_on_m = true;

            Invoke("ResetBuff_m", 5f);
        }


        //Debug.Log(robolist[0].name + "0번째 로봇");
        //Debug.Log(robolist[1].name + "1번째 로봇");
        if (Input.GetKeyDown(KeyCode.V) && buff_on == false)
        {
            //FindTarget();
            photonView.RPC("setBuff", RpcTarget.AllViaServer, null);
        }
    }

    [PunRPC]
    public void setBuff()
    {
        GetComponent<MoveCtrl>().speed += 10;
        buff_on = true;
        Invoke("ResetBuff", 5f);
    }

    public void ResetBuff()
    {
        GetComponent<MoveCtrl>().speed -= 10;
        buff_on = false;
    }

    public void ResetBuff_m()
    {
        this.GetComponent<MoveCtrl>().speed -= 10;
        buff_on_m = false;
    }

    private void FindTarget()
    {
        if (distance == distance0)
            target = robo0;
        else if (distance == distance1)
            target = robo1;
        else if (distance == distance2)
            target = robo2;
        else if (distance == distance3)
            target = robo3;
        else
            target = null;
    }
}
