using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioControl : MonoBehaviour
{
    //소리
    public AudioMixer masterMixer;
    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider effectSlider;

    public float master;
    public float bgm;
    public float effect;

    //Sound
    public void ToggleAudioVolune()
    {
        AudioListener.volume = AudioListener.volume == 0 ? 1 : 0;
    }

    public void OnAudioControl()
    {
        if (masterSlider.value == -40) masterMixer.SetFloat("Master", -80);
        else masterMixer.SetFloat("Master", masterSlider.value);

        if (bgmSlider.value == -40) masterMixer.SetFloat("BGM", -80);
        else masterMixer.SetFloat("BGM", bgmSlider.value);

        if (effectSlider.value == -40) masterMixer.SetFloat("SFX", -80);
        else masterMixer.SetFloat("SFX", effectSlider.value);
    }
}
