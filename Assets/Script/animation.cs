using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class animation : MonoBehaviour
{

    public Animator animator;
    public AudioClip[] audioClips;
    AudioSource audiosource;
    public AudioMixerGroup audioMixerGroup;
    bool Attack = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audiosource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Attack == false && Input.GetMouseButtonDown(1))
        {
            animator.SetBool("IsAttack", true);
            Attack = true;
            Debug.Log(Attack);
        }
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

    void OnIdle()
    {
        animator.SetBool("IsAttack", false);
        Attack = false;
        Debug.Log(Attack);
    }

}
