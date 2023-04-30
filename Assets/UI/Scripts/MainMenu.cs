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
    [SerializeField] private Image _panel;
    private SettingMenu _settingMenu;

    protected override Button[] _menuButtons { get { return _mainMenuButtons; } }

    public void StartGame()
    {
        StartCoroutine(FadeOut());
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
    private IEnumerator FadeOut()
    {
        float elapsedTime = 0;
        float fadeOutTime = 1;
        while(elapsedTime < fadeOutTime)
        {
            elapsedTime += Time.deltaTime;
            _panel.color = new Color(0, 0, 0, elapsedTime);
            yield return null;
        }
        SceneManager.LoadScene("InGame");
    }
}
