using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomData : MonoBehaviour
{

    public string roomName = "";
    public int playerCount = 0;
    public int maxPlayer = 0;

    public Text txtplayerCount;
    public Text txtroomName;
    public Text[] txtplayerList;

    [System.NonSerialized]
    public Text roomDataTxt;

    void Awake()
    {
        roomDataTxt = GetComponentInChildren<Text>();
    }

    public void UpdateInfo()
    {
        roomDataTxt.text = string.Format(" {0} [{1}/{2}]"
                                        , roomName
                                        , playerCount.ToString("00")
                                        , maxPlayer);

        txtplayerCount.text = string.Format("{0}/{1}", playerCount, maxPlayer);
        txtroomName.text = string.Format("방이름 : {0}", roomName);
    }
}
