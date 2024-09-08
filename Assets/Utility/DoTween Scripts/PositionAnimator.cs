using UnityEngine;
using DG.Tweening;
using System.Collections;
using System;

public class PositionAnimator : MonoBehaviour
{
    [SerializeField] private Vector2 _initialPosition, _finalPosition;
    [SerializeField] private float _duration = 1;
    [SerializeField] private Ease _ease = Ease.OutExpo;
    [SerializeField] private bool validAnimation = true;

    // Start is called before the first frame update
    void OnEnable()
    {
        if (validAnimation)
        {
            validAnimation = false;
            Utility.myLog("SpinWheel Enabled------------->>");
            transform.DOLocalMove(_finalPosition, _duration).SetEase(_ease).From(_initialPosition);
            StartCoroutine(Validate());
        }
    }

    IEnumerator Validate()
    {
        yield return new WaitForSeconds(1);
        validAnimation = true;
    }

}
