using System;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "wheelData", menuName = "ScriptableObjects/SpinwheelData", order = 3)]
public class SpinWheelData : ScriptableObject
{
    [SerializeField] public int spinWheelType;
    [SerializeField] public int resultCoinId;
    [SerializeField] public List<spinWheelValue> spinWheelValues;
    [SerializeField] public string spinWheelId;

}

[Serializable]
public class spinWheelValue
{
    [SerializeField] public int Id;
    [SerializeField] public int Coins;
    [SerializeField] public string Icon;
}
