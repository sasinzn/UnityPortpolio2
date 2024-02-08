using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Lightning : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private Transform startTarget;
    private Transform endTarget;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(startTarget == null || endTarget == null) return;

        Vector3 start = startTarget.position;
        Vector3 end = endTarget.position;

        transform.position = (start + end) * 0.5f;

        Vector3 direction = end - start;
        float distance = direction.magnitude;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) + Mathf.PI * 0.5f;
        Vector3 rot = new Vector3();
        rot.z = Mathf.Rad2Deg * angle;

        transform.rotation = Quaternion.Euler(rot);

        Vector2 size = spriteRenderer.size;
        size.y = distance;
        spriteRenderer.size = size;
    }

    public void StartLightining(Transform start, Transform end)
    {
        gameObject.SetActive(true);

        startTarget = start;
        endTarget = end;
    }

    public void EndLightning()
    {
        gameObject.SetActive(false);
    }
}
