using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public event Action OnGameClear;

    public GameManager _gameManager { get; private set; }
    
    public void GameClear()
    {
        OnGameClear?.Invoke();
        Time.timeScale = 0f;
        Debug.Log("게임 클리어!");
    }

    public void GameOver()
    {
        Debug.Log("게임 오버!");
    }
    public void Init(GameManager gameManager)
    {
        _gameManager = gameManager;
    }
}
