using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TweenTest : MonoBehaviour
{
    public Transform stars;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Tween Start");
        transform.DORotate(new Vector3(0, 0, -360), 3,RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart).OnComplete(()=> { Debug.Log("Completed 1"); }); 
        stars.DORotate(new Vector3(0, 0, -360), 3, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart).OnComplete(() => { Debug.Log("Completed 1"); });
    }

    private void OnDestroy()
    {
        Debug.Log(DOTween.KillAll());
    }
}
