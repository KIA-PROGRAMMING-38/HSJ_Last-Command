using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using Util.Enum;

public class StageManager : MonoBehaviour
{
    public event Action OnGameClear;
    public event Action<float, Rank, int> OnGameClearUI;
    public event Action OnGameOver;

    public GameManager _gameManager { get; private set; }
    public float _playTime { get; private set; }
    public Rank _rank { get; private set; }
    public int _hitTime { get; private set; }
    private bool _isCleared;
    private void Awake()
    {
        _hitTime = 0;
        _playTime = 0;
        _rank = Rank.D;
    }

    private void Update()
    {
        if(!_isCleared)
        {
            _playTime += Time.deltaTime;
        }
    }
    public void GameClear()
    {
        _isCleared = true;
        SetRank();
        OnGameClearUI?.Invoke(_playTime, _rank, _hitTime);
        OnGameClear?.Invoke();
    }

    public void GameOver()
    {
        OnGameOver?.Invoke();
    }
    public void Init(GameManager gameManager)
    {
        _gameManager = gameManager;
    }
    public void AddHitCount() => ++_hitTime;
    private void SetRank()
    {
        if(_playTime < 120f && _hitTime == 0)
        {
            _rank = Rank.A;
        }
        else if(_playTime < 150f && _hitTime < 3)
        {
            _rank = Rank.B;
        }
        else if(_playTime < 180f && _hitTime < 7)
        {
            _rank = Rank.C;
        }
        else
        {
            _rank = Rank.D;
        }
    }

    public void StopTime()
    {
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        Time.timeScale = 0;
        float startTime = Time.realtimeSinceStartup;
        while(Time.realtimeSinceStartup - startTime < 0.3f)
        {
            yield return null;
        }
        Time.timeScale = 1;
        yield return null;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
