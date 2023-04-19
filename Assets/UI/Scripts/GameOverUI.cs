using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : Menu
{
    [SerializeField] private Button[] _gameOverButtons;
    protected override Button[] _menuButtons { get { return _gameOverButtons; } }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
    public void Exit()
    {
        Application.Quit();
    }
}
