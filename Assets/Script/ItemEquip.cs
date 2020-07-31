using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class ItemEquip : MonoBehaviourPunCallbacks, IPunObservable
{
    private Vector3 currPos;    // 실시간으로 전송하고 받는 변수
    private Quaternion currRot; // 실시간으로 전송하고 받는 변수
    private bool currPick;
    private Transform tr;   //오브젝트 트랜스폼
    public bool I_picking = false;  //현재 아이템이 피킹되있는지 확인해서 콜라이더 변경

    void Start()
    {
        tr = GetComponent<Transform>();
    }

    void Update()
    {
        if (!photonView.IsMine)
        {
            if ((tr.position - currPos).sqrMagnitude >= 10.0f * 10f)//끊어진 시간이 너무 길 경우(텔레포트)
            {
                tr.position = currPos;
                tr.rotation = currRot;
            }
            else //끊어진 시간이 짧을 경우(자연스럽게 연결 - 데드레커닝)
            {
                tr.position = Vector3.Lerp(tr.position, this.currPos, Time.deltaTime * 10.0f);
                tr.rotation = Quaternion.Slerp(tr.rotation, this.currRot, Time.deltaTime * 10.0f);
            }

            if (this.currPick)
            {
                this.GetComponent<Collider>().enabled = false;
                this.GetComponent<Rigidbody>().isKinematic = true;
            }
            else if (!this.currPick)
            {
                this.GetComponent<Collider>().enabled = true;
                this.GetComponent<Rigidbody>().isKinematic = false;
            }
        }


        //Debug.Log(this.name + this.currPick + "C");
        //Debug.Log(this.name + this.I_picking + "I");
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) //데이터를 계속 전송만
        {
            stream.SendNext(tr.position);   //내 위치값을 보낸다
            stream.SendNext(tr.rotation);   //내 회전값을 보낸다
            stream.SendNext(I_picking);
        }
        else
        {
            //stream.ReceiveNext()는 오브젝트 타입이라  currPos에 맞게 vector3로 변경해준다.
            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
            currPick = (bool)stream.ReceiveNext();
        }
    }
}
