using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningSkill : BaseSkill
{
    private SpriteRenderer lightning;

    private List<Lightning> lightnings = new List<Lightning>();

    private void Awake()
    {
        SetSkillData("LightningSkillData");

        GameObject prefab = Resources.Load<GameObject>("Prefabs/Lightning");

        for(int i = 0; i < skillData.emitterAmount; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            lightnings.Add(obj.GetComponent<Lightning>());
        }        
    }

    private void Start()
    {
        StartCoroutine(Lightning());
    }

    private IEnumerator Lightning()
    {
        while (true)
        {
            StartCoroutine(LightningChain());

            yield return skillTime;
        }
    }

    private IEnumerator LightningChain()
    {
        List<Transform> targets = MonsterManager.instance.GetClosestMonsters(
            transform.position, skillData.emitterAmount);

        Transform start = transform;
        Transform end;

        for (int i = 0; i < targets.Count; i++)
        {
            end = targets[i];
            Vector2 force = (end.position - transform.position).normalized;
            end.GetComponent<MonsterDamage>().Damage(skillData.power, force * skillData.pushPower);

            lightnings[i].StartLightining(start, end);            

            yield return new WaitForSeconds(skillData.speed);

            start = targets[i];
        }        
    }
}
