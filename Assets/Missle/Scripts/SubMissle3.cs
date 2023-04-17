using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubMissle3 : Missle
{

    private Vector3 _direction;
    private Missle3 _missle3;
    private int _angle;
    private void OnEnable()
    {
        _direction = transform.TransformDirection(Vector3.left);
        transform.rotation = Quaternion.Euler(0,0,45 * _missle3._angle);
    }
    private void FixedUpdate()
    {
        transform.position += _direction * Time.fixedDeltaTime;
    }

    protected override void ReturnMissle()
    {
        _currentPool.Release(this);
    }
}
