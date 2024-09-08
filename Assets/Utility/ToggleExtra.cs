using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleExtra : MonoBehaviour
{
    [SerializeField] private Toggle _toggle;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Color32 _selectedColor = Color.white;
    [SerializeField] private Color32 _disabledColor = new Color32(106, 45, 226, 255);
    private void Awake()
    {
        _toggle = GetComponent<Toggle>();
    }
    private void Start()
    {
        OnToggleChange();
    }
    public void OnToggleChange()
    {
        if (_toggle != null && _text != null)
        {
            if (_toggle.isOn)
            {
                _text.color = _selectedColor;
            }
            else
            {
                _text.color = _disabledColor;
            }
        }
    }
}
