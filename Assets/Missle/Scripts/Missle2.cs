using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missle2 : Missle
{
    private float tense = 2f;
    private float frequency = 4f;
    private Vector3 _xDirection;
    private Vector3 _yDirection;

    private void OnEnable()
    {
        _xDirection = transform.TransformDirection(Vector3.left);
        _yDirection = transform.TransformDirection(Vector3.up);
    }
    void FixedUpdate()
    {
        float yMovement = Mathf.Sin(Time.time * frequency) * tense;
        transform.position += _xDirection * Time.fixedDeltaTime + _yDirection * yMovement * Time.fixedDeltaTime;
    }

    protected override void ReturnMissle()
    {
        _currentPool.Release(this);
    }
}
