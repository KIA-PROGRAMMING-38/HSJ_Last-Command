using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Particle : MonoBehaviour
{
    private IObjectPool<Particle> _currentPool;

    public void SetPool(IObjectPool<Particle> objectPool)
    {
        _currentPool = objectPool;
    }

    public void OnAnimFinish()
    {
        _currentPool.Release(this);
    }
}
