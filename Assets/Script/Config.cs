using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Config : MonoBehaviour
{
    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider effectSlider;

    public void Save()
    {
        PlayerPrefs.SetFloat("Master", masterSlider.value);
        PlayerPrefs.SetFloat("BGM", bgmSlider.value);
        PlayerPrefs.SetFloat("SFX", effectSlider.value);
    }

    public void Load()
    {
        masterSlider.value = PlayerPrefs.GetFloat("Master");
        bgmSlider.value = PlayerPrefs.GetFloat("BGM");
        effectSlider.value = PlayerPrefs.GetFloat("SFX");
    }
}
