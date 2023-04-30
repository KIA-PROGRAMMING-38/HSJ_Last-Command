using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundButton : ControlButton
{
    private GameObject[] _backGrounds;
    private Text _text;
    private int _backgroundCount = 4;
    private int _currentId = 0;
    protected override void Awake()
    {
        _backGrounds = new GameObject[_backgroundCount];
        _text = transform.GetChild(0).GetComponent<Text>();
        for(int i = 0; i < _backgroundCount; ++i)
        {
            _backGrounds[i] = _settingMenu._mainMenu.transform.GetChild(_backgroundCount - 1 - i).gameObject;
            _backGrounds[i].SetActive(false);
        }

        _backGrounds[_currentId].SetActive(true);
    }
    public override void OnClickLeft()
    {
        _backGrounds[_currentId].SetActive(false);
        if(_currentId == 0)
        {
            _backGrounds[_backgroundCount - 1].SetActive(true);
            _currentId = _backgroundCount - 1;
        }
        else
        {
            _currentId--;
            _backGrounds[_currentId].SetActive(true);
        }
        _text.text = $"Background {_currentId + 1}";
    }
    public override void OnClickRight()
    {
        _backGrounds[_currentId].SetActive(false);
        if (_currentId == _backgroundCount - 1)
        {
            _backGrounds[0].SetActive(true);
            _currentId = 0;
        }
        else
        {
            _currentId++;
            _backGrounds[_currentId].SetActive(true);
        }
        _text.text = $"Background {_currentId + 1}";
    }
}
