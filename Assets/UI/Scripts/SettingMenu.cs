using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SettingMenu : Menu
{
    [SerializeField] private Button[] _optionButtons;
    
    public MainMenu _mainMenu { get; private set; }
    protected override Button[] _menuButtons { get { return _optionButtons; } }
    private ControlButton[] _menu;
    private void OnEnable ()
    {
        if(_menu == null)
        {
            _menu = new ControlButton[_optionButtons.Length];

            for (int i = 0; i < _menu.Length; ++i)
            {
                _menu[i] = _optionButtons[i] as ControlButton;
                _menu[i].Init(this);
            }
        }
    }

    protected override void Update()
    {
        base.Update();
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            _mainMenu.enabled = true;
            _mainMenu.GetMenu();
            gameObject.SetActive(false);
        }

        int currentId = GetMenuId();

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _menu[currentId].OnClickLeft();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _menu[currentId].OnClickRight();
        }
    }

    public void Init(MainMenu mainMenu)
    {
        _mainMenu = mainMenu;
    }

    public void ChangeMusicVolume()
    {
       
    }
    public void ChangeBackground()
    {

    }
}
