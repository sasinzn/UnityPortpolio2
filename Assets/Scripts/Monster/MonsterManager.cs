using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterManager : MonoBehaviourPunCallbacks
{
    private enum Direction
    {
        Left, Right, Top, Bottom
    };

    public static MonsterManager instance;

    public int poolSize = 50;
    public float spawnInterval = 1.0f;
    public int spawnAmount = 3;
    public float spawnOffset = 0;

    private WaitForSeconds spawnTime;    

    private List<GameObject> monsters = new List<GameObject>();
    [SerializeField]
    private List<GameObject> netMonsters = new List<GameObject>();
    private Transform target;
    private PhotonView pv;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        target = GameManager.instance.player.transform;
        spawnTime = new WaitForSeconds(spawnInterval);
        pv = this.GetComponent<PhotonView>();
        
        if (PhotonNetwork.IsConnected)
        {
            if(PhotonNetwork.IsMasterClient)
            {
                CreateMonsters();
                StartCoroutine(SpawnMonster());
            }
        }
        else
        {
            CreateMonsters();
            StartCoroutine(SpawnMonster());
        } 
    }

    private IEnumerator SpawnMonster()
    {
        while (true)
        {
            if (target == null)
                target = GameManager.instance.player.transform;
            else
            {
                for (int i = 0; i < spawnAmount; i++)
                {
                    if(PhotonNetwork.IsConnected)
                    {
                        if(PhotonNetwork.IsMasterClient)
                        {
                            pv.RPC("NetSpawn", RpcTarget.AllBuffered);
                        }
                    }
                    else
                    {
                        Spawn();
                    }
                       
                }
            }
            yield return spawnTime;
        }        
    }
    [PunRPC]
    private void NetSpawn()
    {

        int key = Random.Range(0, DataManager.instance.GetMonsterDataSize());

        Direction dir = (Direction)Random.Range(0, 4);


        Vector2 spawnPos = new Vector2();
        Vector2 leftBottom = GameManager.instance.screenLeftBottom + (Vector2)target.position;
        Vector2 rightTop = GameManager.instance.screenRightTop + (Vector2)target.position;


        switch (dir)
        {
            case Direction.Left:
                spawnPos.x = leftBottom.x - spawnOffset;
                spawnPos.y = Random.Range(leftBottom.y, rightTop.y);
                break;
            case Direction.Right:
                spawnPos.x = rightTop.x + spawnOffset;
                spawnPos.y = Random.Range(leftBottom.y, rightTop.y);
                break;
            case Direction.Bottom:
                spawnPos.x = Random.Range(leftBottom.x, rightTop.x);
                spawnPos.y = leftBottom.y - spawnOffset;
                break;
            case Direction.Top:
                spawnPos.x = Random.Range(leftBottom.x, rightTop.x);
                spawnPos.y = rightTop.y + spawnOffset;
                break;
        }

        foreach (var monster in netMonsters)
        {
            if (!monster.activeSelf)
            {
                monster.GetComponent<PhotonView>().RPC("NetSpawn", RpcTarget.AllBuffered, key, spawnPos);

                //monster.Value.GetComponent<MonsterMove>().Spawn(key, spawnPos);
                return;
            }
        }
    }

    private void Spawn()
    {   

        int key = Random.Range(0, DataManager.instance.GetMonsterDataSize());

        Direction dir = (Direction)Random.Range(0, 4);
       

        Vector2 spawnPos = new Vector2();
        Vector2 leftBottom = GameManager.instance.screenLeftBottom + (Vector2)target.position;
        Vector2 rightTop = GameManager.instance.screenRightTop + (Vector2)target.position;


        switch (dir)
        {
            case Direction.Left:
                spawnPos.x = leftBottom.x -spawnOffset;
                spawnPos.y = Random.Range(leftBottom.y, rightTop.y);
                break;
            case Direction.Right:
                spawnPos.x = rightTop.x + spawnOffset;
                spawnPos.y = Random.Range(leftBottom.y, rightTop.y);
                break;
            case Direction.Bottom:
                spawnPos.x = Random.Range(leftBottom.x, rightTop.x);
                spawnPos.y = leftBottom.y - spawnOffset;
                break;
            case Direction.Top:
                spawnPos.x = Random.Range(leftBottom.x, rightTop.x);
                spawnPos.y = rightTop.y + spawnOffset;
                break;
        }

        foreach(GameObject monster in monsters)
        {
            if(!monster.activeSelf)
            {
                monster.GetComponent<MonsterMove>().Spawn(key, spawnPos);
                return;
            }
        }
    }

    private void CreateMonsters()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (!PhotonNetwork.IsMasterClient)
                return;
            else
            {
                for (int i = 0; i < poolSize; i++)
                {
                    GameObject monster = PhotonNetwork.Instantiate("Prefabs/Monster", Vector3.zero, Quaternion.identity);
                    monster.transform.SetParent(this.transform);
                    monster.SetActive(false);
                    netMonsters.Add(monster);
                }
               
            }
        }
        else
        {
            GameObject monsterPrefab = Resources.Load<GameObject>("Prefabs/Monster");

            for (int i = 0; i < poolSize; i++)
            {
                GameObject monster = Instantiate(monsterPrefab, transform);
                monster.SetActive(false);
                monsters.Add(monster);
            }
        }
    }

    public Transform GetClosestMonster(Vector3 pos)
    {
        float minDistance = Mathf.Infinity;
        Transform closestMonster = null;

        foreach(GameObject monster in monsters)
        {
            if (!monster.activeSelf) continue;

            float distance = Vector3.Distance(pos, monster.transform.position);

            if(minDistance > distance)
            {
                minDistance = distance;
                closestMonster = monster.transform;
            }
        }

        return closestMonster;
    }

    public Transform GetClosestNetMonster(Vector3 pos)
    {
        float minDistance = Mathf.Infinity;
        Transform closestMonster = null;

        foreach (GameObject monster in netMonsters)
        {
            if (!monster.activeSelf) continue;

            float distance = Vector3.Distance(pos, monster.transform.position);

            if (minDistance > distance)
            {
                minDistance = distance;
                closestMonster = monster.transform;
            }
        }

        return closestMonster;
    }

    public List<Transform> GetClosestMonsters(Vector3 pos, int size)
    {
        List<Transform> closestMonster = new List<Transform>();
        Dictionary<float, Transform> map = new Dictionary<float, Transform>();

        foreach (GameObject monster in monsters)
        {
            if (!monster.activeSelf) continue;

            float distance = Vector3.Distance(pos, monster.transform.position);

            map.Add(distance, monster.transform);
        }

        List<float> list = map.Keys.ToList();
        list.Sort();

        foreach(float distance in list)
        {
            closestMonster.Add(map[distance]);

            if (closestMonster.Count == size)
                return closestMonster;
        }

        return closestMonster;
    }

    public List<Transform> GetClosestNetMonsters(Vector3 pos, int size)
    {
        List<Transform> closestMonster = new List<Transform>();
        Dictionary<float, Transform> map = new Dictionary<float, Transform>();

        foreach (GameObject monster in netMonsters)
        {
            if (!monster.activeSelf) continue;

            float distance = Vector3.Distance(pos, monster.transform.position);

            map.Add(distance, monster.transform);
        }

        List<float> list = map.Keys.ToList();
        list.Sort();

        foreach (float distance in list)
        {
            closestMonster.Add(map[distance]);

            if (closestMonster.Count == size)
                return closestMonster;
        }

        return closestMonster;
    }
}
