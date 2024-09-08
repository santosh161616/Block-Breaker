using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mkey;
using UnityEngine.UI;

namespace MkeyFW
{
    public class Instantiator : MonoBehaviour
    {
        public GameObject[] prefabs;
        public EaseAnim ease;
        private int current = 0;
        private GameObject currentGo;
        private GameObject oldGo;
        private Button[] buttons;

        private void Start()
        {
            Create();
        }

        public void CreateNext()
        {
            if (prefabs == null || prefabs.Length == 0) return;
            current++;
            if (current >= prefabs.Length) current = 0;
            Create();
        }

        public void CreatePrev()
        {
            if (prefabs == null || prefabs.Length == 0) return;
            current--;
            if (current < 0) current = prefabs.Length - 1;
            Create();
        }

        private void Create()
        {
            if (prefabs[current])
            {
                SetControlInteractable(false);
                if (currentGo) { oldGo = currentGo; Destroy(oldGo); }
                currentGo = Instantiate(prefabs[current]);
                SimpleTween.Value(gameObject, 0, 1, 0.25f)
                    .SetOnUpdate((float val) => { if (currentGo) currentGo.transform.localScale = new Vector3(val, val, val); })
                    .SetEase(ease);

                SimpleTween.Value(gameObject, 0, 1, 0.5f).AddCompleteCallBack(() =>
                {
                    SetControlInteractable(true);
                    if (currentGo) currentGo.GetComponent<WheelController>().StartSpin();
                });
            }
        }

        private void SetControlInteractable(bool interactable)
        {
            buttons = GetComponentsInChildren<Button>();
            foreach (var item in buttons)
            {
                if (item) item.interactable = interactable;
            }
        }
    }
}