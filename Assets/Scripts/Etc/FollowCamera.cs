using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private GameObject target;

    private void Start()
    {
        target = GameManager.instance.player;
    }
        
    private void Update()
    {
        transform.position = target.transform.position;
    }
}
