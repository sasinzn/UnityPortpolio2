using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;

    public int poolSize = 100;

    private List<GameObject> effects = new List<GameObject>();

    private void Awake()
    {
        instance = this;

        CreateEffects();
    }

    private void CreateEffects()
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/Effect");

        for(int i = 0; i < poolSize; i++)
        {
            GameObject effect = Instantiate(prefab, transform);
            effect.SetActive(false);

            effects.Add(effect);
        }
    }

    public void Play(Vector3 pos)
    {
        foreach(GameObject effect in effects)
        {
            if(!effect.activeSelf)
            {
                effect.transform.position = pos;
                effect.SetActive(true);                
                return;
            }
        }
    }
}
