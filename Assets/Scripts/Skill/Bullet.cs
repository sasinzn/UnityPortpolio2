using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private SkillData skillData;
    private SpriteRenderer renderer;
    private Vector3 direction;
    private Collider2D collider;

    private Transform player;

    private void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        player = GameManager.instance.player.transform;
    }

    private void Update()
    {
        if (!skillData.isMove) return;

        transform.Translate(direction * skillData.speed * Time.deltaTime, Space.World);

        if(!renderer.isVisible)
            gameObject.SetActive(false);
    }

    public void SetFire(Vector3 firePos, Vector3 targetPos)
    {
        gameObject.SetActive(true);
        direction = (targetPos - firePos).normalized;        
        transform.position = firePos;

        float angle = Mathf.Atan2(direction.y, direction.x) + Mathf.PI * 0.5f;
        Vector3 rot = new Vector3();
        rot.z = Mathf.Rad2Deg * angle;
        transform.rotation = Quaternion.Euler(rot);
    }

    public void SetFire(Vector3 firePos, float angle)
    {
        gameObject.SetActive(true);
        direction.x = Mathf.Cos(angle * Mathf.Deg2Rad);
        direction.y = Mathf.Sin(angle * Mathf.Deg2Rad);
        transform.position = firePos;
        
        Vector3 rot = new Vector3();
        rot.z = angle + 90;
        transform.rotation = Quaternion.Euler(rot);
    }

    public void SetSkillData(SkillData skillData)
    {
        this.skillData = skillData;
        transform.localScale = Vector3.one * skillData.scale;
        collider.isTrigger = skillData.isTrigger;
        renderer.sprite = skillData.sprite;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (skillData.key == "Player" && collision.collider.CompareTag("Monster"))
        {
            Vector2 force = (collision.transform.position - player.position).normalized;
            force *= skillData.pushPower;
            gameObject.SetActive(false);
            collision.gameObject.GetComponent<MonsterDamage>().Damage(skillData.power, force);
            EffectManager.instance.Play(transform.position);


        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (skillData.key == "Player" && collision.CompareTag("Monster"))
        {
            Vector2 force = (collision.transform.position - player.position).normalized;
            force *= skillData.pushPower;

            collision.gameObject.GetComponent<MonsterDamage>().Damage(skillData.power, force);
            EffectManager.instance.Play(transform.position);
        }
    }
}
