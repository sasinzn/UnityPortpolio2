using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : MonoBehaviour
{
    // Start is called before the first frame update
    public int poolSize = 4;
    private GameObject boomPrefab;

    private List<GameObject> boomEffects = new List<GameObject>();

    private void Awake()
    {
        boomPrefab = Resources.Load<GameObject>("Prefabs/X86BoomEffect");

        for (int i = 0; i < poolSize; i++)
        {
            GameObject boom = Instantiate(boomPrefab, Vector3.zero, Quaternion.identity);
            boom.SetActive(false);
            boomEffects.Add(boom);
        }
    }

    public void UseX86Boom()
    {
        foreach (GameObject boom in boomEffects)
        {
            if(!boom.activeSelf)
            {
                boom.SetActive(true);
                transform.position = Vector3.zero;
                return;
            }
        }
    }
}
