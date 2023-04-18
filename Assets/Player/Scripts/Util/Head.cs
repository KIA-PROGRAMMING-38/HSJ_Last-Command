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

    void OnEnable()
    {
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
        _expectedPaths.Enqueue(_frontHead.position);
        if (_expectedPaths.Count > _frameDelay)
        {
            _targetPosition = _expectedPaths.Dequeue();
        }

        if(!_isDead)
        {
            transform.position = _targetPosition;
        }
    }

    private void OnDisable()
    {
        ClearPath();
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
}
