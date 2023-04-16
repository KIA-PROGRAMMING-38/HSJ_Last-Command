using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missle2 : Missle
{
    float tense = 2f;
    float frequency = 4f;

    void FixedUpdate()
    {
        float yMovement = Mathf.Sin(Time.time * frequency) * tense;
        transform.position += (Vector3)(Vector2.right * Time.fixedDeltaTime + Vector2.up * yMovement * Time.fixedDeltaTime);
    }

    protected override void ReturnMissle()
    {
        _currentPool.Release(this);
    }
}
