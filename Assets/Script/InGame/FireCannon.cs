using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UIElements;
using UnityEngine.Audio;

public class FireCannon : MonoBehaviourPunCallbacks
{

    public Animator animator;
    public AudioClip[] audioClips;
    AudioSource audiosource;
    public AudioMixerGroup audioMixerGroup;
    public bool attacking = false;
    public GameObject firevector, fireball, explosion, player;
    private GameObject fired_fireball;

    public float fire_delay = 0.5f;

    public SkinnedMeshRenderer skin;
    Mesh BakedMeshResult;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    private Mesh bakedMesh;
    private GameObject ghostContainer;

    public float delayTime = 0.5f;
    private float nowT = 0.0f;

    public Material WeaponMate;

    private Vector3 AfterImagePosition;
    private Color matColor;
    private Color WeaponColor;
    private float AlphaColor = 255.0f;

    private float _time = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audiosource = GetComponent<AudioSource>();
        photonView.RPC("CreateAfterImage", RpcTarget.AllViaServer, null);
    }

    // Update is called once per frame
    void Update()
    {
        //if (!photonView.IsMine) return;
        nowT += Time.deltaTime;
        if (photonView.IsMine)
        {
            if (attacking == false && Input.GetMouseButtonDown(1))
            {
                photonView.RPC("Fire", RpcTarget.AllViaServer, null);
                attacking = true;
                photonView.RPC("WhatTime", RpcTarget.AllViaServer, null);
            }
        }
    }

    [PunRPC]
    void CreateAfterImage()
    {
        nowT = 0.0f;
        bakedMesh = new Mesh();
        ghostContainer = new GameObject("VFX_UnitGhost");
        meshFilter = ghostContainer.AddComponent<MeshFilter>();
        meshFilter.sharedMesh = bakedMesh;

        meshRenderer = ghostContainer.AddComponent<MeshRenderer>();

        ghostContainer.GetComponent<MeshRenderer>().material = WeaponMate;
        ghostContainer.SetActive(false);
    }

    [PunRPC]
    void WhatTime()
    {
        ghostContainer.SetActive(true);
        while (_time > 0f)
        {
            _time -= Time.deltaTime;
            nowT += Time.deltaTime;
            StartCoroutine(unitGhost(_time));
        }
        _time = 1.0f;
        Invoke("setactivefalse", 3f);
    }

    [PunRPC]
    IEnumerator unitGhost(float time)
    {
        if (nowT > delayTime)
        {
            for (float f = 40f; f > 0; f -= 1f)
            {
                //setColor(time);
                AfterImagePosition = this.transform.position;
                AfterImagePosition.x += 0.5f;
                AfterImagePosition.y += 0.5f;
                AfterImagePosition.z -= 1.0f;
                skin.BakeMesh(bakedMesh);
                ghostContainer.transform.rotation = this.transform.rotation;
                ghostContainer.transform.position = AfterImagePosition;
                ghostContainer.transform.localScale = this.transform.localScale * 0.5f;
                nowT = 0;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    void setactivefalse()
    {
        ghostContainer.SetActive(false);
    }

    [PunRPC]
    void Fire()
    {
        animator.SetBool("IsAttack", true);
        if(player.GetComponent<MoveCtrl>().St_active)
        {
            //*nstantiate(fireball, firevector.transform.position, firevector.transform.rotation);*/
        }

        //Debug.Log(animator.GetBool("IsAttack"));
        //Debug.Log("총쏨");

    }

    void OnAttack()
    {
        audiosource.clip = audioClips[0];
        audiosource.outputAudioMixerGroup = audioMixerGroup;
        audiosource.Play();
        audiosource.loop = false;

    }

    void OnAttack1()
    {
        audiosource.clip = audioClips[1];
        audiosource.outputAudioMixerGroup = audioMixerGroup;
        audiosource.Play();
        audiosource.loop = false;
    }

    void OnFireBall()
    {
        Instantiate(fireball, firevector.transform.position, firevector.transform.rotation);
        Invoke("Explosion", 0.95f);

    }



    void OnIdle()
    {
        photonView.RPC("EndFire", RpcTarget.AllViaServer, null);
        Invoke("attack_false", fire_delay);
    }

    void attack_false()
    {
        attacking = false;
    }


    [PunRPC]
    void EndFire()
    {
        animator.SetBool("IsAttack", false);

    }



    public void Explosion()
    {
        photonView.RPC("CreateExplosion", RpcTarget.AllViaServer, null);
    }

    [PunRPC]
    public void CreateExplosion()
    {
        fired_fireball = GameObject.Find("Magic fire pro orange(Clone)").gameObject;

        Instantiate(explosion, fired_fireball.transform.position, fired_fireball.transform.rotation);
    }

}
