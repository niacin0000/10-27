using System.Collections;
using System.Collections.Generic;
using Photon.Pun; // 유니티용 포톤 컴포넌트들
using Photon.Realtime; // 포톤 서비스 관련 라이브러리
//using System.Globalization;
//using System.Media;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Security.Cryptography;

public class GameMgr : MonoBehaviourPunCallbacks
{
    public Text msgList;
    public Text playerCount;

    // Start is called before the first frame update

    public GameObject Robo;
    public Collision collision;

    public GameObject sword;
    public GameObject espadon;
    public GameObject shield;
    public GameObject staff;

    private int currPlayer, maxPlayer;

    private bool createPlayer = true;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        // photonNetwork의 데이터 통신을 다시 연결시켜준다. 
        PhotonNetwork.IsMessageQueueRunning = true;
        Invoke("CheckPlayerCount", 0f);
        Invoke("Create", 0.5f);


    }


    void Create()
    {
        Debug.Log(currPlayer + "/.sadafdsaf");

        if (currPlayer == 1)
            CreatePlayer1();
        else if (currPlayer == 2)
            CreatePlayer2();
        else if (currPlayer == 3)
            CreatePlayer3();
        else if (currPlayer == 4)
            CreatePlayer4();
    }

    public void DropWeapon()
    {
        Transform[] points = GameObject.Find("SpawnWeaponsGroup").GetComponentsInChildren<Transform>();
        Instantiate(sword, points[0].position, Quaternion.identity);
        Instantiate(espadon, points[1].position, Quaternion.identity);
        Instantiate(shield, points[2].position, Quaternion.identity);
        Instantiate(staff, points[3].position, Quaternion.identity);
    }

    public void CreatePlayer1()
    {
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        PhotonNetwork.Instantiate("Player1", points[0].position, Quaternion.identity);
    }
    public void CreatePlayer2()
    {
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        PhotonNetwork.Instantiate("Player2", points[1].position, Quaternion.identity);
    }
    public void CreatePlayer3()
    {
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        PhotonNetwork.Instantiate("Player3", points[2].position, Quaternion.identity);
    }
    public void CreatePlayer4()
    {
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        //int idx = Random.Range(1, points.Length);
        PhotonNetwork.Instantiate("Player4", points[3].position, Quaternion.identity);
    }

    public void OnExitClick()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        Destroy(GameObject.Find("PCanvas"));

        Destroy(gameObject);

        SceneManager.LoadScene("Lobby");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        CheckPlayerCount();

        string msg = string.Format("[{0}]님이 입장", newPlayer.NickName);

        ReceiveMsg(msg);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        CheckPlayerCount();

        string msg = string.Format("[{0}]님이 퇴장"
                                    , otherPlayer.NickName);

        //photonView.RPC("ReceiveMsg", RpcTarget.Others, msg);
        ReceiveMsg(msg);
    }
    void CheckPlayerCount()
    {
        currPlayer = PhotonNetwork.PlayerList.Length;
        maxPlayer = PhotonNetwork.CurrentRoom.MaxPlayers;
        //playerCount.text = string.Format("[{0}/{1}]", currPlayer, maxPlayer);
    }
    void ReceiveMsg(string msg)
    {
        msgList.text += "\n" + msg;
    }
}
