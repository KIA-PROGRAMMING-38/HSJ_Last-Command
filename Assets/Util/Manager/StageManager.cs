using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] private Boss _boss;
    [SerializeField] private Player _player;

    private void Awake()
    {
        if(_boss != null)
        {
            _boss.OnDie -= GameClear;
            _boss.OnDie += GameClear;
        }
        if(_player != null)
        {
            _player.OnDie -= GameOver;
            _player.OnDie += GameOver;
        }
    }

    private void OnDestroy()
    {
        if (_boss != null)
        {
            _boss.OnDie -= GameClear;
        }
        if (_player != null)
        {
            _player.OnDie -= GameOver;
        }
    }
    void GameClear()
    {
        Time.timeScale = 0f;
        Debug.Log("게임 클리어!");
    }

    void GameOver()
    {
        Time.timeScale = 0f;
        Debug.Log("게임 오버!");
    }
}
