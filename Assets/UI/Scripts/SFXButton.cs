using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util.Enum;

public class SFXButton : SoundButton
{
    public override void OnClickLeft()
    {
        base.OnClickLeft();
        SoundManager.instance.ChangeVolume("SFXVolume", Mathf.Max(0.0001f, 0.125f * (_currentId + 1)));
    }

    public override void OnClickRight()
    {
        base.OnClickRight();
        SoundManager.instance.ChangeVolume("SFXVolume", 0.125f * (_currentId + 1));
    }
}
