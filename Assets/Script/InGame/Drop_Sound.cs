using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Drop_Sound : MonoBehaviour
{
    private bool one = true;
    public AudioClip[] audioClips;
    AudioSource audiosource;
    public AudioMixerGroup audioMixerGroup;

    public void Start()
    {
        audiosource = GetComponent<AudioSource>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("GROUND") && one)
        {
            if (this.name == "Drop_Es(Clone)" || this.name == "Drop_Sw(Clone)")
            {
                audiosource.clip = audioClips[0];
                audiosource.outputAudioMixerGroup = audioMixerGroup;
                audiosource.Play();
                audiosource.loop = false;
                Debug.Log("Drop" + this.name);
            }
            else if (this.name == "Drop_Sh(Clone)" || this.name == "Drop_St(Clone)")
            {
                audiosource.clip = audioClips[1];
                audiosource.outputAudioMixerGroup = audioMixerGroup;
                audiosource.Play();
                audiosource.loop = false;
            }
            one = false;
        }
    }
}
