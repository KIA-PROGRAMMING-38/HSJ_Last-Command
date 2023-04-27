using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPattern : MonoBehaviour
{
    private BoxCollider2D _collider;
    public BoxCollider2D bossCollider { get { return _collider; } }
    private Animator _animator;
    private Boss _boss;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _boss = GetComponentInParent<Boss>();
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
        _animator.SetTrigger("isAttack");
    }

    public void SetPosition()
    {
        _animator.SetBool("Attacked", true);
    }
    public void SetCollider() => _collider.enabled = true;
}
