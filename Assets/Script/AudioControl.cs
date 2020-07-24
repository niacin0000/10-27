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

    public void Start()
    {
        masterSlider.value = 10f;
        bgmSlider.value = 10f;
        effectSlider.value = 10f;
    }
    //Sound
    public void ToggleAudioVolune()
    {
        AudioListener.volume = AudioListener.volume == 0 ? 1 : 0;
    }

    public void OnAudioControl()
    {
        float master = masterSlider.value;
        if (master == 0) masterMixer.SetFloat("Master", -80);
        else masterMixer.SetFloat("Master", master - 95);

        float bgm = bgmSlider.value;
        if (bgm == 0) masterMixer.SetFloat("BGM", -80);
        else masterMixer.SetFloat("BGM", bgm-95);

        float effect = effectSlider.value;
        if (effect == 0) masterMixer.SetFloat("SFX", -80);
        else masterMixer.SetFloat("SFX", effect-95);
    }
}
