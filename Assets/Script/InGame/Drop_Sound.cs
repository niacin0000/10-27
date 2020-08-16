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
            if (this.name == "Drop_Es" || this.name == "Drop_Sw")
            {
                audiosource.clip = audioClips[0];
                audiosource.outputAudioMixerGroup = audioMixerGroup;
                audiosource.Play();
                audiosource.loop = false;
            }
            else if (this.name == "Drop_Sh" || this.name == "Drop_St")
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
