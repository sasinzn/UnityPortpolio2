using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private string gameVersion = "1";       //gameversion
    private TextMeshProUGUI connectText;    //접속 텍스트
    [SerializeField]
    private Button joinButton;              //접속 버튼
    [SerializeField]
    private Button titleButton;             //타이틀 이동 버튼
    private GameObject joinUI;              //UIPanel
    private RoomOptions roomOptions;

    private void Awake()
    {
        joinUI = GameObject.Find("JoinUI");
        connectText = GetChildObj(joinUI.transform, "JoinMessage").GetComponent<TextMeshProUGUI>();
        //조인 버튼 세팅
        joinButton = GetChildObj(joinUI.transform, "Button_Join").GetComponent<Button>();
        joinButton.onClick.AddListener(ConnectToRoom);
        //타이틀 버튼 세팅
        titleButton = GetChildObj(joinUI.transform, "Button_Title").GetComponent<Button>();
        titleButton.onClick.AddListener(MoveToTitle);
        roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
    }

    private void Start()
    {
        //버전 세팅 및 세팅된 정보로 접속 시도
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
        //조인 버튼 비활성화
        joinButton.interactable = false;
        //접속상황 표시
        connectText.text = "마스터 서버에 접속중입니다...";
        Debug.Log(connectText.text);
    }

    //서버 접속 성공시 실행
    public override void OnConnectedToMaster()
    {
        //조인 버튼 활성화
        joinButton.interactable = true;
        //접속상황 표시
        connectText.text = "온라인 : 마스터 서버 연결됨";
        PhotonNetwork.LocalPlayer.NickName = NetworkManager.instance.nickName;
        Debug.Log(connectText.text);
    }

    //서버 접속 실패시 실행
    public override void OnDisconnected(DisconnectCause cause)
    {
        // 조인 버튼 비활성화
        joinButton.interactable = false;
        //접속상황 표시
        connectText.text = "오프라인 : 마스터 서버 연결실패 \n 재접속 시도중...";
        Debug.Log(connectText.text);
        //마스터서버 재접속 시도
        PhotonNetwork.ConnectUsingSettings();
    }
    //타이틀 이동
    public void MoveToTitle()
    {
        SceneManager.LoadScene(0);
        PhotonNetwork.Disconnect();
    }


    //방참가
    public void ConnectToRoom()
    {
        // 중복 접속 방지를 위해 조인 버튼 비활성화
        joinButton.interactable = false;
        if(PhotonNetwork.IsConnected)
        {
            //룸 접속 실행
            connectText.text = "방 접속 시도중...";
            Debug.Log(connectText.text);
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            //마스터 서버 접속중이 아닐시
            // 조인 버튼 비활성화
            joinButton.interactable = false;
            //접속상황 표시
            connectText.text = "오프라인 : 마스터 서버 연결실패 \n 재접속 시도중...";
            Debug.Log(connectText.text);
            //마스터서버 재접속 시도
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    //방생성 실패시
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        connectText.text = "방생성에 실패 하였습니다.";
        Debug.Log(connectText.text);
        Debug.Log(message);
    }

    //빈방이 없어 참가 실패시 실행
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //접속상황 표시
        connectText.text = "참여가능한 방이 없습니다. \n 새로운 방 생성중...";
        Debug.Log(connectText.text);
        int roomName = Random.Range(0, 1000);
        PhotonNetwork.CreateRoom(roomName.ToString(),roomOptions,null);
    }

    //참가 완료시 실행
    public override void OnJoinedRoom()
    {
        //방 접속중이고 현재 접속중인 플레이어가 2명(최대인원)일때
        if(PhotonNetwork.InRoom && PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            //접속상황 표시
            connectText.text = "방참가 성공\n게임을 시작합니다.";
            Debug.Log(connectText.text);
            //모든 참가자들이 MainScene을 호출
            PhotonNetwork.LoadLevel("MainScene");
        }
        else if(PhotonNetwork.InRoom && PhotonNetwork.CurrentRoom.PlayerCount < 2)
        {
            //접속상황 표시
            connectText.text = "방참가 성공\n다른 플레이어를 기다리는중 입니다.";
            Debug.Log(connectText.text);
            
        }
    }
    //새로 플레이어가 들어왔고 인원수가 맥스일떄
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.InRoom && PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            //접속상황 표시
            connectText.text = "모든 플레이어 참가 완료 게임을 시작합니다.";
            Debug.Log(connectText.text);
            //모든 참가자들이 MainScene을 호출
            PhotonNetwork.LoadLevel("MainScene");
        }
    }

    //UI 하위 자식child 검색
    private GameObject GetChildObj(Transform modelTr, string strName)
    {
        Transform[] AllData = modelTr.GetComponentsInChildren<Transform>();

        foreach (Transform trans in AllData)
        {
            if (trans.name == strName)
            {
                return trans.gameObject;
            }
        }
        return null;
    }

    
}
