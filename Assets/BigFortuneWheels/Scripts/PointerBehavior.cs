using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mkey;

namespace MkeyFW
{
    public class PointerBehavior : MonoBehaviour
    {
        [SerializeField]
        private AudioClip pointerHit;
        private AudioSource audioSource;

        #region regular
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource && pointerHit) audioSource.clip = pointerHit;
        }
        #endregion regular

        /// <summary>
        /// Used as animation curve event handler
        /// </summary>
        public void Hithandler()
        {
            if (pointerHit && audioSource && !audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }
}