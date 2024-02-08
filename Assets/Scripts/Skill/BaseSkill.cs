using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSkill : MonoBehaviour
{
    protected SkillData skillData;
    protected WaitForSeconds skillTime;
    protected Transform target = null;        

    virtual protected void Awake()
    {
        SetSkillData("BaseSkillData");        
    }

    protected void SetSkillData(string dataFile)
    {
        skillData = Resources.Load<SkillData>("SkillData/" + dataFile);
        skillTime = new WaitForSeconds(skillData.interval);
    }

    virtual protected void Start()
    {
        StartCoroutine(Fire());
    }

    private IEnumerator Fire()
    {
        while(true)
        {
            yield return skillTime;

            target = MonsterManager.instance.GetClosestMonster(transform.position);

            if (target)
            {
                SetBullet(BulletManager.instance.GetBullet("Player"));
            }            
        }
    }

    private void SetBullet(GameObject obj)
    {
        Bullet bullet = obj.GetComponent<Bullet>();

        bullet.SetSkillData(skillData);
        bullet.SetFire(transform.position, target.position);        
        obj.layer = LayerMask.NameToLayer("PlayerBullet");

        SoundManager.instance.PlayFX(SoundKey.FIRE);
    }    
}
