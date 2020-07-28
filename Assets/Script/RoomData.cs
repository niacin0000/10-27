using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomData : MonoBehaviourPunCallbacks
{

    public string roomName = "";
    public int playerCount = 0;
    public int maxPlayer = 0;

    [System.NonSerialized]
    public Text roomDataNametxt;
    public Text roomDataPCtxt;
    public Text rommDataPlayerNick;
    public string msg = "";

    void Awake()
    {
        roomDataNametxt = GetComponentInChildren<Text>();
        roomDataPCtxt = GetComponentInChildren<Text>();
    }

    public void UpdateInfo()
    {
        roomDataNametxt.text = string.Format("방 이름 : {0}", roomName);
        roomDataPCtxt.text = string.Format("[{0}/{1}]",playerCount.ToString("00"), maxPlayer);
        rommDataPlayerNick.text = msg;
    }
}
