using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Fade : MonoBehaviour
{
    private Image _image;
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _duration = 1;
    [SerializeField] private Ease _ease = Ease.Linear;
    [SerializeField] private float _fadeStartValue = 0, _fadeFinalValue = 1;

    private void Start()
    {
        _image = GetComponent<Image>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        if (_image != null) _image.DOFade(_fadeFinalValue, _duration).SetEase(_ease).From(_fadeFinalValue);
        if (_spriteRenderer != null) _spriteRenderer.DOFade(_fadeFinalValue, _duration).SetEase(_ease).From(_fadeFinalValue);
    }
}
