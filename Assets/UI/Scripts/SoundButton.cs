using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SoundButton : ControlButton
{
    private Image[] _box;
    private int _defaultId = 4;
    private int _boxLength = 8;
    private int _currentId;
    protected override void Awake()
    {
        _box = new Image[_boxLength];
        for(int i = 0; i < _boxLength; ++i)
        {
            _box[i]= transform.GetChild(i).GetComponent<Image>();
        }
        _currentId = _defaultId;
        for (int i = 0; i <= _defaultId; ++i)
        {
            _box[i].color = Color.white;
        }
        for(int i = _defaultId + 1; i < _boxLength; ++i)
        {
            _box[i].color = Color.gray;
        }
    }
    public override void OnClickLeft()
    {
        if(_currentId >= 0)
        {
            _box[_currentId].color = Color.gray;
            _currentId--;
        }
    }
    public override void OnClickRight()
    {
        if (_currentId < _boxLength - 1)
        {
            _currentId++;
            _box[_currentId].color = Color.white;
        }
    }
}
