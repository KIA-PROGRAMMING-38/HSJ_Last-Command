using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakingMissle : Missile
{
    private float tense = 2f;
    private float frequency = 4f;
    private Vector3 _yDirection;

    private void OnEnable()
    {
        _yDirection = transform.TransformDirection(Vector3.up);
    }
    void FixedUpdate()
    {
        float yMovement = Mathf.Sin(Time.time * frequency) * tense;
        transform.position += _direction * Time.fixedDeltaTime + _yDirection * yMovement * Time.fixedDeltaTime;
    }

    protected override void ReturnMissle()
    {
        _currentPool.Release(this);
    }
}
