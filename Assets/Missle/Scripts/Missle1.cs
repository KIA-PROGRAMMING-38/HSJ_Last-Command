using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Missle1 : Missle
{
    private float _speed;
    private void OnEnable()
    {
        _speed = Random.Range(3, 6);
    }
    private void FixedUpdate()
    {
        transform.position += _direction * Time.fixedDeltaTime * _speed;
    }

    protected override void ReturnMissle()
    {
        _currentPool.Release(this);
    }
}