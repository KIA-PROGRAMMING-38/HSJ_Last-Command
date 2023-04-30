using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Effect : MonoBehaviour
{
    private IObjectPool<Effect> _currentPool;

    public void SetPool(IObjectPool<Effect> objectPool)
    {
        _currentPool = objectPool;
    }
    public void OnAnimFinish()
    {
        _currentPool.Release(this);
    } 
}
