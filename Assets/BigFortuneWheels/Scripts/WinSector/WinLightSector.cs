using Mkey;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLightSector : WinSectorBehavior
{
    private SpriteRenderer sR;
    ColorFlasher cF;
    #region override
    protected override void PlayWin()
    {
        base.PlayWin();
        sR = GetComponent<SpriteRenderer>();
        if (!sR)
        {
            return;
        }
        FlashAlpha();
      
    }

    protected override void Cancel()
    {
        base.Cancel();
        if (!this) return;
        if (cF != null) cF.Cancel();
        SimpleTween.Cancel(gameObject, false);
        if(sR) sR.color = new Color(1, 1, 1, 0);
    }
    #endregion override

    private void FlashAlpha()
    {
        cF = new ColorFlasher(gameObject, null, null, new SpriteRenderer[] {sR}, null, 1);
        cF.FlashingAlpha();
    }
}

/*
   old
   //if (tweenSeq != null)
        //{
        //    return;
        //}
        //tweenSeq = new TweenSeq();
        //float fadeTime = 0.2f;
        //float delay = 0.2f;
        //float stayTime = 0.2f;
        //EaseAnim ease = EaseAnim.EaseInOutSine;

        //for (int i = 0; i < 10; i++)
        //{
        //    tweenSeq.Add((callBack) =>  //fadein
        //    {
        //        SimpleTween.Value(gameObject, 0, 1, fadeTime)
        //            .SetOnUpdate((float val) => { if (this && sR) sR.color = new Color(1, 1, 1, val); })
        //            .SetDelay(delay)
        //            .SetEase(ease)
        //            .AddCompleteCallBack(callBack);
        //    });

        //    tweenSeq.Add((callBack) => //fadeout
        //    {
        //        SimpleTween.Value(gameObject, 1, 0, fadeTime)
        //            .SetOnUpdate((float val) => { if (this && sR) sR.color = new Color(1, 1, 1, val); })
        //            .SetDelay(stayTime)
        //            .SetEase(ease)
        //            .AddCompleteCallBack(callBack);
        //    });
        //}

        //tweenSeq.Add((callBack) =>
        //{
        //    tweenSeq = null;
        //    callBack?.Invoke();
        //});
        //tweenSeq.Start();
 */