using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPanel : MonoBehaviour
{
    public GameObject[] panels;

    public void OnTogglePalnelforCreate()
    {
        if (!panels[0].activeSelf)
        {
            panels[0].SetActive(true);
            panels[1].SetActive(false);
        }
        else
            return;
    }
    public void OnTogglePalnelforInfo()
    {
        if (!panels[1].activeSelf)
        {
            panels[1].SetActive(true);
            panels[0].SetActive(false);
        }
        else
            return;
    }

    public void OnToggleOff()
    {
        panels[0].SetActive(false);
        panels[1].SetActive(false);
    }

    public void OnWarning()
    {
        panels[2].SetActive(true);
    }



    public void Click_OK()
    {
        panels[2].SetActive(false);
    }

}
