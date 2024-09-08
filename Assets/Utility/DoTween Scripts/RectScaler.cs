using UnityEngine;
using DG.Tweening;

public class RectScaler : MonoBehaviour
{
    private RectTransform _objectToScale;
    [SerializeField] private float _duration = 1f;
    [SerializeField] private Ease ease = Ease.OutExpo;

    private void Awake()
    {
            _objectToScale = GetComponent<RectTransform>();
    }
    // Start is called before the first frame update
    void OnEnable()
    {
        if (_objectToScale != null)
            _objectToScale.DOScale(_objectToScale.transform.localScale, _duration).SetEase(ease).From(0);
    }

    private void OnDestroy()
    {
        DOTween.KillAll();
    }

    [ContextMenu("Test Scaling")]
    private void Test()
    {
        _objectToScale = GetComponent<RectTransform>();
        if (_objectToScale != null)
            _objectToScale.DOScale(_objectToScale.transform.localScale, _duration).SetEase(ease).From(0);

    }
}
