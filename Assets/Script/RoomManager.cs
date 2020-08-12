﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        GetComponent<LobbyManager>().DoStart();
        PhotonNetwork.LoadLevel("Lobby");
    }
}
