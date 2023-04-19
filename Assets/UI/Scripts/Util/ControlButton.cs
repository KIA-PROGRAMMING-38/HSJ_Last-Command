using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ControlButton : Button, IControllable
{
    protected SettingMenu _settingMenu;
    public abstract void OnClickLeft();
    public abstract void OnClickRight();
    public void Init(SettingMenu settingMenu)
    {
        _settingMenu = settingMenu;
    }
}
