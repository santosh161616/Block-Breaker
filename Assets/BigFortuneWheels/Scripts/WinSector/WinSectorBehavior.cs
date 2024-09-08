using MkeyFW;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mkey
{
    public class WinSectorBehavior : MonoBehaviour
    {
        protected Sector WinSector { get; private set; }
        protected WheelController Wheel{ get; private set; }

        #region regular
        private void Start()
        {
            Wheel = GetComponentInParent<WheelController>();
            if (!Wheel) return;

            WinSector = Wheel.WinSector;
            if (!WinSector) return;

            PlayWin();
        }

        private void OnDestroy()
        {
            Cancel();
        }
        #endregion regular

        #region virtual
        protected virtual void PlayWin()
        {

        }

        protected virtual void Cancel()
        {

        }
        #endregion virtual
    }
}