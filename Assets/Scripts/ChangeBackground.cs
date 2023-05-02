using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBackground : MonoBehaviour
{
    public SpriteRenderer spritesToChange;
    public Sprite[] newSprites;

    private void Start()
    {
        ChangeSprites();
    }
    void ChangeSprites()
    {
        int t = Random.Range(0, newSprites.Length);
        spritesToChange.sprite = newSprites[t];        
    }
}
