using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDie : MonoBehaviour
{
    public event Action OnAnimationFinished;
    public void OnBossDie()
    {
        OnAnimationFinished?.Invoke();
    }
}
