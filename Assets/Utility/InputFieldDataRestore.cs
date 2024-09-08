using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldDataRestore : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputFieldTMP;
    [SerializeField] private InputField _inputField;
    [SerializeField] private string _initialString, _savedString;

    private void Start()
    {
        _inputField = GetComponent<InputField>();
        _inputFieldTMP = GetComponent<TMP_InputField>();
    }

    public void SaveDataOnValueChange(string data)
    {
        if (!string.IsNullOrWhiteSpace(data))
        {
            Utility.myLog("SaveDataOnValueChange - " + data);
            _savedString = data;
        }
    }

    public void RestoreData()
    {
        Utility.myLog("RestoreData - " + _savedString);
        if (_inputField != null)
            _inputField.text = _savedString;

        if (_inputFieldTMP != null)
            _inputFieldTMP.text = _savedString;
    }

    public void OnSelect(string data)
    {
        Utility.myLog("OnSelect - " + data);
        _initialString = data;

    }

    public void OnDeselect(string data)
    {
        Utility.myLog("OnDeselect - " + data);
    }

}
