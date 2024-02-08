using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMove : MonoBehaviourPun
{
    public int key;
    public bool isMove = true;
    public float knockbackSpeed = 50.0f;

    private GameObject target;
    private MonsterData data;
    private CharacterSprite characterSprite;
    private Animator animator;
    
    private Vector2 _force;
    public Vector2 force
    { set { _force = value; } }

    private int horizontalHash = Animator.StringToHash("Horizontal");
    private int verticalHash = Animator.StringToHash("Vertical");

    private void Awake()
    {
        characterSprite = GetComponent<CharacterSprite>();        
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        target = GameManager.instance.player;        
    }

    private void OnEnable()
    {
        isMove = true;
    }

    private void Update()
    {
        Move();
        Knockback();
    }

    private void Move()
    {
        if (!isMove) return;        

        Vector2 direction = target.transform.position - transform.position;

        transform.Translate(direction.normalized * data.speed * Time.deltaTime);

        animator.SetFloat(horizontalHash, direction.normalized.x);
        animator.SetFloat(verticalHash, direction.normalized.y);
    }

    private void Knockback()
    {
        if (_force.sqrMagnitude == 0) return;

        isMove = false;

        transform.Translate(_force * Time.deltaTime);

        _force = Vector2.Lerp(_force, Vector2.zero, knockbackSpeed * Time.deltaTime);

        if (_force.sqrMagnitude < 0.1f)
        {
            isMove = true;
        }
    }

    public void Spawn(int key, Vector2 pos)
    {
        transform.position = pos;

        gameObject.SetActive(true);

        data = DataManager.instance.GetMonsterData(key);
        characterSprite.LoadSprite(data.spriteName);        

        GetComponent<MonsterDamage>().SetCollider(data);
    }
    [PunRPC]
    public void NetSpawn(int key, Vector2 pos)
    {
        transform.position = pos;

        gameObject.SetActive(true);

        data = DataManager.instance.GetMonsterData(key);
        characterSprite.LoadSprite(data.spriteName);

        GetComponent<MonsterDamage>().SetCollider(data);
    }
}
