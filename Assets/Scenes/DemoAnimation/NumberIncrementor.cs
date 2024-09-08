using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;
using DG.Tweening;

public class NumberIncrementor : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private int currentScore, finalScore;
    [SerializeField] float timeToIncrese;
    [SerializeField]
    float timeToWait
    {
        get
        {
            return timeToIncrese / Mathf.Abs(finalScore - currentScore);
        }
    }
    [SerializeField] bool scoreUpdated;

    public void UpdateScore()
    {
        if (currentScore < finalScore && !scoreUpdated)
        {
            scoreUpdated = true;
            var waitingTime = timeToWait;
            Debug.Log("delay Time - " + waitingTime);
            StartCoroutine(IncreaseScore(waitingTime));
        }
        else if (currentScore > finalScore && !scoreUpdated)
        {
            scoreUpdated = true;
            var waitingTime = timeToWait;
            Debug.Log("delay Time decrese - " + waitingTime);

            StartCoroutine(IncreaseScore(waitingTime));
        }
    }
    IEnumerator IncreaseScore(float waitingTime)
    {
        DOVirtual.Int(currentScore, finalScore, 1, (currentScore) => { scoreText.text = currentScore.ToString(); }).SetEase(Ease.Flash).OnComplete(() =>{ currentScore = finalScore; scoreUpdated = false; });
        //Debug.Log("Started Coroutine at timestamp : " + Time.time);
        //while (currentScore < finalScore)
        //{
        //    currentScore += 1;
        //    scoreText.text = currentScore.ToString();
        //    //await Task.Delay(timeToWait);
        //    yield return new WaitForSeconds(waitingTime);
        //}
        ////Debug.Log("Ended Coroutine at timestamp : " + Time.time);
        //scoreUpdated = false;
        yield break;
    }

    IEnumerator DecreaseScore(float waitingTime)
    {
        //Debug.Log("Started Coroutine at timestamp : " + Time.time);
        while (currentScore > finalScore)
        {
            currentScore -= 1;
            scoreText.text = currentScore.ToString();
            //await Task.Delay(timeToWait);
            yield return new WaitForSeconds(waitingTime);
        }
        //Debug.Log("Ended Coroutine at timestamp : " + Time.time);
        scoreUpdated = false;
        yield break;
    }
}
