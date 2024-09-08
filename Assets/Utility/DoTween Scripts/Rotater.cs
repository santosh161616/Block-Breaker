using DG.Tweening;
using UnityEngine;

public class Rotater : MonoBehaviour
{
    [SerializeField] private Vector3 _finalRotation = new Vector3(0, 0, -360);
    [SerializeField] private float _duration = 3;
    [SerializeField] private bool _loopInfinite, _rotateBy;
    // Start is called before the first frame update
    void Start()
    {
        transform.DORotate(_finalRotation, _duration, _rotateBy ? RotateMode.FastBeyond360 : RotateMode.Fast).SetEase(Ease.Linear).SetLoops(_loopInfinite ? -1 : 1, LoopType.Restart);
    }

}
