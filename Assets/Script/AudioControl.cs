using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioControl : MonoBehaviour
{
    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    //소리
    public AudioMixer masterMixer;
    public Slider audioSlider;
    //Sound
    public void ToggleAudioVolune()
    {
        AudioListener.volume = AudioListener.volume == 0 ? 1 : 0;
    }
    public void OnAudioControl()
    {
        float sound = audioSlider.value;
        if (sound == -40f) masterMixer.SetFloat("Master", -80);
        else masterMixer.SetFloat("Master", sound);
    }
}
