using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubMissle3 : Missle
{
    private Vector3 _direction;
    private float _speed = 3f;

    private void FixedUpdate()
    {
        transform.position += _direction * Time.fixedDeltaTime * _speed;
    }

    public void SetDirection(Vector3 direction)
    {
        _direction = direction;
    }
    protected override void ReturnMissle()
    {
        _currentPool.Release(this);
    }
}
