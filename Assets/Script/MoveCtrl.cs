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
    private float h, v;
    private Rigidbody rb;
    private Transform tr;

    public float speed = 10.0f;

    public Text nickName;


    //체력 게이지
    public float currHP = 100.0f;

    //대쉬 게이지
    public float currDP = 100.0f;

    bool dash_c = true;
    bool dashR_c = false;


    private bool isDie = false;
    public float respawnTime = 3.0f;
    bool td_c = true;

    //물건을 들었는지 상태여부
    public bool isPicking = false;

    ItemEquip itemequip;

    private GameObject settarget_I;
    public GameObject body;

    public GameObject playerEquipPoint, playerTakeDownPoint;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();

        if (photonView.IsMine)
        {
            Camera.main.GetComponent<SmoothFollow>().target = tr.Find("CamPivot").transform;
        }
        else
        {
            GetComponent<Rigidbody>().isKinematic = true;  //물리충돌 일어나지 않게 isKinematic
        }

        playerEquipPoint = GameObject.FindGameObjectWithTag("EquipPoint");
        playerTakeDownPoint = GameObject.Find("TakeDownPoint");

        nickName.text = photonView.Owner.NickName;
    }

    // Update is called once per frame
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

            //마우스 우클릭
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

    void Dash()
    {
        tr.Translate(Vector3.forward * v * speed * 7 * Time.deltaTime);
        tr.Translate(Vector3.right * h * speed * 7 * Time.deltaTime);

        Debug.Log("Dash");
    }

    private void CancelDash()
    {
        CancelInvoke("Dash");
        dash_c = true;
    }

    private void RecoveryDP_c()
    {
        dashR_c = true;
    }

    private void RecoveryDP()
    {

        if (currDP < 100 && dashR_c == true)
        {
            currDP += 3f;

            Debug.Log(currDP);
        }

        if (currDP >= 100)
        {
            CancelInvoke("RecoveryDP");
            currDP = 100;
            dashR_c = false;
            Debug.Log(currDP);
        }
    }

    private void TakeDamage_c()
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
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BLOCK") && !isDie)
        {
            speed = 5.0f;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("ITEMS") && !isDie && isPicking == false)
        {
            Debug.Log(GameObject.FindGameObjectWithTag("ITEMS").name);
            settarget_I = other.gameObject;
        }

    }
    public void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("BLOCK")&&!isDie)
        {
            speed = 10.0f;
        }
    }

    public void Pickup(GameObject item)
    { 
        SetEquip(item, true);

        item.transform.SetParent(playerEquipPoint.transform);
        item.transform.localPosition = Vector3.zero;
        item.transform.rotation = new Quaternion(0, 0, 0, 0);

        isPicking = true;
    }

    public void Drop(GameObject item)
    {
        SetEquip(settarget_I, false);

        item.transform.position = playerTakeDownPoint.transform.position;

        playerEquipPoint.transform.DetachChildren();

        isPicking = false;
    }

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
}
