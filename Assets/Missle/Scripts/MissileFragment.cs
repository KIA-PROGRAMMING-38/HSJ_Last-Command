using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileFragment : Missile
{
    private float _speed = 5f;

    private void FixedUpdate()
    {
        transform.position += _direction * Time.fixedDeltaTime * _speed;
    }

    protected override void ReturnMissle()
    {
        _currentPool.Release(this);
    }
}
