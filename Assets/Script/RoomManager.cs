using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public Button StartButton;

    private void Start()
    {
        print(1);
        CheckNick();
    }
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

    void CheckNick()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length - 1; i++)
        {
            if (PhotonNetwork.PlayerList[i].NickName == PhotonNetwork.NickName)
            {
                //PhotonNetwork.NickName = txtUserId.text + "("+i+")";
                LeaveRoom();
            }
            else
                print(PhotonNetwork.PlayerList[i].NickName);
        }
    }

}
