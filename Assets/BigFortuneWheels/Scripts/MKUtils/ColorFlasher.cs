using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
  06.07.2020 - first
 */ 
namespace Mkey
{
    public class ColorFlasher
    {
        #region input
        private Dictionary<TextMesh, Color> rendTM;
        private Dictionary<Text, Color> rendT;
        private Dictionary<SpriteRenderer, Color> rendS;
        private Dictionary<Image, Color> rendI;
        private GameObject gameObject;
        private float period;
        #endregion input

        #region temp vars
        private bool useTM = false;
        private bool useT = false;
        private bool useS = false;
        private bool useI = false;
        private Color c;
        #endregion temp vars

        public ColorFlasher(GameObject gameObject, TextMesh[] textMeshes, Text[] texts, SpriteRenderer[] sprites, Image[] images, float period)
        {
            this.gameObject = gameObject;
            this.period = period;

            if (textMeshes != null && textMeshes.Length > 0)
            {
                rendTM = new Dictionary<TextMesh, Color>();
                foreach (var item in textMeshes)
                {
                    if (item)
                    {
                        rendTM.Add(item, item.color);
                    }
                }
                useTM = rendTM.Count > 0;
            }

            if (texts != null && texts.Length > 0)
            {
                rendT = new Dictionary<Text, Color>();
                foreach (var item in texts)
                {
                    if (item)
                    {
                        rendT.Add(item, item.color);
                    }
                }
                useT = rendT.Count > 0;
            }

            if (sprites != null && sprites.Length > 0)
            {
                rendS = new Dictionary<SpriteRenderer, Color>();
                foreach (var item in sprites)
                {
                    if (item)
                    {
                        rendS.Add(item, item.color);
                    }
                }
                useS = rendS.Count > 0;
            }

            if (images != null && images.Length > 0)
            {
                rendI = new Dictionary<Image, Color>();
                foreach (var item in images)
                {
                    if (item)
                    {
                        rendI.Add(item, item.color);
                    }
                }
                useI = rendI.Count > 0;
            }
        }

        private void UpdateColor(float multiplier, Func<Color, float, Color> getColor)
        {
            if (useTM)
                foreach (var item in rendTM)
                {
                    if (item.Key)
                    {
                        c = item.Value;
                        if (getColor != null) item.Key.color = getColor(c, multiplier);
                    }
                }
            if (useT)
                foreach (var item in rendT)
                {
                    if (item.Key)
                    {
                        c = item.Value;
                        if (getColor != null) item.Key.color = getColor(c, multiplier);
                    }
                }
            if (useS)
                foreach (var item in rendS)
                {
                    if (item.Key)
                    {
                        c = item.Value;
                        if (getColor != null) item.Key.color = getColor(c, multiplier);
                    }
                }
            if (useI)
                foreach (var item in rendI)
                {
                    if (item.Key)
                    {
                        c = item.Value;
                        if (getColor != null) item.Key.color = getColor(c, multiplier);
                    }
                }
        }

        /// <summary>
        /// Flashing color alpha channel
        /// </summary>
        public void FlashingAlpha()
        {
            Cancel();
            SimpleTween.Value(gameObject, 0, Mathf.PI * 2f, period).SetOnUpdate((float val) =>
            {
                float k = 0.5f * (Mathf.Cos(val) + 1f);
                UpdateColor(k, (sc, t) => { return new Color(sc.r, sc.g, sc.b, sc.a * t); });
            }).SetCycled();
        }

        /// <summary>
        /// Flashing update using func getColor, returned new color from source color and multiplier
        /// </summary>
        /// <param name="getColor"></param>
        public void Flashing(Func<Color, float, Color> getColor)
        {
            Cancel();
            SimpleTween.Value(gameObject, 0, Mathf.PI * 2f, period).SetOnUpdate((float val) =>
            {
                float k = 0.5f * (Mathf.Cos(val) + 1f);
                UpdateColor(k, getColor);
            }).SetCycled();
        }

        /// <summary>
        /// Cancel flashing, set source color
        /// </summary>
        public void Cancel()
        {
            SimpleTween.Cancel(gameObject, false);
            UpdateColor(1, (sc, t) => { return new Color(sc.r, sc.g, sc.b, sc.a); });
        }
    }
}
