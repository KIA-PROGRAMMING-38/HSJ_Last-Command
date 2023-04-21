using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Util.Direction;

public class PlayerInput : MonoBehaviour
{
    private Animator _animator;
    public Direction _playerDirection { get; private set; }

    private Right _right = new Right();
    private Left _left = new Left();
    private Up _up = new Up();
    private Down _down = new Down();

    [SerializeField] private float _dashWaitTime;
    private float _elapsedTime;
    private bool _isDashNoticed;

    public event Action OnDirectionChanged;
    public event Action OnDashReady;
    public event Action<float, float> OnWaitDash;
    public event Action OnDashUsed;
  
    private void Awake()
    {
        _playerDirection = _right;
        _animator = gameObject.GetComponent<Animator>();
        _isDashNoticed = false;
        _elapsedTime = 0;
    }

    private void Update()
    {
        _elapsedTime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _playerDirection = _right;
            OnDirectionChanged?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _playerDirection = _left;
            OnDirectionChanged?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _playerDirection = _up;
            OnDirectionChanged?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _playerDirection = _down;
            OnDirectionChanged?.Invoke();
        }

        if (Input.GetKey(KeyCode.D))
        {
            _animator.SetBool("isAnalyzing", true);
        }
        else
        {
            _animator.SetBool("isAnalyzing", false);
        }

        if (_elapsedTime >= _dashWaitTime && !_isDashNoticed)
        {
            Debug.Log("대시 준비완료!");
            OnDashReady?.Invoke();
            _isDashNoticed = true;
        }
        else if(_elapsedTime < _dashWaitTime)
        {
            OnWaitDash?.Invoke(_elapsedTime, _dashWaitTime);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (_elapsedTime > _dashWaitTime)
            {
                _elapsedTime = 0;
                OnDashUsed?.Invoke();
                _animator.SetBool("isDashing", true);
                _isDashNoticed = false;
            }
        }
    }
}
