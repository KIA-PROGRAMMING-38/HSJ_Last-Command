using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour
{
    private Vector2 _targetPosition;
    public int _frameDelay { set; private get; }
    public Transform _frontHead { set; private get; }
    private Queue<Vector2> _expectedPaths;
    private bool _isDead = false;

    private Player _player;
    private GameObject _invincible;
    void OnEnable()
    {
        if(_player == null)
        {
            _player = GetComponentInParent<Player>();
            _player.OnInvincibleStart -= Invincible;
            _player.OnInvincibleStart += Invincible;
            _player.OnInvincibleEnd -= EndInvincible;
            _player.OnInvincibleEnd += EndInvincible;
        }
        if (_expectedPaths == null)
        {
            _expectedPaths = new Queue<Vector2>();
        }
        if (_frontHead != null)
        {
            _targetPosition = _frontHead.position;
        }
    }


    void FixedUpdate()
    {
        if(!_isDead)
        {
            _expectedPaths.Enqueue(_frontHead.position);
            if (_expectedPaths.Count > _frameDelay)
            {
                _targetPosition = _expectedPaths.Dequeue();
            }


            transform.position = _targetPosition;
        }
    }

    private void OnDisable()
    {
        _targetPosition = _frontHead.position;
        transform.localPosition = Vector2.zero;
        ClearPath();
    }

    private void OnDestroy()
    {
        if(_player != null)
        {
            _player.OnInvincibleStart -= Invincible;
            _player.OnInvincibleEnd -= EndInvincible;
        }
    }
    public void ClearPath()
    {
        _expectedPaths?.Clear();
        _targetPosition = _frontHead.position;
    }
    public void ManipulateLocation()
    {
        if (_expectedPaths.Count > 0)
        {
            transform.position = (Vector3)_expectedPaths.Dequeue();
        }
    }
    public void Die()
    {
        _isDead = true;
    }
    public void Invincible()
    {
        if(_invincible == null)
        {
            _invincible = transform.GetChild(0).gameObject;
        }
        _invincible.SetActive(true);
    }
    public void EndInvincible()
    {
        if (_invincible == null)
        {
            _invincible = transform.GetChild(0).gameObject;
        }
        _invincible.SetActive(false);
    }
}
