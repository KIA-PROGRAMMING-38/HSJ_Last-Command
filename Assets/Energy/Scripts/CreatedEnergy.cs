using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class CreatedEnergy : Energy
{
    public event Func<Transform> OnRespawn;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            transform.position = OnRespawn.Invoke().position;
            gameObject.SetActive(true);
        }
    }
}
