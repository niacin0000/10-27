using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityStandardAssets.Utility;
using System;

public class MouseFocus : MonoBehaviourPunCallbacks
{
    Vector3 V3;
    public float turnspeed = 2f;
    private Transform tr;

    Camera cameraFocuse;

    private void Start()
    {
        tr = GetComponent<Transform>();
        cameraFocuse = GameObject.Find("Main Camera").gameObject.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {

            LookMouse();
            //???????
        }
    }


    public void LookMouse()
    {
        Ray ray = cameraFocuse.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            Vector3 mouseDir = new Vector3(hit.point.x, transform.position.y, hit.point.z) - transform.position;
            this.transform.forward = mouseDir;
        }
    }

}
