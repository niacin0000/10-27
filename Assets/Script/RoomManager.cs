using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public Button StartButton;
    private bool isLoading = true;



    public void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartButton.interactable = true;
        }
        else
        {
            StartButton.interactable = false;
        }

        if (isLoading)
        {
            if (PhotonNetwork.CurrentRoom == null)
            {
                return;
            }
            else
            {
                CheckNick();
                isLoading = false;
            }
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

    void CheckNick()
    {
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount-1; i++)
        {
            if (PhotonNetwork.PlayerList[i].NickName == PhotonNetwork.NickName)
            {
                LeaveRoom();
            }
            else
                print(PhotonNetwork.PlayerList[i].NickName);
        }
    }

}
