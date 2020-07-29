using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityStandardAssets.Utility;
using TMPro;
using UnityEngine.UI;

public class MoveCtrl : MonoBehaviourPunCallbacks, IPunObservable
{
    private float h, v; //이동에 쓰는코드 (위아래, 좌우)
    private Transform tr; //오브젝트의 트랜스폼
    private Rigidbody rb;

    public float speed = 10.0f; //무브스피드

    public Text nickName; //플레이어 닉네임

    public float currHP = 100.0f; //체력 게이지

    public float currDP = 100.0f; //대쉬 게이지

    bool dash_c = true; //대쉬 상태여부
    bool dashR_c = false; //대쉬회복 상태여부


    private bool isDie = false; //캐릭터사망
    public float respawnTime = 3.0f; //리스폰시간
    bool td_c = true; //데미지받음(TakeDamage) 상태여부

    public bool isPicking = false; //물건을 들었는지 상태여부

    private GameObject settarget_I; //현재 부딪힌 아이템 타게팅
    public GameObject body; //캐릭터 몸체

    public GameObject playerEquipPoint, playerTakeDownPoint; //아이템 들고 내려두는 포인트

    private void Awake()
    {
        DontDestroyOnLoad(gameObject); //씬변환시 부수지않음
    }

    void Start()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();

        if (photonView.IsMine)
        {
            Camera.main.GetComponent<SmoothFollow>().target = tr.Find("CamPivot").transform; //카메라가 캠피봇을 따라감
        }
        else
        {
            GetComponent<Rigidbody>().isKinematic = true;  //물리충돌 일어나지 않게 isKinematic
        }

        playerEquipPoint = this.transform.Find("ItemPoint").gameObject; //아이템 픽업 포인터 지정
        playerTakeDownPoint = this.transform.Find("head").transform.Find("TakeDownPoint").gameObject; //아이템 다운 포인터 지정

        nickName.text = photonView.Owner.NickName; //닉네임가져오기
    }

    void Update()
    {
        if (photonView.IsMine && !isDie)
        {
            //이동
            if (dash_c == true)
            {
                v = Input.GetAxis("Vertical");
                h = Input.GetAxis("Horizontal");
                tr.Translate(Vector3.forward * v * speed * Time.deltaTime);
                tr.Translate(Vector3.right * h * speed * Time.deltaTime);
            }

            //대쉬
            if (Input.GetKeyDown(KeyCode.Space))
            {

                if (currDP >= 33.3f && dash_c == true)
                {
                    CancelInvoke("RecoveryDP");
                    dashR_c = false;
                    dash_c = false;
                    InvokeRepeating("Dash", 0.0f, 0.02f);
                    Invoke("CancelDash", 0.2f);

                    Invoke("RecoveryDP_c", 2f);
                    InvokeRepeating("RecoveryDP", 2f, 0.1f);

                    currDP -= 33.3f;
                }
            }

            //자가데미지체크
            if (Input.GetKeyDown(KeyCode.B))
            {
                currHP -= 20;
            }

            //마우스 좌클릭
            if (Input.GetButtonDown("Fire1") && isPicking == false)
            {
                Pickup(settarget_I);
            }

            //마우스 가운데클릭
            if (Input.GetButtonDown("Fire3") && isPicking == true)
            {
                Drop(settarget_I);
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
                tr.position = Vector3.Lerp(tr.position, currPos, Time.deltaTime * 10.0f);
                tr.rotation = Quaternion.Slerp(tr.rotation, currRot, Time.deltaTime * 10.0f);
            }
        }
    }

    void Dash() //대쉬
    {
        tr.Translate(Vector3.forward * v * speed * 7 * Time.deltaTime);
        tr.Translate(Vector3.right * h * speed * 7 * Time.deltaTime);

        Debug.Log("Dash");
    }

    private void CancelDash() //대쉬중단
    {
        CancelInvoke("Dash");
        dash_c = true;
    }

    private void RecoveryDP_c() //대쉬포인트 회복 여부
    {
        dashR_c = true;
    }

    private void RecoveryDP() //대쉬포인트 회복
    {

        if (currDP < 100 && dashR_c == true)
        {
            currDP += 3f;
        }

        if (currDP >= 100)
        {
            CancelInvoke("RecoveryDP");
            currDP = 100;
            dashR_c = false;
        }
    }

    private void TakeDamage_c() //데미지받음(색변환)
    {
        body.GetComponent<MeshRenderer>().material.color = Color.blue;
        td_c = true;
    }


    private Vector3 currPos;    // 실시간으로 전송하고 받는 변수
    private Quaternion currRot; // 실시간으로 전송하고 받는 변수
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) //데이터를 계속 전송만
        {
            stream.SendNext(tr.position);   //내 위치값을 보낸다
            stream.SendNext(tr.rotation);   //내 회전값을 보낸다
        }
        else
        {
            //stream.ReceiveNext()는 오브젝트 타입이라  currPos에 맞게 vector3로 변경해준다.
            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        //적이라는 태그를 가진 오브젝트와 닿았을 때
        if (collision.collider.CompareTag("ENEMY") && !isDie && td_c)
        {

            currHP -= 20.0f;
            td_c = false;
            body.GetComponent<MeshRenderer>().material.color = Color.red;
            Invoke("TakeDamage_c", 2f);

            if (photonView.IsMine && currHP <= 0.0f)
            {
                isDie = true;
                Debug.Log("Die");

            }
        }

        //POTAL이라는 태그를 가진 오브젝트와 닿았을 때
        if (collision.collider.CompareTag("POTAL") && !isDie)
        {
            PhotonNetwork.LoadLevel("Level_1");
        }


        ////혹시 바닥에 떨어지고나서부터 캐릭터의 y값을 변환시키고 싶지 않을때
        //if (collision.collider.CompareTag("FLOOR"))
        //{
        //    rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        //}
    }

    public void OnTriggerEnter(Collider other)
    {
        //BLOCK이라는 태그를 가진 오브젝트와 닿았을 때
        if (other.CompareTag("BLOCK") && !isDie)
        {
            speed = 5.0f;
        }

        //BOSSWALLHIT이라는 태그를 가진 오브젝트와 닿았을 때
        if (other.CompareTag("BOSSWALLHIT") && !isDie && td_c)
        {
            currHP -= 20.0f;
            td_c = false;
            body.GetComponent<MeshRenderer>().material.color = Color.red;
            tr.Translate(Vector3.back * 500 * Time.deltaTime);
            Invoke("TakeDamage_c", 2f);

            if (photonView.IsMine && currHP <= 0.0f)
            {
                isDie = true;
                Debug.Log("Die");
            }
        }
    }

    public void OnTriggerStay(Collider other)
    {
        //ITEMS이라는 태그를 가진 오브젝트와 닿고 있는중
        if (other.CompareTag("ITEMS") && !isDie && isPicking == false)
        {
            settarget_I = other.gameObject;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        //BLOCK이라는 태그를 가진 오브젝트에 나갔을 때
        if (other.CompareTag("BLOCK") && !isDie)
        {
            speed = 10.0f;
        }
        //ITEMS이라는 태그를 가진 오브젝트에 나갔을 때
        if (other.CompareTag("ITEMS") && !isPicking)
        {
            settarget_I = null;
        }
    }

    //아이템 픽업
    public void Pickup(GameObject item)
    {
        SetEquip(item, true);

        item.transform.SetParent(playerEquipPoint.transform);
        item.transform.localPosition = Vector3.zero;
        item.transform.rotation = new Quaternion(0, 0, 0, 0);

        isPicking = true;
        photonView.RPC("setItemis", RpcTarget.AllViaServer, true);
    }

    //아이템 드랍
    public void Drop(GameObject item)
    {
        SetEquip(settarget_I, false);

        item.transform.position = playerTakeDownPoint.transform.position;

        playerEquipPoint.transform.DetachChildren();

        photonView.RPC("setItemis", RpcTarget.AllViaServer, false);

    }

    //아이템의 콜라이더와 키네마틱 상태 변환
    void SetEquip(GameObject item, bool isEquip)
    {
        Collider[] itemColliders = item.GetComponents<Collider>();
        Rigidbody itemRigidbody = item.GetComponent<Rigidbody>();

        foreach (Collider itemCollider in itemColliders)
        {
            itemCollider.enabled = !isEquip;
        }
        itemRigidbody.isKinematic = isEquip;

    }

    //아이템의 콜라이더와 키네마틱 상태 변환(아이템쪽 코드에서)
    [PunRPC]
    void setItemis(bool ia)
    {
        if (ia)
        {
            settarget_I.GetComponent<ItemEquip>().I_picking = true;
        }
        else if (!ia)
        {
            settarget_I.GetComponent<ItemEquip>().I_picking = false;
            isPicking = false;
            settarget_I = null;
        }

    }
}
