using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillData",
    menuName ="Create SkillData", order = 1)]
public class SkillData : ScriptableObject
{
    public string key = "Player";
    public Sprite sprite;
    public float interval = 1.0f;
    public float power = 10.0f;    
    public float speed = 10.0f;
    public float scale = 1.0f;
    public float distance = 2.0f;
    public float pushPower = 0.0f;
    public int emitterAmount = 1;
    public bool isTrigger = false;
    public bool isMove = true;
}
