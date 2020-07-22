using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviourPunCallbacks
{
    GameObject target;
    float distance, distance0, distance1, distance2, distance3;
    GameObject robo0, robo1, robo2;
    bool buff_on, buff_on_m = false;


    public void Update()
    {
        if(GameObject.Find("Robo(Clone)") &&GameObject.Find("Robo_J(Clone)") )
        {
            robo0 = GameObject.Find("Robo(Clone)");
            robo1 = GameObject.Find("Robo_J(Clone)");
            //robo2 = GameObject.Find("Robo_J(Clone)");
        }

        distance0 = Vector3.Distance(robo0.transform.position, transform.position);
        distance1 = Vector3.Distance(robo1.transform.position, transform.position);
        //distance2 = Vector3.Distance(robo2.transform.position, transform.position);
   

        distance = Mathf.Min(distance0, distance1);//, distance2);


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
            FindTarget();
            Debug.Log(target.name);
            photonView.RPC("setBuff", RpcTarget.Others, null);
        }
    }

    [PunRPC]
    public void setBuff()
    {
        Debug.Log(target.name);

        target.GetComponent<MoveCtrl>().speed += 10;
        buff_on = true;
        Invoke("ResetBuff", 5f);
    }

    [PunRPC]
    public void ResetBuff()
    {
        target.GetComponent<MoveCtrl>().speed -= 10;
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
        //else if (distance == distance2)
        //    target = robo2;
        else
            target = null;
    }
}
