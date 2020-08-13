using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public Button StartButton;

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartButton.interactable = true;
        }
        else
        {
            StartButton.interactable = false;
        }
    }
    public void LeaveRoom()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadSceneAsync("Lobby");
    }


    public void GameStart()
    {
        PhotonNetwork.LoadLevel("Map_01");
    }
}
