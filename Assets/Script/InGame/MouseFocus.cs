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

            //Vector3 mPosition = Input.mousePosition; //마우스 좌표 저장
            //Vector3 oPosition = transform.position; //게임 오브젝트 좌표 저장

            //mPosition.z = oPosition.z - Camera.main.transform.position.z;
            ////mPosition.y = oPosition.y - Camera.main.transform.position.y;

            //Vector3 target = Camera.main.ScreenToWorldPoint(mPosition);

            //float dy = target.z - oPosition.z;
            //float dx = target.x - oPosition.x;

            //float rotateDegree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;

            ////transform.rotation = Quaternion.Euler(0f, -rotateDegree + 90, 0);
            //transform.rotation = Quaternion.Euler(0f, rotateDegree, 0f);
            LookMouse();
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
