using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    private int _currentID;
    private int _menuNum;
    [SerializeField] private Button[] _menuButtons;
    private Text[] _menuText;
    private GameObject _select;

    private void Awake()
    {
        _currentID = 0;
        _menuNum = _menuButtons.Length;
        _menuText = new Text[_menuNum];
        for (int i = 0; i < _menuNum; ++i)
        {
            _menuText[i] = _menuButtons[i].GetComponent<Text>();
        }
        _select = transform.Find("Select").gameObject;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _currentID++;
            if (_currentID >= _menuNum)
            {
                _currentID = 0;
            }
            _menuButtons[_currentID].Select();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _currentID--;
            if (_currentID < 0)
            {
                _currentID = _menuNum - 1;
            }

            _menuButtons[_currentID].Select();
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            _menuButtons[_currentID].onClick.Invoke();
        }
        for (int i = 0; i < _menuNum; ++i)
        {
            if (i == _currentID)
            {
                _menuText[i].color = Color.white;
            }
            else
            {
                _menuText[i].color = Color.gray;
            }
        }
        _select.GetComponent<RectTransform>().position = _menuText[_currentID].rectTransform.position;
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
    public void Exit()
    {
        Application.Quit();
    }
}
