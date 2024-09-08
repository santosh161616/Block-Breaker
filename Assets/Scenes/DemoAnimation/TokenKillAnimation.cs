using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;

public class TokenKillAnimation : MonoBehaviour
{
    public GameObject effect;
    List<Tween> tween = new List<Tween>();
    async void OnEnable()
    {
        ResetPositionAndScale();
        await Task.Delay(1000);
        tween.Add(transform.DOLocalJump(transform.position, 2, 1, 1).SetLoops(-1).OnStepComplete(() => { /*Instantiate(effect,transform.position, Quaternion.identity);*/ }));
        tween.Add(transform.DOPunchScale(new Vector3(0, 0.02f, 0), 1f, 2, 0.5f).SetLoops(-1));/*.OnComplete(() => { transform.dos})*/;
    }

    private void ResetPositionAndScale()
    {
        transform.position = Vector3.zero;
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }

    private void OnDisable()
    {
        Debug.Log(tween.Count);
        tween.ForEach(t => t.Kill());
    }


}
