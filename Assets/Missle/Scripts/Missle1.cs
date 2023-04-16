using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Missle1 : Missle
{
    private void FixedUpdate()
    {
        transform.position += (Vector3)(Vector2.left * Time.fixedDeltaTime);
    }

    protected override void ReturnMissle()
    {
        _currentPool.Release(this);
    }
}