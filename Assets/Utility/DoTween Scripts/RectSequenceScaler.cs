using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System.Threading.Tasks;

public class RectSequenceScaler : MonoBehaviour
{
    [SerializeField] private List<Transform> _transforms = new List<Transform>();
    [SerializeField] private Vector3 _finalScale = Vector3.one;
    private async void OnEnable()
    {
        for (int i = 0; i < _transforms.Count; i++)
        {
            _transforms[i].DOScale(_finalScale, 1).From(Vector3.zero);
            await Task.Delay(100);
        }
    }
}
