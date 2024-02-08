using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RotateSkill : BaseSkill
{
    private List<Transform> bullets = new List<Transform>();

    private void Awake()
    {
        SetSkillData("RotateSkillData");
    }

    private void Start()
    {
        for(int i = 0; i < skillData.emitterAmount; i++)
        {
            GameObject bulletObj = BulletManager.instance.GetBullet("Player");
            bulletObj.GetComponent<Bullet>().SetSkillData(skillData);
            bulletObj.SetActive(true);
            bullets.Add(bulletObj.transform);
        }

        StartCoroutine(RotateBullet());
    }

    private IEnumerator RotateBullet()
    {
        float angle = 0.0f;

        while(true)
        {
            angle += skillData.speed * Time.deltaTime;            

            float stepAngle = Mathf.PI * 2 / skillData.emitterAmount;

            for(int i = 0; i < bullets.Count; i++)
            {
                float bulletAngle = angle + stepAngle * i;
                Vector3 pos = new Vector3(Mathf.Cos(bulletAngle), Mathf.Sin(bulletAngle), 0);
                pos *= skillData.distance;

                bullets[i].position = pos + transform.position;
            }

            yield return null;
        }
    }
}
