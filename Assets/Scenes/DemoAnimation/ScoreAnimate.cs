using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class ScoreAnimate : MonoBehaviour
{
    bool isAlphaUpdate;
    // Start is called before the first frame update
    void OnEnable()
    {
        var tran = GetComponent<RectTransform>() as Transform;
        tran.DOLocalMoveY(5, 1f).From(0);
        tran.DOScale(2, 1f).From(1).OnUpdate(
            () =>
            {
                if (tran.localScale.x > 1.5f && !isAlphaUpdate)
                {
                    isAlphaUpdate = true;
                    tran.GetComponent<TextMeshProUGUI>().DOFade(0, 0.5f);
                    Debug.Log("InProgress");
                }
                else
                {
                    Debug.Log("**************************");
                }
            }).OnComplete(() => { gameObject.SetActive(false); tran.GetComponent<TextMeshProUGUI>().DOFade(1, 0.05f); });
    }
    private void OnDisable()
    {
        isAlphaUpdate = false;
    }


}
