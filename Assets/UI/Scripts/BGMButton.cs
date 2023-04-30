using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util.Enum;

public class BGMButton : SoundButton
{
    public override void OnClickLeft()
    {
        base.OnClickLeft();
        SoundManager.instance.ChangeVolume("BGMVolume", Mathf.Max(0.0001f, 0.125f * (_currentId + 1)));
    }

    public override void OnClickRight()
    {
        base.OnClickRight();
        SoundManager.instance.ChangeVolume("BGMVolume", 0.125f * (_currentId + 1));
    }
}
