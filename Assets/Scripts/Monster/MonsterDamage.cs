using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterDamage : MonoBehaviour
{
    public Vector2 offset;
    public float damageEffectSpeed = 1.0f;

    private CircleCollider2D collider;
    private MonsterMove monsterMove;
    private MonsterData monsterData;
    private Slider hpBar = null;    
    private Material monsterMaterial;
    private Rigidbody2D rigidbody;

    private Color baseColor = Color.black;
    private Color damageColor = Color.white;

    private float curHp;

    private void Awake()
    {
        collider = GetComponent<CircleCollider2D>();
        monsterMove = GetComponent<MonsterMove>();
        monsterData = DataManager.instance.GetMonsterData(monsterMove.key);
        monsterMaterial = GetComponent<SpriteRenderer>().material;
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        GameObject hpBarPrefab = Resources.Load<GameObject>("Prefabs/HpBar");

        GameObject hpBarParent = GameObject.Find("MonsterHpBar");

        GameObject hpBarObj = Instantiate(hpBarPrefab, hpBarParent.transform);
        hpBar = hpBarObj.GetComponent<Slider>();
        hpBar.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        curHp = monsterData.health;

        if(hpBar)
        {
            hpBar.gameObject.SetActive(false);
            hpBar.value = 1.0f;
        }

        monsterMaterial.SetColor("_AddColor", baseColor);
    }

    private void Update()
    {
        Vector2 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        screenPos += offset;
        hpBar.transform.position = screenPos;
    }

    public void SetCollider(MonsterData data)
    {
        collider.radius = data.colliderSize;
        collider.offset = new Vector2(0, data.colliderOffset);
    }

    public void Damage(float damage, Vector2 force)
    {
        curHp -= damage;
        hpBar.gameObject.SetActive(true);
        hpBar.value = curHp / monsterData.health;
        //StopAllCoroutines();
        StopCoroutine(DamageEffect());
        StartCoroutine(DamageEffect());

        monsterMove.force = force;

        if (curHp <= 0)
        {
            gameObject.SetActive(false);
            hpBar.gameObject.SetActive(false);
            SoundManager.instance.PlayFX(SoundKey.EXPLOSION);
        }
    }

    //private IEnumerator Knockback()
    //{
    //    monsterMove.isMove = false;
    //
    //    yield return new WaitForSeconds(0.5f);
    //
    //    monsterMove.isMove = true;
    //    rigidbody.velocity = Vector3.zero;
    //}

    private IEnumerator DamageEffect()
    {
        Color color;
        float time = 0.0f;
        bool isIncrease = true;
    
        while(true)
        {
            if(isIncrease)
                time += damageEffectSpeed * Time.deltaTime;
            else
                time -= damageEffectSpeed * Time.deltaTime;

            color = Color.Lerp(baseColor, damageColor, time);
            monsterMaterial.SetColor("_AddColor", color);
            yield return null;

            if(time > 1.0f)
            {
                time = 1.0f;
                isIncrease = false;
            }

            if (time < 0.0f)
                yield break;
        }
    }
}
