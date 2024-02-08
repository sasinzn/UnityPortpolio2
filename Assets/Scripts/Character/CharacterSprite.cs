using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSprite : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Sprite[] sprites;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();        
    }

    public void LoadSprite(string spriteName)
    {
        string path = "Sprites/" + spriteName;
        sprites = Resources.LoadAll<Sprite>(path);
    }

    private void LateUpdate()
    {
        string spriteName = spriteRenderer.sprite.name;

        string[] temp = spriteName.Split("_");
        int index = int.Parse(temp[2]);
        spriteRenderer.sprite = sprites[index];
    }
}
