using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenSize : MonoBehaviour
{
    public Toggle IsFullScreen;
    public int Checking;

    const int FULLSCREEN_TRUE = 1;
    const int FULLSCREEN_FALSE = 0;

    public void Save()
    {
        if(IsFullScreen.isOn)
        {
            Checking = FULLSCREEN_TRUE;
            Debug.Log("isOn");
        }
        else
        {
            Checking = FULLSCREEN_FALSE;

            Debug.Log("isOff");
        }
        PlayerPrefs.SetInt("FullScreen", Checking);
    }

    public void Load()
    {
        Checking = PlayerPrefs.GetInt("FullScreen");
        if (Checking == 1)
        {
            IsFullScreen.SetIsOnWithoutNotify(true);
        }
        else
        {
            IsFullScreen.SetIsOnWithoutNotify(false);
        }
    }
}
