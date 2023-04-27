using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelWarning : Warning
{
    private GameObject _wheel;
    protected override void AfterWarning()
    {
        gameObject.SetActive(false);
        _wheel.SetActive(true);
    }

    public void SetWheel(GameObject wheel)
    {
        _wheel = wheel;
    }
}
