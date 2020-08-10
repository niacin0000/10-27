using Photon.Pun; // 유니티용 포톤 컴포넌트들
using Photon.Realtime; // 포톤 서비스 관련 라이브러리
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



// 마스터(매치 메이킹) 서버와 룸 접속을 담당
public class LobbyManager : MonoBehaviourPunCallbacks
{
    public enum ActivePanel
    {
        TITLE = 0,
        ROOMS = 1,
        OPTION = 2,
        SOLO = 3,
        TEAM = 4,
    }
    private string gameVersion = "1"; // 게임 버전
    public string userId = "YouRang";
    public byte maxPlayer = 20;

    public Text connectionInfoText; // 네트워크 정보를 표시할 텍스트
    public Button joinButton; // 게임로비 접속 버튼
    public Button StartButton; // 룸 접속 버튼

    public GameObject[] panels;
    public Dropdown teamSelect;

    public InputField txtUserId;
    public InputField txtRoomName;

    public Text txtplayerCount;
    public Text txtroomName;
    public Text[] txtplayerList;

    public Toggle isPassword;
    public InputField txtRoomPassword;

    public GameObject room;
    public Transform gridTr;

    public bool fullScreen = true;
    public int screenSize_x = 1920;
    public int screenSize_y = 1080;

    private void Awake()
    {
        // photon1과 photon2로 바뀌면서 달라진점 (같은방 동기화)
        PhotonNetwork.AutomaticallySyncScene = true;
        GetComponent<ScreenSize>().Load();
        if (GetComponent<ScreenSize>().Checking == 1)
        {
            fullScreen = true;
        }
        else
        {
            fullScreen = false;
        }
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Screen.SetResolution(screenSize_x, screenSize_y, fullScreen);
    }
    // 게임 실행과 동시에 마스터 서버 접속 시도
    private void Start()
    {
        // 접속에 필요한 정보(게임 버전) 설정
        PhotonNetwork.GameVersion = gameVersion;
        // 설정한 정보를 가지고 마스터 서버 접속 시도
        PhotonNetwork.ConnectUsingSettings();

        txtUserId.text = PlayerPrefs.GetString("USER_ID", "USER_Chan");
        txtRoomName.text = PlayerPrefs.GetString("ROOM_NAME", "ROOM_Chan");
        // 룸 접속 버튼을 잠시 비활성화
        //joinButton.interactable = false;
        //// 접속을 시도 중임을 텍스트로 표시
        connectionInfoText.text = "마스터 서버에 접속중...";

        GetComponent<Config>().Load();
    }

    private void Update()
    {
        if (GetComponent<RoomPanel>().panels[0].activeSelf == true)
        {
            OnPasswordCheck();
        }

        //for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        //{
        //    Debug.Log(PhotonNetwork.PlayerList[i]);
        //}

        if (PhotonNetwork.IsMasterClient)
        {
            StartButton.interactable = true;
        }
        else
        {
            StartButton.interactable = false;
        }

    }

    private void ChangePanel(ActivePanel panel)
    {
        foreach (GameObject _panel in panels)
        {
            Debug.Log(panels);
            _panel.SetActive(false);
        }
        panels[(int)panel].SetActive(true);
    }

    // 마스터 서버 접속 성공시 자동 실행
    public override void OnConnectedToMaster()
    {
        // 룸 접속 버튼을 활성화
        //joinButton.interactable = true;
        // 접속 정보 표시
        connectionInfoText.text = "온라인 : 마스터 서버와 연결됨";
    }

    // 마스터 서버 접속 실패시 자동 실행
    public override void OnDisconnected(DisconnectCause cause)
    {
        // 룸 접속 버튼을 비활성화
        //joinButton.interactable = false;
        // 접속 정보 표시
        //connectionInfoText.text = "오프라인 : 마스터 서버와 연결되지 않음\n접속 재시도 중...";

        // 마스터 서버로의 재접속 시도
        PhotonNetwork.ConnectUsingSettings();
    }

    // 룸 접속 시도
    public void Connect()
    {
        PhotonNetwork.JoinLobby();
        // 중복 접속 시도를 막기 위해, 접속 버튼 잠시 비활성화
        //joinButton.interactable = false;

        // 마스터 서버에 접속중이라면
        if (PhotonNetwork.IsConnected)
        {
            // 룸 접속 실행
            //connectionInfoText.text = "룸에 접속...";
            //PhotonNetwork.JoinRandomRoom();

            PhotonNetwork.ConnectUsingSettings();

            PlayerPrefs.SetString("USER_ID", PhotonNetwork.NickName);
            ChangePanel(ActivePanel.ROOMS);
        }
        else
        {
            // 마스터 서버에 접속중이 아니라면, 마스터 서버에 접속 시도
            connectionInfoText.text = "오프라인 : 마스터 서버와 연결되지 않음\n접속 재시도 중...";
            // 마스터 서버로의 재접속 시도
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    public void OnCreateRoomClick()
    {
        if (!PhotonNetwork.IsConnected)
            return;

            PhotonNetwork.CreateRoom(teamSelect.options[teamSelect.value].text + "/" + txtRoomName.text + "_" + txtRoomPassword.text
                        , new RoomOptions { MaxPlayers = this.maxPlayer }, TypedLobby.Default);
    }

    //패스워드관련
    public void OnPasswordCheck()
    {
        if (isPassword.isOn)
        {
            txtRoomPassword.interactable = true;
        }
        else
        {
            txtRoomPassword.interactable = false;
            txtRoomPassword.text = null;
        }
    }


    // 룸에 참가 완료된 경우 자동 실행
    public override void OnJoinedRoom()
    {
        PhotonNetwork.GameVersion = this.gameVersion;
        //PhotonNetwork.PlayerList[0].NickName == PhotonNetwork.NickName)


        if(PhotonNetwork.PlayerList.Length == 1)
        {
            PhotonNetwork.NickName = txtUserId.text;
        }
        for (int i = 1; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (PhotonNetwork.PlayerList[i].NickName == txtUserId.text)
            {
                PhotonNetwork.NickName = txtUserId.text + "(Clone)";
            }
        }

        // 접속 상태 표시
        connectionInfoText.text = "방 참가 성공";
        GetComponent<RoomPanel>().OnToggleOff();
        // 모든 룸 참가자들이 Main 씬을 로드하게 함
        PhotonNetwork.IsMessageQueueRunning = false;


        if (teamSelect.value == 0)
        {
            ChangePanel(ActivePanel.SOLO);
            PhotonNetwork.IsMessageQueueRunning = true;

            var value = PhotonNetwork.PlayerList.GetValue(0);
            Debug.Log("벨류"+value);
        }
        //else if(teamSelect.value == 0)
        //{
        //    ChangePanel(ActivePanel.TEAM);
        //    PhotonNetwork.IsMessageQueueRunning = true;
        //}
        else //방 참가시
            PhotonNetwork.IsMessageQueueRunning = true;


    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("ROOM"))
        {
            Destroy(obj);
        }
        foreach (RoomInfo roomInfo in roomList)
        {
            GameObject _room = Instantiate(room, gridTr);
            RoomData roomData = _room.GetComponent<RoomData>();
            roomData.roomName = roomInfo.Name;
            roomData.maxPlayer = roomInfo.MaxPlayers;
            roomData.playerCount = roomInfo.PlayerCount;
            roomData.UpdateInfo();
            roomData.GetComponent<Button>().onClick.AddListener
            (
                delegate
                {
                    //OnClickRoom(roomData);
                    OnJoinUpdate(roomData);
                }
            );
        }
    }
    
    private void OnJoinUpdate(RoomData roomdata)
    {
        txtplayerCount.text = string.Format("{0}/{1}", roomdata.playerCount, roomdata.maxPlayer);
        txtroomName.text = string.Format("방이름 : {0}", roomdata.roomName);
        joinButton.onClick.AddListener
            (
            delegate
            {
                OnClickRoom(roomdata);
            }
            );
    }

    void OnClickRoom(RoomData roomdata)
    {
        PhotonNetwork.IsMessageQueueRunning = false;

        PhotonNetwork.NickName = txtUserId.text;

        PlayerPrefs.SetString("USER_ID", PhotonNetwork.NickName);

        PhotonNetwork.IsMessageQueueRunning = true;
        PhotonNetwork.JoinRoom(roomdata.roomName, null);

        GetComponent<RoomPanel>().OnToggleOff();
        string[] check = roomdata.roomName.Split(new char[] { '/' });
        if (check[0] == "팀전")
        {
            ChangePanel(ActivePanel.TEAM);
        }

        else if (check[0] == "개인전")
        {
            ChangePanel(ActivePanel.SOLO);
        }
        else
            return;

    }

    public void OnEnterOption()
    {
        ChangePanel(ActivePanel.OPTION);
    }
    public void OnReturnTitle()
    {
        GetComponent<RoomPanel>().OnToggleOff();
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
        ChangePanel(ActivePanel.TITLE);
    }

    public void OnLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        ChangePanel(ActivePanel.ROOMS);
    }

    public void OnExitGame()
    {
        Debug.Log("ExitGame");
        Application.Quit();
    }

    public void GameStart()
    {
        PhotonNetwork.LoadLevel("Map_01");
    }
}
