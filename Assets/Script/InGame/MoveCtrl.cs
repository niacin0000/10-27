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
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MoveCtrl : MonoBehaviourPunCallbacks, IPunObservable
{
    private float h, v; //이동에 쓰는코드 (위아래, 좌우)
    private Transform tr, tr_f; //오브젝트의 트랜스폼, foot의 트랜스폼
    public bool t_Damage = false;

    private Rigidbody rb;
    private Vector3 foot_v;

    public float speed = 10.0f; //무브스피드

    public Text nickName; //플레이어 닉네임

    public Image hpbar; //플레이어 hpbar
    public Image dpbar; //플레이어 dpbar


    public float currHP = 100.0f; //체력 게이지
    private float initHP = 100.0f;

    public float currDP = 100.0f; //대쉬 게이지
    private float initDP = 100.0f;


    bool dash_c = true; //대쉬 상태여부
    bool dashR_c = false; //대쉬회복 상태여부


    private bool isDie = false; //캐릭터사망
    public float respawnTime = 3.0f; //리스폰시간
    bool td_c = true; //데미지받음(TakeDamage) 상태여부

    public GameObject Me, Mouse_Vector;
    int start_time = 0;

    public GameObject Espadon, Shield, Staff, Sword;    //무기
    public bool Es_active = false, Sh_active = false, St_active = false, Sw_active = false;    //집은무기 확인
    private Color Original_Color_St, Original_Color_Es, Original_Color_Sw, Original_Color_Sh;   //무기 색

    private bool hittime = true;    //무적

    // 오디오 관련 변수
    public AudioClip[] audioClips;
    AudioSource audiosource;
    public AudioMixerGroup audioMixerGroup;

    private void Awake()
    {
        //DontDestroyOnLoad(gameObject); //씬변환시 부수지않음
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

        nickName.text = photonView.Owner.NickName; //닉네임가져오기

        Original_Color_St = Staff.GetComponent<MeshRenderer>().material.color;
        Original_Color_Es = Espadon.GetComponent<MeshRenderer>().material.color;
        Original_Color_Sw = Sword.GetComponent<MeshRenderer>().material.color;
        Original_Color_Sh = Shield.GetComponent<MeshRenderer>().material.color;

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
                    photonView.RPC("Dash_Pun", RpcTarget.AllViaServer, null);
                }
            }

            //자가데미지체크
            if (Input.GetKeyDown(KeyCode.B))
            {
                photonView.RPC("selfDamage", RpcTarget.AllViaServer, null);
                Knockback();
            }

            //쥬금
            if (currHP <= 0)
            {
                Dead();
                photonView.RPC("Destroy_Me", RpcTarget.AllViaServer, null);
                photonView.RPC("DeadCountUp", RpcTarget.AllViaServer, null);
            }

            if (start_time < 20)
                start_time++;
            //이김
            if (GameObject.FindGameObjectsWithTag("Player").Length == 1 && start_time > 15)
            {
                //Invoke("Win", 3f);

            }

            //Debug.Log(GameObject.FindGameObjectsWithTag("Player").Length + "pla");
            //Debug.Log(start_time + "time");
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


        //체력포인트바 업데이트
        photonView.RPC("HPbarUpdate", RpcTarget.AllViaServer, null);

        //대쉬포인트바 업데이트
        photonView.RPC("DPbarUpdate", RpcTarget.AllViaServer, null);

        if (transform.Find("Staff").gameObject.activeSelf == true)
        {
            St_active = true;
        }
        if (transform.Find("Espadon").gameObject.activeSelf == true)
        {
            Es_active = true;
        }
        if (transform.Find("Sword").gameObject.activeSelf == true)
        {
            Sw_active = true;
        }
        if (transform.Find("Shield").gameObject.activeSelf == true)
        {
            Sh_active = true;
        }


        //Debug.Log(transform.Find("Espadon").GetComponent<Animator>().GetBool("IsAttack"));
    }


    [PunRPC]
    void HPbarUpdate()
    {
        this.hpbar.fillAmount = currHP / initHP;
    }

    [PunRPC]
    void DPbarUpdate()
    {
        this.dpbar.fillAmount = currDP / initDP;
    }



    [PunRPC]
    void selfDamage()
    {
        this.currHP -= 1;
    }

    [PunRPC]
    void Dash_Pun()
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


    void Dash() //대쉬
    {
        tr.Translate(Vector3.forward * v * speed * 3 * Time.deltaTime);
        tr.Translate(Vector3.right * h * speed * 3 * Time.deltaTime);

        //Debug.Log("Dash");
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
        ////적이라는 태그를 가진 오브젝트와 닿았을 때
        //if (collision.collider.CompareTag("ENEMY") && !isDie && td_c)
        //{

        //    currHP -= 20.0f;
        //    td_c = false;
        //    body.GetComponent<MeshRenderer>().material.color = Color.red;
        //    Invoke("TakeDamage_c", 2f);

        //    if (photonView.IsMine && currHP <= 0.0f)
        //    {
        //        isDie = true;
        //        Debug.Log("Die");

        //    }
        //}
    }

    public void OnTriggerEnter(Collider other)
    {
        //FIREBALL 이라는 태그를 가진 오브젝트와 닿았을 때
        if (other.CompareTag("FIREBALL") && !isDie && hittime && St_active == false)
        {
            hittime = false;
            Invoke("hittimer", 3f);
            HitSound();
            Knockback();
            photonView.RPC("Hit_Fireball", RpcTarget.AllViaServer, null);
        }

        //ESPA 이라는 태그를 가진 오브젝트와 닿았을 때
        if (other.CompareTag("ESPA") && !isDie && hittime && Es_active == false
            && other.transform.parent.GetComponent<Animator>().GetBool("IsAttack"))
        {
            hittime = false;
            Invoke("hittimer", 3f);
            HitSound();
            Knockback();
            photonView.RPC("Hit_Fireball", RpcTarget.AllViaServer, null);
        }

        //SWORD 이라는 태그를 가진 오브젝트와 닿았을 때
        if (other.CompareTag("SWORD") && !isDie && hittime && Sw_active == false
            && other.transform.parent.GetComponent<Animator>().GetBool("IsAttack"))
        {
            hittime = false;
            Invoke("hittimer", 3f);
            HitSound();
            Knockback();
            photonView.RPC("Hit_Fireball", RpcTarget.AllViaServer, null);
        }

        //SHIELD 이라는 태그를 가진 오브젝트와 닿았을 때
        if (other.CompareTag("SHIELD") && !isDie && hittime && Sh_active == false
            && other.transform.parent.GetComponent<Animator>().GetBool("IsAttack"))
        {
            hittime = false;
            Invoke("hittimer", 3f);
            HitSound();
            Knockback();
            photonView.RPC("Hit_Fireball", RpcTarget.AllViaServer, null);
        }

    }

    public void OnTriggerExit(Collider other)
    {
        ////BLOCK이라는 태그를 가진 오브젝트에 나갔을 때
        //if (other.CompareTag("BLOCK") && !isDie)
        //{
        //    speed = 10.0f;
        //}
    }

    public void Dead()
    {
        GameObject.Find("GameManager").GetComponent<GameMgr>().Lose_panel();
    }

    public void Win()
    {
        GameObject.Find("GameManager").GetComponent<GameMgr>().Win_panel();
    }

    [PunRPC]
    public void Destroy_Me()
    {
        Destroy(Me);

    }


    public void hittimer()
    {
        hittime = true;
    }

    public void Knockback()
    {
        t_Damage = true;
        this.transform.position += -Mouse_Vector.transform.forward * 200 * Time.deltaTime;
        photonView.RPC("HitColor", RpcTarget.AllViaServer, null);
        Invoke("HitTimer_Originalcolor", 3f);
    }

    public void HitSound()
    {
        if(Es_active == true)
        {
            audiosource.clip = audioClips[0];
            audiosource.outputAudioMixerGroup = audioMixerGroup;
            audiosource.Play();
            audiosource.loop = false;
        }
        else if(Sh_active == true)
        {
            audiosource.clip = audioClips[1];
            audiosource.outputAudioMixerGroup = audioMixerGroup;
            audiosource.Play();
            audiosource.loop = false;
        }
        else if(St_active == true)
        {
            audiosource.clip = audioClips[2];
            audiosource.outputAudioMixerGroup = audioMixerGroup;
            audiosource.Play();
            audiosource.loop = false;
        }
        else if(Sw_active == true)
        {
            audiosource.clip = audioClips[3];
            audiosource.outputAudioMixerGroup = audioMixerGroup;
            audiosource.Play();
            audiosource.loop = false;
        }
    }

    public void HitTimer_Originalcolor()
    {
        photonView.RPC("OriginalColor", RpcTarget.AllViaServer, null);
    }


    [PunRPC]
    public void HitColor()
    {
        if(St_active)
        {
            Staff.GetComponent<MeshRenderer>().material.color = new Color(255, 255, 255, 0);
        }
        if (Es_active)
        {
            Espadon.GetComponent<MeshRenderer>().material.color = new Color(255, 255, 255, 0);
        }
        if (Sw_active)
        {
            Sword.GetComponent<MeshRenderer>().material.color = new Color(255, 255, 255, 0);
        }
        if (Sh_active)
        {
            Shield.GetComponent<MeshRenderer>().material.color = new Color(255, 255, 255, 0);
        }
    }

    [PunRPC]
    public void OriginalColor()
    {
        if (St_active)
        {
            Staff.GetComponent<MeshRenderer>().material.color = Original_Color_St;
        }
        if (Es_active)
        {
            Espadon.GetComponent<MeshRenderer>().material.color = Original_Color_Es;
        }
        if (Sw_active)
        {
            Sword.GetComponent<MeshRenderer>().material.color = Original_Color_Sw;
        }
        if (Sh_active)
        {
            Shield.GetComponent<MeshRenderer>().material.color = Original_Color_Sh;
        }
    }


    [PunRPC]
    public void Hit_Fireball()
    {
        this.currHP -= 5;
    }



}
