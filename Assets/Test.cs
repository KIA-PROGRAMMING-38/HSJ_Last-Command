using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public BoxCollider2D _collider;

    private void Awake()
    {
        _collider = gameObject.GetComponent<BoxCollider2D>();
        _collider.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.gameObject.GetComponent<Player>().Damaged();
        }
    }
    public void PrepareAttack()
    {
        GetComponent<Animator>().SetTrigger("isAttack");
        _collider.enabled = true;
    }
}
