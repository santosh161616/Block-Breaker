using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShinyEffect : MonoBehaviour
{
    private Material spriteMaterial = null;
    private float shinePositon = 0;
    private Coroutine shineRoutine = null;
    private int shineLocationParameterID;
    private SpriteRenderer spriteRenderer;
    private float shineSpeed;
    private bool loop;
    private int shineRepeats;
    private float shineWaitTime;

    void Start()
    {
        shineLocationParameterID = Shader.PropertyToID("_ShineLocation");

        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteMaterial = spriteRenderer.material;
    }

    public void StartShine(float delay)
    {
        if (shineRoutine != null)
            StopCoroutine(shineRoutine);
        shineRoutine = StartCoroutine(StartShineCoroutine(delay));
    }

    public void StopShine()
    {
        if (shineRoutine != null)
            StopCoroutine(shineRoutine);
        shineRoutine = null;
    }

    private float ShineCurve(float lerpProgress)
    {
        float newValue = lerpProgress * lerpProgress * lerpProgress * (lerpProgress * (6f * lerpProgress - 15f) + 10f);
        return newValue;
    }

    private IEnumerator StartShineCoroutine(float dealay)
    {
        yield return new WaitForSeconds(dealay);

        if (shineSpeed <= 0f)
            yield break;

        int count = loop ? 1 : shineRepeats;
        while (count > 0)
        {
            yield return new WaitForSeconds(shineWaitTime);

            count = loop ? 1 : count - 1;

            float startTime = Time.time;

            while (Time.time < startTime + 1f / shineSpeed)
            {
                shinePositon = ShineCurve((Time.time - startTime) * shineSpeed);
                spriteMaterial.SetFloat(shineLocationParameterID, shinePositon);

                yield return new WaitForEndOfFrame();
            }
        }

        yield break;
    }
}
