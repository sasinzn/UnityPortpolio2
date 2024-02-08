using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;
    public Camera mainCam;

    public bool isPlayGame = true;
    

    private GameObject _player;
    private List<GameObject> _networkPlayers;
    private GameObject _networkPlayer;
    public GameObject player {  get { return PhotonNetwork.IsConnected ? _networkPlayer : _player; } }

    private Vector2 _screenLeftBottom;
    public Vector2 screenLeftBottom
    {
        get { return _screenLeftBottom; }
    }

    private Vector2 _screenRightTop;
    public Vector2 screenRightTop
    {
        get { return _screenRightTop; }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        instance = this;
        _networkPlayers = new List<GameObject>();

        GameObject playerPrefab = Resources.Load<GameObject>("Prefabs/Player");
        _player = Instantiate(playerPrefab) as GameObject;


        Vector2 screenSize = new Vector2(Screen.width, Screen.height);

        
        _screenLeftBottom = mainCam.ScreenToWorldPoint(Vector2.zero);
        _screenRightTop = mainCam.ScreenToWorldPoint(screenSize);

        BulletManager.instance.CreateBullets("Player",
            "Prefabs/BaseBullet", 100);
    }

    private void Start()
    {
        SoundManager.instance.PlayBGM(SoundKey.LOBBY_BGM);
        if(PhotonNetwork.IsConnected)
        {
            _player.SetActive(false);
            _networkPlayer = PhotonNetwork.Instantiate("Prefabs/Player", Vector3.zero, Quaternion.identity);
            _networkPlayers.Add(_networkPlayer);
        }
        
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SoundManager.instance.PlayBGM(SoundKey.LOBBY_BGM);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SoundManager.instance.PlayBGM(SoundKey.PLAY_BGM);
        }
    }

    public void GoTitle()
    {
        SceneManager.LoadScene(0);
        SoundManager.instance.PlayBGM(SoundKey.LOBBY_BGM);
    }

    public void GoLogin()
    {
        SceneManager.LoadScene(1);
    }

    public void GoMain()
    {
        SceneManager.LoadScene(2);
        SoundManager.instance.PlayBGM(SoundKey.PLAY_BGM);
    }

    public void ClosestPlayerSearch()
    {
        float minPlayersPosX = 999.0f;
        float maxPlayersPosX = -999.0f;
        float minPlayersPosY = 999.0f;
        float maxPlayersPosY = -999.0f;

        foreach(var player in _networkPlayers)
        {
            //플레이어들의 위치값을 기준으로 플레이어들의 범위 확인
            if(player.transform.position.x < minPlayersPosX)
                minPlayersPosX = player.transform.position.x;
            if(player.transform.position.x > maxPlayersPosX)
                maxPlayersPosX = player.transform.position.x;
            if(player.transform.position.y < minPlayersPosY)
                minPlayersPosY = player.transform.position.y;
            if(player.transform.position.y > maxPlayersPosY)
                maxPlayersPosY = player.transform.position.y;

        }
    }
}
