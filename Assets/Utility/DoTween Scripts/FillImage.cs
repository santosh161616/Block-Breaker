using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

public class FillImage : MonoBehaviour
{
    [Header("Auto Reference for getting image component automatically")]
    [SerializeField] private bool _autoReference = true;
    [Space]
    [SerializeField] private Image _imageToFill;
    [SerializeField] private float _timeToFill = 3;
    public UnityEvent OnFillCompleteEvent = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {
        if (_autoReference) _imageToFill = GetComponent<Image>();
        EnableSliderFilling();
    }

    public void EnableSliderFilling()
    {
        _imageToFill.DOFillAmount(1, _timeToFill).From(0).OnComplete(() => { OnFillCompleteEvent?.Invoke(); });
    }
}
