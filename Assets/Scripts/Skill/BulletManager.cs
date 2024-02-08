using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    private static BulletManager _instance;
    public static BulletManager instance
    {
        get 
        { 
            if( _instance == null )
            {
                //Create Empty
                GameObject obj = new GameObject("BulletManager");
                _instance = obj.AddComponent<BulletManager>();
            }

            return _instance; 
        }
        
    }

    private GameObject bulletPrefab;
    //private List<GameObject> bullets = new List<GameObject>();    
    private Dictionary<string, List<GameObject>> totalBullet = new Dictionary<string, List<GameObject>>();

    public void CreateBullets(string key, string prefab,  int poolSize)
    {
        bulletPrefab = Resources.Load<GameObject>(prefab);

        List<GameObject> bullets = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform);
            bullet.SetActive(false);
            bullets.Add(bullet);
        }

        totalBullet[key] = bullets;
    }

    public void Fire(string key, Vector3 pos)
    {
        foreach (GameObject bullet in totalBullet[key])
        {
            if (!bullet.activeSelf)
            {
                bullet.SetActive(true);
                bullet.transform.position = pos;
                return;
            }
        }
    }

    public void Fire(string key, Vector3 firePos, Vector3 targetPos)
    {
        foreach (GameObject bullet in totalBullet[key])
        {
            if (!bullet.activeSelf)
            {
                bullet.GetComponent<Bullet>().SetFire(firePos, targetPos);
                return;
            }
        }
    }

    public void FireAngle(string key, Vector3 firePos, float angle)
    {
        foreach (GameObject bullet in totalBullet[key])
        {
            if (!bullet.activeSelf)
            {
                bullet.GetComponent<Bullet>().SetFire(firePos, angle);
                return;
            }
        }
    }

    public GameObject GetBullet(string key)
    {
        foreach (GameObject bullet in totalBullet[key])
        {
            if (!bullet.activeSelf)
            {
                return bullet;
            }
        }

        return null;
    }
}
