using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    //Config params.
    [SerializeField] AudioClip breakSound;
    [SerializeField] GameObject blockSparklesVFX;
    [SerializeField] Sprite[] hitSprites;

    Level level;    //cached variable.

    //State variable
    [SerializeField] int hitCount;  //Only for debug purpose

    //Sound
    float volume; 
    private void Awake()
    {
        volume = PlayerPrefs.GetFloat(StaticUrlScript.Volume, 1);
        CountBreakableBlocks();
    }

    private void CountBreakableBlocks()
    {
        level = FindObjectOfType<Level>();
        if (tag == "Breakable")
        {
            level.CountBlocks();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (tag == "Breakable")
        {
            HandleHit();

        }
    }

    private void ShowNextHitSprite()
    {
        int spriteIndex = hitCount - 1;
        // if(hitSprites[spriteIndex] != null)
        if (spriteIndex >= 0 && spriteIndex < hitSprites.Length && hitSprites[spriteIndex] != null)
        {
            GetComponent<SpriteRenderer>().sprite = hitSprites[spriteIndex];
        }
        else
        {
            Debug.LogError("Block Sprite Missing Error!" + gameObject.name);
        }
    }

    private void HandleHit()
    {
        hitCount++;
        int maxHits = hitSprites.Length + 1;
        if (hitCount >= maxHits)
        {
            DestroyBlock();
        }
        else
        {
            ShowNextHitSprite();
        }
    }

    private void DestroyBlock()
    {
        PlayBlockDestroySFX();
        Destroy(gameObject);
        level.BlockDestroyed();
        TriggerSparkleVFX();

    }

    private void PlayBlockDestroySFX()
    {
        GameSession.Instance.AddToScore();
        AudioSource.PlayClipAtPoint(breakSound, Camera.main.transform.position, volume);
    }

    private void TriggerSparkleVFX()
    {
        GameObject sparkles = Instantiate(blockSparklesVFX, transform.position, transform.rotation);
        Destroy(sparkles, 1f);
    }
}
