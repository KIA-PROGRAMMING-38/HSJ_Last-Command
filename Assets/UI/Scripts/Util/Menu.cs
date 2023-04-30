using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Util.Enum;

public abstract class Menu : MonoBehaviour
{
    private int _currentID;
    private int _menuNum;
    protected abstract Button[] _menuButtons { get; }
    private Text[] _menuText;
    private RectTransform[] _menuTransform;
    private GameObject _select;

    protected void Awake()
    {
        _currentID = 0;
        _menuNum = _menuButtons.Length;
        _menuText = new Text[_menuNum];
        _menuTransform = new RectTransform[_menuNum];
        
        for (int i = 0; i < _menuNum; ++i)
        {
            _menuTransform[i] = _menuButtons[i].gameObject.GetComponent<RectTransform>();
            _menuText[i] = _menuButtons[i].GetComponent<Text>();
        }
        _select = transform.Find("Select").gameObject;
    }

    protected virtual void Update()
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
            SoundManager.instance.Play(SoundID.UIClick);
            _menuButtons[_currentID].onClick.Invoke();
        }
        if (_menuText[0] != null)
        {
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
        }
        _select.GetComponent<RectTransform>().position = _menuTransform[_currentID].position;
    }

    public int GetMenuId()
    {
        return _currentID;
    }
}
