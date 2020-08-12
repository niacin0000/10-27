using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityStandardAssets.Utility;
using System;

public class MouseVector : MonoBehaviourPunCallbacks
{
    private Transform tr;
    Camera cameraFocuse;
    private bool attacking = false;

    private void Start()
    {
        tr = GetComponent<Transform>();
        cameraFocuse = GameObject.Find("Main Camera").gameObject.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (attacking == false)
        {
            if (photonView.IsMine)
            {
                //NorayMouse();
                LookMouse();
            }
        }


        if (Input.GetMouseButtonDown(1))
        {
            attacking = true;
            Invoke("EndRPC", 3f);
        }
    }


    public void LookMouse()
    {
        Ray ray = cameraFocuse.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            //Vector3 mouseDir = new Vector3(hit.point.x, transform.position.y+65, hit.point.z) - transform.position;
            Vector3 mouseDir = new Vector3(hit.point.x, transform.position.y, hit.point.z) - transform.position;
            this.transform.forward = mouseDir;
        }
    }


    void EndRPC()
    {
        photonView.RPC("EndFocus", RpcTarget.AllViaServer, null);
    }

    [PunRPC]
    void EndFocus()
    {
        attacking = false;
    }
}