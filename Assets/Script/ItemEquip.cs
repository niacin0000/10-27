using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class ItemEquip : MonoBehaviourPunCallbacks, IPunObservable
{
    GameObject player;
    public GameObject playerEquipPoint, playerTakeDownPoint;
    
    MoveCtrl playerFunction;
    private Transform tr;
    bool isPlayerEnter = false;


    private void Start()
    {
        tr = GetComponent<Transform>();
    }


    // Update is called once per frame
    void Update()
    {
        if(photonView.IsMine)
        {
            if (Input.GetButtonDown("Fire1") && isPlayerEnter && playerFunction.isPicking == false)
            {
                Debug.Log("들기");
                transform.SetParent(playerEquipPoint.transform);
                transform.localPosition = Vector3.zero;
                transform.rotation = new Quaternion(0, 0, 0, 0);

                isPlayerEnter = false;

                photonView.RPC("RPCPick", RpcTarget.AllViaServer, null);

            }

            if (Input.GetButtonDown("Fire3") && playerFunction.isPicking)
            {
                Debug.Log("내려놓기");

                photonView.RPC("RPCDrop", RpcTarget.AllViaServer, null);

            }

        }
        else
        {
            if ((tr.position - currPos).sqrMagnitude >= 10.0f * 10.0f)
            {
                tr.position = currPos;
                tr.rotation = currRot;
            }
            else
            {
                tr.position = Vector3.Lerp(tr.position, currPos, Time.deltaTime * 10);
                tr.rotation = Quaternion.Slerp(tr.rotation, currRot, Time.deltaTime * 10);
                //playerFunction.isPicking = pick;
            }

        }


        if (!playerFunction.isPicking)
        {
            this.GetComponent<Collider>().enabled = true;
            this.GetComponent<Rigidbody>().isKinematic = false;

        }
    }

    [PunRPC]
    void RPCPick()
    {
        playerFunction.Pickup(this.gameObject);
    }

    [PunRPC]
    void RPCDrop()
    {
        playerFunction.Drop();
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ROBO"))
        {
            player = other.gameObject;
            playerEquipPoint = player.transform.Find("ItemPoint").gameObject;
            playerTakeDownPoint = player.transform.Find("head").gameObject.gameObject.transform.Find("TakeDownPoint").gameObject;

            playerFunction = player.GetComponent<MoveCtrl>();
            isPlayerEnter = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ROBO"))
        {
            isPlayerEnter = false;
        }
    }



    private Vector3 currPos;    // 실시간으로 전송하고 받는 변수
    private Quaternion currRot; // 실시간으로 전송하고 받는 변수
    //private bool pick;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) //데이터를 계속 전송만
        {
            stream.SendNext(tr.position);   //내 위치값을 보낸다
            stream.SendNext(tr.rotation);   //내 회전값을 보낸다
            //stream.SendNext(playerFunction.isPicking);
        }
        else
        {
            //stream.ReceiveNext()는 오브젝트 타입이라  currPos에 맞게 vector3로 변경해준다.
            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
            //pick = (bool)stream.ReceiveNext();
        }
    }

}
