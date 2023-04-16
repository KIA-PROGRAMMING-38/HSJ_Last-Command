using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Missle1 : Missle
{
    private Vector3 _direction;
    private void OnEnable()
    {
        _direction = transform.TransformDirection(Vector3.left);
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