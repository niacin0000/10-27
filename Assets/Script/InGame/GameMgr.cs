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

    public GameObject Menu;
    public GameObject Option;
    public GameObject MenuImage;
    public GameObject GameResult;

    public GameObject Win_text, Lose_text;

    private bool menuOn = false;



    public bool fullScreen = true;
    public int screenSize_x = 1920;
    public int screenSize_y = 1080;
    //private bool createPlayer = true;

    void Start()
    {
        GetComponent<Config>().Load();
        // photonNetwork의 데이터 통신을 다시 연결시켜준다. 
        PhotonNetwork.IsMessageQueueRunning = true;
        Invoke("CheckPlayerCount", 0f);
        Invoke("Create", 0.5f);
    }

    public void Update()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            Debug.Log(PhotonNetwork.PlayerList[i].ActorNumber);
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(menuOn == false)
            {
                Menu.SetActive(true);
                menuOn = true;
            }
            else
            {
                Menu.SetActive(false);
                Option.SetActive(false);
                MenuImage.SetActive(true);
                GetComponent<Config>().Save();
                GetComponent<ScreenSize>().Save();
                menuOn = false;
            }
        }
    }

    void Awake()
    {
        DropWeapon();
    }



    void Create()
    {
        if (PhotonNetwork.PlayerList[0].NickName == PhotonNetwork.NickName)
            CreatePlayer1();
        else if (PhotonNetwork.PlayerList[1].NickName == PhotonNetwork.NickName)
            CreatePlayer2();
        else if (PhotonNetwork.PlayerList[2].NickName == PhotonNetwork.NickName)
            CreatePlayer3();
        else if (PhotonNetwork.PlayerList[3].NickName == PhotonNetwork.NickName)
            CreatePlayer4();
    }

    public void DropWeapon()
    {
        Transform[] points = GameObject.Find("SpawnWeaponsGroup").GetComponentsInChildren<Transform>();
        Instantiate(sword, points[1].position, Quaternion.identity);
        Instantiate(espadon, points[2].position, Quaternion.identity);
        Instantiate(shield, points[3].position, Quaternion.identity);
        Instantiate(staff, points[4].position, Quaternion.identity);

        GameObject.FindGameObjectWithTag("SHIELD").transform.rotation = Quaternion.Euler(-90, -90, -90);
    }

    public void CreatePlayer1()
    {
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        PhotonNetwork.Instantiate("Player1", points[1].position, Quaternion.identity);
    }
    public void CreatePlayer2()
    {
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        PhotonNetwork.Instantiate("Player2", points[2].position, Quaternion.identity);
    }
    public void CreatePlayer3()
    {
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        PhotonNetwork.Instantiate("Player3", points[3].position, Quaternion.identity);
    }
    public void CreatePlayer4()
    {
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        //int idx = Random.Range(1, points.Length);
        PhotonNetwork.Instantiate("Player4", points[4].position, Quaternion.identity);
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

        //ReceiveMsg(msg);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        CheckPlayerCount();


        

        string msg = string.Format("[{0}]님이 퇴장"
                                    , otherPlayer.NickName);

        //photonView.RPC("ReceiveMsg", RpcTarget.Others, msg);
        //ReceiveMsg(msg);
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

    public void OpenOption()
    {
        MenuImage.SetActive(false);
        Option.SetActive(true);
        GetComponent<ScreenSize>().Load();
    }

    public void ReturnMenu()
    {
        GetComponent<ScreenSize>().Save();
        if (GetComponent<ScreenSize>().Checking == 1)
        {
            fullScreen = true;
        }
        else
        {
            fullScreen = false;
        }
        Screen.SetResolution(screenSize_x, screenSize_y, fullScreen);
        Option.SetActive(false);
        MenuImage.SetActive(true);
    }

    public void ReturnGame()
    {
        Menu.SetActive(false);
        menuOn = false;
    }




    public void Win_panel()
    {

        GameResult.SetActive(true);
        Win_text.SetActive(true);
        Lose_text.SetActive(false);

    }

    public void Lose_panel()
    {

        GameResult.SetActive(true);
        Win_text.SetActive(false);
        Lose_text.SetActive(true);

    }




}
