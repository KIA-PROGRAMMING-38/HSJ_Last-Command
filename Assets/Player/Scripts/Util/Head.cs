using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour
{
    public Vector2 _targetPosition;
    public int _frameDelay;
    public Transform _frontHead;
    public Queue<Vector2> _expectedPaths;
    void Awake()
    {
        _expectedPaths = new Queue<Vector2>();
    }

    void FixedUpdate()
    {
        _expectedPaths.Enqueue(_frontHead.position);

        if(_expectedPaths.Count > _frameDelay)
        {
            _targetPosition = _expectedPaths.Dequeue();
        }

        transform.position = _targetPosition;
    }
}
