using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterMove : MonoBehaviourPun
{
    public string spriteName;
    public float speed = 1f;
    public float stickRange = 200.0f;
    public TextMeshProUGUI nickName;

    private float horizontal;
    private float vertical;
    private Vector2 direction;

    private Animator animator;
    private CharacterSprite characterSprite;
    

    [SerializeField]
    private GameObject joypad;
    private Transform padStick;

    private void Start()
    {
        if(PhotonNetwork.IsConnected)
        {
            nickName.text = GetComponent<PhotonView>().IsMine ? PhotonNetwork.NickName : GetComponent<PhotonView>().Owner.NickName;
            nickName.color = GetComponent<PhotonView>().IsMine ? Color.green : Color.red;
        }
        else
        {
            nickName.gameObject.SetActive(false);
        }
        animator = GetComponent<Animator>();
        characterSprite = GetComponent<CharacterSprite>();
        characterSprite.LoadSprite(spriteName);
        joypad = GameObject.Find("Joypad");
        padStick = joypad.transform.GetChild(0);

        joypad.SetActive(false);
    }


    private void Update()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected) return;

        TouchControl();
        //KeyboardControl();
        Move();
    }

    private void TouchControl()
    {
        if(Input.GetMouseButtonDown(0))
        {
            joypad.SetActive(true);
            joypad.transform.position = Input.mousePosition;
        }

        if(Input.GetMouseButton(0))
        {
            padStick.position = Input.mousePosition;

            Vector2 stickPos = padStick.position;
            Vector2 padPos = joypad.transform.position;
            direction = (stickPos - padPos) / stickRange;                        

            if(direction.sqrMagnitude > 1.0f)
            {
                direction.Normalize();
                padStick.position = padPos + direction * stickRange;
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            joypad.SetActive(false);

            direction = Vector2.zero;
        }
    }

    private void KeyboardControl()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        direction = Vector2.right * horizontal + Vector2.up * vertical;

        if (direction.sqrMagnitude > 1)
            direction.Normalize();
    }

    private void Move()
    {        
        transform.Translate(direction * speed * Time.deltaTime);

        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);
    }
    
}
