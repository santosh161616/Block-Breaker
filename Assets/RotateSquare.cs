using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RotateSquare : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject _square1, _square2, _square3;
    float speed = 50f;
    float duration = 10f;

    [SerializeField] private Text _timeTxt;

    private UnityEvent OnSquareOneComplete = new UnityEvent();
    private UnityEvent OnSquareTwoComplete = new UnityEvent();
    private UnityEvent OnSquareThreeComplete = new UnityEvent();

    private void Start()
    {
        InitilizeObjectListener();
    }

    private void Update()
    {
        if (duration > 0)
        {
            duration -= Time.deltaTime;
            DisplayTime(duration);

            if (duration < 7f && duration > 5f)
                OnSquareOneComplete.Invoke();
            if (duration < 5f && duration > 3f)
                OnSquareTwoComplete.Invoke();
            else if (duration < 3f && duration > 0f)
                OnSquareThreeComplete.Invoke();

            if (duration < 0)
                duration = 7;
        }
    }

    void InitilizeObjectListener()
    {
        OnSquareOneComplete.AddListener(() => { RotateObj(_square1, duration); });
        OnSquareTwoComplete.AddListener(() => { RotateObj(_square2, duration); });
        OnSquareThreeComplete.AddListener(() => { RotateObj(_square3, duration); });
    }
    private void RotateObj(GameObject square, float duration)
    {
        square.transform.Rotate(0f, 0f, 1f * Time.deltaTime * speed, Space.World);

        square.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.red, Color.blue, Mathf.PingPong(Time.time, 1));
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minute = Mathf.Floor(timeToDisplay / 60f);
        float sec = Mathf.Floor(timeToDisplay % 60f);

        _timeTxt.text = string.Format("{0:00}:{1:00}", minute, sec);
    }
}
