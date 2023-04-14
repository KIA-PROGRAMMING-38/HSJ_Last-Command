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
    private float _lastDashTime;
    private bool _isDashNoticed;
  
    private void Awake()
    {
        _playerDirection = _right;
        _animator = gameObject.GetComponent<Animator>();
        _isDashNoticed = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _playerDirection = _right;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _playerDirection = _left;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _playerDirection = _up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _playerDirection = _down;
        }

        if (Input.GetKey(KeyCode.D))
        {
            _animator.SetBool("isAnalyzing", true);
        }
        else
        {
            _animator.SetBool("isAnalyzing", false);
        }

        if (Time.time > _lastDashTime + _dashWaitTime && !_isDashNoticed)
        {
            Debug.Log("대시 준비완료!");
            _isDashNoticed = true;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (Time.time > _lastDashTime + _dashWaitTime)
            {
                _lastDashTime = Time.time;
                _animator.SetBool("isDashing", true);
                _isDashNoticed = false;
            }
        }
    }
}
