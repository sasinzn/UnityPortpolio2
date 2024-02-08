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
    private TextMeshProUGUI connectText;    //���� �ؽ�Ʈ
    [SerializeField]
    private Button joinButton;              //���� ��ư
    [SerializeField]
    private Button titleButton;             //Ÿ��Ʋ �̵� ��ư
    private GameObject joinUI;              //UIPanel
    private RoomOptions roomOptions;

    private void Awake()
    {
        joinUI = GameObject.Find("JoinUI");
        connectText = GetChildObj(joinUI.transform, "JoinMessage").GetComponent<TextMeshProUGUI>();
        //���� ��ư ����
        joinButton = GetChildObj(joinUI.transform, "Button_Join").GetComponent<Button>();
        joinButton.onClick.AddListener(ConnectToRoom);
        //Ÿ��Ʋ ��ư ����
        titleButton = GetChildObj(joinUI.transform, "Button_Title").GetComponent<Button>();
        titleButton.onClick.AddListener(MoveToTitle);
        roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
    }

    private void Start()
    {
        //���� ���� �� ���õ� ������ ���� �õ�
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
        //���� ��ư ��Ȱ��ȭ
        joinButton.interactable = false;
        //���ӻ�Ȳ ǥ��
        connectText.text = "������ ������ �������Դϴ�...";
        Debug.Log(connectText.text);
    }

    //���� ���� ������ ����
    public override void OnConnectedToMaster()
    {
        //���� ��ư Ȱ��ȭ
        joinButton.interactable = true;
        //���ӻ�Ȳ ǥ��
        connectText.text = "�¶��� : ������ ���� �����";
        PhotonNetwork.LocalPlayer.NickName = NetworkManager.instance.nickName;
        Debug.Log(connectText.text);
    }

    //���� ���� ���н� ����
    public override void OnDisconnected(DisconnectCause cause)
    {
        // ���� ��ư ��Ȱ��ȭ
        joinButton.interactable = false;
        //���ӻ�Ȳ ǥ��
        connectText.text = "�������� : ������ ���� ������� \n ������ �õ���...";
        Debug.Log(connectText.text);
        //�����ͼ��� ������ �õ�
        PhotonNetwork.ConnectUsingSettings();
    }
    //Ÿ��Ʋ �̵�
    public void MoveToTitle()
    {
        SceneManager.LoadScene(0);
        PhotonNetwork.Disconnect();
    }


    //������
    public void ConnectToRoom()
    {
        // �ߺ� ���� ������ ���� ���� ��ư ��Ȱ��ȭ
        joinButton.interactable = false;
        if(PhotonNetwork.IsConnected)
        {
            //�� ���� ����
            connectText.text = "�� ���� �õ���...";
            Debug.Log(connectText.text);
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            //������ ���� �������� �ƴҽ�
            // ���� ��ư ��Ȱ��ȭ
            joinButton.interactable = false;
            //���ӻ�Ȳ ǥ��
            connectText.text = "�������� : ������ ���� ������� \n ������ �õ���...";
            Debug.Log(connectText.text);
            //�����ͼ��� ������ �õ�
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    //����� ���н�
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        connectText.text = "������� ���� �Ͽ����ϴ�.";
        Debug.Log(connectText.text);
        Debug.Log(message);
    }

    //����� ���� ���� ���н� ����
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //���ӻ�Ȳ ǥ��
        connectText.text = "���������� ���� �����ϴ�. \n ���ο� �� ������...";
        Debug.Log(connectText.text);
        int roomName = Random.Range(0, 1000);
        PhotonNetwork.CreateRoom(roomName.ToString(),roomOptions,null);
    }

    //���� �Ϸ�� ����
    public override void OnJoinedRoom()
    {
        //�� �������̰� ���� �������� �÷��̾ 2��(�ִ��ο�)�϶�
        if(PhotonNetwork.InRoom && PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            //���ӻ�Ȳ ǥ��
            connectText.text = "������ ����\n������ �����մϴ�.";
            Debug.Log(connectText.text);
            //��� �����ڵ��� MainScene�� ȣ��
            PhotonNetwork.LoadLevel("MainScene");
        }
        else if(PhotonNetwork.InRoom && PhotonNetwork.CurrentRoom.PlayerCount < 2)
        {
            //���ӻ�Ȳ ǥ��
            connectText.text = "������ ����\n�ٸ� �÷��̾ ��ٸ����� �Դϴ�.";
            Debug.Log(connectText.text);
            
        }
    }
    //���� �÷��̾ ���԰� �ο����� �ƽ��ϋ�
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.InRoom && PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            //���ӻ�Ȳ ǥ��
            connectText.text = "��� �÷��̾� ���� �Ϸ� ������ �����մϴ�.";
            Debug.Log(connectText.text);
            //��� �����ڵ��� MainScene�� ȣ��
            PhotonNetwork.LoadLevel("MainScene");
        }
    }

    //UI ���� �ڽ�child �˻�
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
