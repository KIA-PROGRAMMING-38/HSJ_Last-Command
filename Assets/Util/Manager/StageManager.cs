using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public GameManager _gameManager { get; private set; }
    
    public void GameClear()
    {
        Time.timeScale = 0f;
        Debug.Log("���� Ŭ����!");
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        Debug.Log("���� ����!");
    }
    public void Init(GameManager gameManager)
    {
        _gameManager = gameManager;
    }
}
