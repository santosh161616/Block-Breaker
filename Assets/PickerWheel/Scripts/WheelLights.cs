using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EasyUI.PickerWheelUI
{
    public class WheelLights : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        private void Start()
        {
            PickerWheel.onSpinStartEvent += StartLights;
            PickerWheel.onSpinEndEvent += StopLights;
        }
        public void StartLights()
        {
            Debug.Log("StartLights");
            _animator.SetBool("Running", true);
        }

        public void StopLights(Piece piece)
        {
            Debug.Log("StopLights");
            _animator.SetBool("Running", false);
        }

        private void OnDestroy()
        {
            PickerWheel.onSpinStartEvent -= StartLights;
            PickerWheel.onSpinEndEvent -= StopLights;
        }
    }
}
