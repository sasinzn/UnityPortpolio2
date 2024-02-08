using Photon.Pun;
using Photon.Realtime;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    private enum State
    {
        LOGIN,
        CREATEACCOUNT,
        ERROR
    };
    public static NetworkManager instance;
    public TMP_InputField idInput;
    public TMP_InputField pwInput;
    public TMP_InputField pw2Input;
    public TMP_InputField inputNickName;
    public TextMeshProUGUI showNickName;
    public TextMeshProUGUI topMessage;
    public TextMeshProUGUI bottomMessage;
    public Text errorText;
    public bool rankingLoad = false;
    public string nickName;

    [SerializeField]
    private GameObject ui;
    [SerializeField]
    private string userId;
    [SerializeField]
    private string userPassword;
    [SerializeField]
    private string correctPassword;
    [SerializeField] 
    private string userNickname;
    [SerializeField]
    private State state;
    private State curState;

    private GameObject loginPenel;
    private GameObject createPenel;
    private GameObject errorPenel;

    private static string playerPrefNameKey = "PlayerName";
    private string _playFabPlayerIdCache;

    private void Awake()
    {
        instance = this;
        state = State.LOGIN;
        PlayFabSettings.TitleId = "6A64E";
        // 시크릿키 = 8RRA87PGWY3F7MUP3S8BS3KZNPFGARK5A18C1YGERS5FUXS46B
        ui = GameObject.Find("LoginUI");
        DontDestroyOnLoad(gameObject);

        loginPenel = GetChildObj(ui.transform, "PanelLogin");
        createPenel = GetChildObj(ui.transform, "PanelCreateAccount");
        errorPenel = GetChildObj(ui.transform, "PanelError");
        topMessage = GetChildObj(ui.transform, "TopMessage").GetComponent<TextMeshProUGUI>();
        bottomMessage = GetChildObj(ui.transform, "BottomMessage").GetComponent<TextMeshProUGUI>();
        createPenel.SetActive(false);
        errorPenel.SetActive(false);
    }

    private void ChangeState(State state)
    {
        if(state == State.CREATEACCOUNT)
        {
            ResetText();
            loginPenel.SetActive(false);
            createPenel.SetActive(true);
            this.state = state;
        }
        if(state == State.LOGIN)
        {
            ResetText();
            createPenel.SetActive(false);
            loginPenel.SetActive(true);
            this.state = state;
        }
        if(state == State.ERROR)
        {
            errorPenel.SetActive(true);
            curState = this.state;
            this.state = state;
        }
    }

    private void ResetText()
    {
        if (idInput != null) idInput.text = "";
        if (pwInput != null) pwInput.text = "";
        if (pw2Input != null) pw2Input.text = "";
        if (inputNickName != null) inputNickName.text = "";
    }

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

    public void IDValueChanged()
    {
        if(state == State.LOGIN)
        {
            idInput = GetChildObj(ui.transform, "InputField_ID").GetComponent<TMP_InputField>();
            userId = idInput.text.ToString();
            //userId = userId.Remove(userId.Length - 1);
        }

        if (state == State.CREATEACCOUNT)
        {
            idInput = GetChildObj(ui.transform, "ID_Input").GetComponent<TMP_InputField>();
            userId = idInput.text.ToString();
            //userId = userId.Remove(userId.Length - 1);
        }
    }

    public void PWValueChanged()
    {
        if (state == State.LOGIN)
        {
            pwInput = GetChildObj(ui.transform, "InputField_PW").GetComponent<TMP_InputField>();
            userPassword = pwInput.text.ToString();
            //userPassword = userPassword.Remove(userPassword.Length - 1);
        }

        if (state == State.CREATEACCOUNT)
        {
            pwInput = GetChildObj(ui.transform, "PW_Input").GetComponent<TMP_InputField>();
            userPassword = pwInput.text.ToString();
            //userPassword = userPassword.Remove(userPassword.Length - 1);
        }
    }

    public void PW2ValueChanged()
    {
        if (state == State.CREATEACCOUNT)
        {
            pw2Input = GetChildObj(ui.transform, "PW_Input2").GetComponent<TMP_InputField>();
            correctPassword = pw2Input.text.ToString();
            //userPassword2 = userPassword2.Remove(userPassword2.Length - 1);
        }
    }

    public void NickNameValueChanged()
    {
        if (state == State.CREATEACCOUNT)
        {
            inputNickName = GetChildObj(ui.transform, "Nick_input").GetComponent<TMP_InputField>();
            userNickname = inputNickName.text.ToString();
            //userNickname = userNickname.Remove(userNickname.Length - 1);
        }
    }


    public void Login()
    {
        //서버로 ID 와 PW 정보를 보내 일치하는 유저가 있는지 요청
        var request = new LoginWithPlayFabRequest { Username = userId, Password = userPassword };
        //로그인 성공시 LoginSuccess로 실패시 LoginFailure로 request 전달
        PlayFabClientAPI.LoginWithPlayFab(request, LoginSuccess, LoginFailure);
    }

    public void Register()
    {
        //입력받은 패스워드와 패스워드 확인이 일치하는지 확인
        if(userPassword == correctPassword)
        {
            //ID와 PW, 닉네임 순서로 서버로 계정생성 요청, 이메일은생략
            var request = new RegisterPlayFabUserRequest { Username = userId, Password = userPassword, 
                DisplayName = userNickname, RequireBothUsernameAndEmail = false };
            //가입성공시 RegisterSuccess로 실패시 RegisterFailure로 result전달
            PlayFabClientAPI.RegisterPlayFabUser(request, RegisterSuccess, RegisterFailure);
            topMessage.text = "가입성공";
            bottomMessage.text = "회원가입이 완료되었습니다.";
            ChangeState(State.ERROR);
        }
        else
        {
            topMessage.text = "오류";
            bottomMessage.text = "패스워드가 서로 일치하지 않습니다.";
            ChangeState(State.ERROR);
        }
       
    }

    private void LoginSuccess(LoginResult result)
    {
        Debug.Log("로그인 성공");
        //SceneManager.LoadScene("Launcher");

        // 계정정보 받아옴
        var request = new GetAccountInfoRequest { Username = userId };
        // 계정정보 요청 성공시 GetAcountSuccess로 실패시 GetAccountFailure로 result전달
        PlayFabClientAPI.GetAccountInfo(request, GetAccountSuccess, GetAccountFailure);
        RequestPhotonToken(result);

        // 로비화면으로
        SceneManager.LoadScene(3);
    }

    private void LoginFailure(PlayFabError error)
    {
        Debug.LogWarning("로그인 실패");
        Debug.LogWarning(error.GenerateErrorReport());
        errorText.text = error.GenerateErrorReport();
    }

    private void RegisterSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("가입 성공");
        errorText.text = "가입 성공";
    }

    private void RegisterFailure(PlayFabError error)
    {
        Debug.LogWarning("가입 실패");
        Debug.LogWarning(error.GenerateErrorReport());
        errorText.text = error.GenerateErrorReport();
    }

    private void GetAccountSuccess(GetAccountInfoResult result)
    {
        Debug.Log("Accout를 정상적으로 받아옴");

        nickName = result.AccountInfo.TitleInfo.DisplayName;
        if (nickName == null)
        {
            //받아온 닉네임이 없으면 닉네임 설정 씬으로
            //SceneManager.LoadScene("NicknameSet");
            showNickName.text = "닉네임을 설정해주세요";
        }
        else
        {
            //받아온 닉네임이 있으면 설정하고 시작으로
            //PlayerPrefs.SetString("Nickname", nickname);
            //SceneManager.LoadScene("Lobby");
            //showNickName.text = nickname;
        }
    }

    private void GetAccountFailure(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }

    public void OnclickRegister()
    {
        ChangeState(State.CREATEACCOUNT);
    }

    public void OnclickCancel()
    {
        ChangeState(State.LOGIN);
    }

    public void OnclickOK()
    {
        errorPenel.SetActive(false);
        this.state = curState;
    }
    private void OnError(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }

    //Playfab
    void RequestPhotonToken(LoginResult result)
    {

        Debug.Log("Status : PlayFab authenticated. Requesting photon token...");
        _playFabPlayerIdCache = result.PlayFabId;
        PlayFabClientAPI.GetPhotonAuthenticationToken(new GetPhotonAuthenticationTokenRequest
        {
            PhotonApplicationId = "c365abe6-ca57-4c21-aed2-5698a0d0a364"
        }, AuthenticateWithPhoton, OnError);
    }

    void AuthenticateWithPhoton(GetPhotonAuthenticationTokenResult result)
    {
        Debug.Log("Status : Photon token acquired! Authentication complete.");

        //We set AuthType to custom, meaning we bring our own, PlayFab authentication procedure.
        var customAuth = new Photon.Realtime.AuthenticationValues { AuthType = CustomAuthenticationType.Custom };
        //We add "username" parameter. Do not let it confuse you: PlayFab is expecting this parameter to contain player PlayFab ID (!) and not username.
        customAuth.AddAuthParameter("username", _playFabPlayerIdCache);    // expected by PlayFab custom auth service

        //We add "token" parameter. PlayFab expects it to contain Photon Authentication Token issues to your during previous step.
        customAuth.AddAuthParameter("token", result.PhotonCustomAuthenticationToken);

        //We finally tell Photon to use this authentication parameters throughout the entire application.
        PhotonNetwork.AuthValues = customAuth;
        Debug.Log(result);
    }
}
