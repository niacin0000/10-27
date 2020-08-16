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
    

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audiosource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (!photonView.IsMine) return;

        if (photonView.IsMine)
        {
            if (attacking == false && Input.GetMouseButtonDown(1))
            {
                photonView.RPC("Fire", RpcTarget.AllViaServer, null);
                attacking = true;
                if (player.GetComponent<MoveCtrl>().St_active)
                {
                    Invoke("Explosion", 0.95f);
                }

            }


        }
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
