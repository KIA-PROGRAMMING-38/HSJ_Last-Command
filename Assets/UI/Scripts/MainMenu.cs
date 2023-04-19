using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : Menu
{
    [SerializeField] private Button[] _mainMenuButtons;
    [SerializeField] private GameObject[] _mainMenuObjects;
    [SerializeField] private GameObject _settings;
    private SettingMenu _settingMenu;

    protected override Button[] _menuButtons { get { return _mainMenuButtons; } }

    public void StartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void GoToSettings()
    {
        CleanMenu();
        if (_settingMenu == null)
        {
            _settingMenu = _settings.GetComponent<SettingMenu>();
            _settingMenu.Init(this);
        }
        _settings.SetActive(true);
        this.enabled = false;
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void CleanMenu()
    {
        foreach (GameObject gameObject in _mainMenuObjects)
        {
            gameObject.SetActive(false);
        }
    }
    public void GetMenu()
    {
        foreach (GameObject gameObject in _mainMenuObjects)
        {
            gameObject.SetActive(true);
        }
    }
}
