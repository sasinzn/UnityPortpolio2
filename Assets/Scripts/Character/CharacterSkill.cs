using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSkill : MonoBehaviour
{

    List<BaseSkill> skillList = new List<BaseSkill>();

    private void Awake()
    {
        skillList.Add(gameObject.AddComponent<BaseSkill>());
        skillList.Add(gameObject.AddComponent<SpearSkill>());
        skillList.Add(gameObject.AddComponent<RotateSkill>());
        skillList.Add(gameObject.AddComponent<LightningSkill>());
    }
}
