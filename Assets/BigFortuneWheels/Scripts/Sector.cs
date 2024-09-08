using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Events;

namespace MkeyFW
{
    [ExecuteInEditMode]
    public class Sector : MonoBehaviour
    {
        [SerializeField]
        private bool autoText = true;
        [SerializeField]
        private int coins;
        [SerializeField]
        private int id;
        [SerializeField]
        private bool bigWin;
        [SerializeField]
        private List<GameObject> hitPrefabs;
        private float destroyTime = 3f;
        [SerializeField]
        private UnityEvent hitEvent;

        [SerializeField]
        public AudioClip hitSound;
        [SerializeField]
        public SpriteRenderer coinSprite, emojiSprite;
        [SerializeField] private Texture2D loadingSpr, _defaultTexture;

        public TextMesh Text { get; private set; }

        public int Coins
        {
            get => coins;
            set { coins = Mathf.Max(0, value); RefreshText(); }
        }
        public int Id
        {
            get => id;
            set { id = value; }
        }

        public bool BigWin
        {
            get => bigWin;
            set => bigWin = value;
        }

        public string IconUrl;
        #region regular
        void Start()
        {
            Text = GetComponent<TextMesh>();
            RefreshText();

        }

        void OnValidate()
        {
            coins = Mathf.Max(0, coins);
            RefreshText();
        }
        #endregion regular

        private void RefreshText()
        {
            if (!autoText) return;
            if (!Text) Text = GetComponent<TextMesh>();
            if (!Text) return;
            var f = new NumberFormatInfo { NumberGroupSeparator = " " }; // Text.text = Coins.ToString("n0", f);
            //Enable disable Sprites
            if (coins == 0 && emojiSprite != null && coinSprite != null)
            {
                if (!string.IsNullOrWhiteSpace(IconUrl))
                {
                    Davinci.get()
                    .load(IconUrl)
                    .setLoadingPlaceholder(loadingSpr)
                    .setErrorPlaceholder(_defaultTexture)
                    .setCached(true)
                    .into(emojiSprite)
                    .start();
                }
                emojiSprite?.gameObject.SetActive(true);
                coinSprite?.gameObject.SetActive(false);
                Text.text = "";
            }
            else
            {
                if (emojiSprite != null && coinSprite != null)
                {
                    coinSprite?.gameObject.SetActive(true);
                    emojiSprite?.gameObject.SetActive(false);
                    Text.text = coins.ToString("N0", CultureInfo.CreateSpecificCulture("en-US"));
                }
            }

        }

        /// <summary>
        /// Instantiate all prefabs and invoke hit event
        /// </summary>
        /// <param name="position"></param>
        public void PlayHit(Vector3 position)
        {
            if (!bigWin) return;

            if (hitPrefabs != null)
            {
                foreach (var item in hitPrefabs)
                {
                    if (item)
                    {
                        Transform partT = Instantiate(item).transform;
                        partT.position = position;
                        if (this && partT) Destroy(partT.gameObject, destroyTime);
                    }
                }
            }
            hitEvent?.Invoke();
        }
    }

}