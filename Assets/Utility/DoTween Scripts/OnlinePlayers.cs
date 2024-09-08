using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlinePlayers : MonoBehaviour
{
    [SerializeField] private Transform _playersContainer;
    private List<GameObject> _playersList;
    private Tween _tween;

    // Start is called before the first frame update
    void Start()
    {
        _playersList.Clear();
        foreach (Transform item in _playersContainer)
        {
            _playersList.Add(item.gameObject);
        }
    }

    private void OnEnable()
    {
        StartAnimation();
    }

    private void OnDisable()
    {
        StopAnimation();
    }

    private void StartAnimation()
    {
        foreach (var item in _playersList)
        {
            item.transform.localScale = Vector3.zero;

        }
    }

    private void StopAnimation()
    {

    }

}
