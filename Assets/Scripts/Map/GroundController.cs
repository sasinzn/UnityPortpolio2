using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GroundController : MonoBehaviour
{
    private enum Direction
    {
        UP, DOWN, LEFT, RIGHT, NONE
    }

    public int gridWidth = 30;
    public int gridHeight = 20;

    private List<Tilemap> grounds = new List<Tilemap>();    
    private Transform[,] groundMap = new Transform[3,3];
    private GameObject target;

    private void Awake()
    {
        GetComponentsInChildren<Tilemap>(grounds);

        for(int i = 0; i < 3; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                groundMap[j , i] = grounds[i * 3 + j].transform;
            }
        }
    }

    private void Start()
    {
        target = GameManager.instance.player;
    }

    private void Update()
    {
        Direction direction = GetTargetDirection();

        switch (direction)
        {
            case Direction.RIGHT:
                SetRightTile();
                break;
            case Direction.LEFT:
                SetLeftTile();
                break;
            case Direction.UP:
                SetUpTile();
                break;
            case Direction.DOWN:
                SetDownTile();
                break;
        }
    }

    private Direction GetTargetDirection()
    {
        Vector2 temp = target.transform.position - groundMap[1, 1].position;

        Vector2 halfSize = new Vector2(gridWidth * 0.5f, gridHeight * 0.5f);

        if (temp.x > halfSize.x)
            return Direction.RIGHT;
        if (temp.x < -halfSize.x)
            return Direction.LEFT;
        if(temp.y > halfSize.y)
            return Direction.UP;
        if (temp.y < -halfSize.y)
            return Direction.DOWN;

        return Direction.NONE;
    }

    private void HorizontalRelocate()
    {
        for (int i = 1; i <= 2; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Transform temp = groundMap[i, j];
                groundMap[i, j] = groundMap[(i + 1) % 3, j];
                groundMap[(i + 1) % 3, j] = temp;
            }
        }
    }

    private void VerticalRelocate()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 1; j >= 0; j--)
            {
                Transform temp = groundMap[i, j];
                groundMap[i, j] = groundMap[i, (j + 1) % 3];
                groundMap[i, (j + 1) % 3] = temp;
            }
        }
    }

    private void SetRightTile()
    {
        for(int i = 0; i < 3; i++)
        {
            Vector3 pos = groundMap[2, i].transform.position;
            pos.x += gridWidth;
            groundMap[0, i].transform.position = pos;
        }

        HorizontalRelocate();
    }

    private void SetLeftTile()
    {
        for (int i = 0; i < 3; i++)
        {
            Vector3 pos = groundMap[0, i].transform.position;
            pos.x -= gridWidth;
            groundMap[2, i].transform.position = pos;
        }

        HorizontalRelocate();
    }

    private void SetUpTile()
    {
        for (int i = 0; i < 3; i++)
        {
            Vector3 pos = groundMap[i, 0].transform.position;
            pos.y += gridHeight;
            groundMap[i, 2].transform.position = pos;
        }

        VerticalRelocate();
    }

    private void SetDownTile()
    {
        for (int i = 0; i < 3; i++)
        {
            Vector3 pos = groundMap[i, 2].transform.position;
            pos.y -= gridHeight;
            groundMap[i, 0].transform.position = pos;
        }

        VerticalRelocate();
    }
}
