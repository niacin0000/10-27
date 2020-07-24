using System.Collections;
using System.Collections.Generic;
using Photon.Pun; // 유니티용 포톤 컴포넌트들
using Photon.Realtime; // 포톤 서비스 관련 라이브러리
//using System.Globalization;
//using System.Media;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameMgr : MonoBehaviourPunCallbacks
{
    public Text msgList;
    public Text playerCount;
    public GameObject Ch1;
    public GameObject Ch2;
    public GameObject Ch3;
    public GameObject Ch4;
    public GameObject menuSet;
    // Start is called before the first frame update

    public GameObject Robo;
    public Collision collision;

    private int a;

    void Awake()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        DontDestroyOnLoad(gameObject);

    }
    void Start()
    {
        //CreateCube();
        // photonNetwork의 데이터 통신을 다시 연결시켜준다. 
        PhotonNetwork.IsMessageQueueRunning = true;
        Invoke("CheckPlayerCount", 0.5f);
        GetComponent<Config>().Load();
    }

    private void Update()
    {
        // sub menu
        if (Input.GetButtonDown("Cancel"))
        {
            if (menuSet.activeSelf)
                menuSet.SetActive(false);
            else
                menuSet.SetActive(true);
        }
    }
    public void CreateCube1()
    {
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();

        int idx = Random.Range(1, points.Length);
        PhotonNetwork.Instantiate("Robo", points[idx].position, Quaternion.identity);



        a = 1;
        photonView.RPC("DestroyButton", RpcTarget.AllViaServer, a);

    }
    public void CreateCube2()
    {
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();

        int idx = Random.Range(1, points.Length);
        PhotonNetwork.Instantiate("Robo_D", points[idx].position, Quaternion.identity);

        //Ch2.SetActive(false);
        a = 2;
        photonView.RPC("DestroyButton", RpcTarget.AllViaServer, a);

        //DestroyButton();
    }
    public void CreateCube3()
    {
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();

        int idx = Random.Range(1, points.Length);
        PhotonNetwork.Instantiate("Robo_J", points[idx].position, Quaternion.identity);

        a = 3;
        photonView.RPC("DestroyButton", RpcTarget.AllViaServer, a);
        //Ch3.SetActive(false);
    }
    public void CreateCube4()
    {
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();

        int idx = Random.Range(1, points.Length);
        PhotonNetwork.Instantiate("Player", points[idx].position, Quaternion.identity);

        a = 4;
        photonView.RPC("DestroyButton", RpcTarget.AllViaServer, a);
        //Ch4.SetActive(false);
    }

    [PunRPC]
    void DestroyButton(int a)
    {
        switch (a)
        {
            case 1: Destroy(Ch1); break;
            case 2: Destroy(Ch2); break;
            case 3: Destroy(Ch3); break;
            case 4: Destroy(Ch4); break;

        }



    }

    public void OnExitClick()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        Destroy(GameObject.Find("PCanvas"));

        //Don'tDestroyOnLoad 에 있는 아이템 제거
        GameObject[] D_I = GameObject.FindGameObjectsWithTag("ITEMS");
        for (int i = 0; i < D_I.Length; i++)
            Destroy(D_I[i]);

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
        int currPlayer = PhotonNetwork.PlayerList.Length;
        int maxPlayer = PhotonNetwork.CurrentRoom.MaxPlayers;
        playerCount.text = string.Format("[{0}/{1}]", currPlayer, maxPlayer);
    }
    void ReceiveMsg(string msg)
    {
        msgList.text += "\n" + msg;
    }

    public void OnExitGame()
    {
        Debug.Log("ExitGame");
        Application.Quit();
    }
}
