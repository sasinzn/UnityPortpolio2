using System.Collections;
using System.Collections.Generic;
//using UnityEditor.U2D.Animation;
using UnityEngine;

public struct MonsterData
{
    public int key;
    public int health;
    public int attack;
    public float speed;
    public int exp;
    public int type;
    public float range;
    public string spriteName;
    public float colliderSize;
    public float colliderOffset;
}

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    private Dictionary<int,  MonsterData> monsterDatas = new Dictionary<int, MonsterData>();

    public MonsterData GetMonsterData(int key)
    { return monsterDatas[key]; }

    public int GetMonsterDataSize() {  return monsterDatas.Count; }

    private void Awake()
    {
        instance = this;

        LoadMonsterTable();
        DontDestroyOnLoad(gameObject);
    }

    private void LoadMonsterTable()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("DataTable/MonsterTable");

        string temp = textAsset.text;

        string[] rows = temp.Split("\r\n");

        for (int i = 1; i < rows.Length; i++)
        {
            if (rows[i].Length == 0)
                return;

            string[] cols = rows[i].Split(',');

            MonsterData data;
            data.key = int.Parse(cols[0]);
            data.health = int.Parse(cols[1]);
            data.attack = int.Parse(cols[2]);
            data.speed = float.Parse(cols[3]);
            data.exp = int.Parse(cols[4]);
            data.type = int.Parse(cols[5]);
            data.range = float.Parse(cols[6]);
            data.spriteName = cols[7];
            data.colliderSize = float.Parse(cols[8]);
            data.colliderOffset = float.Parse(cols[9]);

            monsterDatas.Add(data.key, data);
        }
    }
}
