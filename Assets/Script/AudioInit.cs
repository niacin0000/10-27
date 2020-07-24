using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioInit : MonoBehaviour
{
    //소리
    public AudioMixer masterMixer;
    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider effectSlider;

    public void Awake()
    {
        masterMixer = GetComponent<AudioControl>().masterMixer;
        masterSlider = GetComponent<AudioControl>().masterSlider;
        bgmSlider = GetComponent<AudioControl>().bgmSlider;
        effectSlider = GetComponent<AudioControl>().effectSlider;
    }

    //Sound
    public void ToggleAudioVolune()
    {
        AudioListener.volume = AudioListener.volume == 0 ? 1 : 0;
    }
    public void OnAudioControl()
    {
        float maser = masterSlider.value;
        if (maser == -40f) masterMixer.SetFloat("Master", -80);
        else masterMixer.SetFloat("Master", maser);

        float bgm = bgmSlider.value;
        if (bgm == -40f) masterMixer.SetFloat("BGM", -80);
        else masterMixer.SetFloat("BGM", bgm);

        float effect = effectSlider.value;
        if (effect == -40f) masterMixer.SetFloat("SFX", -80);
        else masterMixer.SetFloat("SFX", effect);
    }
}
