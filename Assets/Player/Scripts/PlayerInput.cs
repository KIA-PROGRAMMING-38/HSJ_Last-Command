using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;
using UnityEngine.UIElements;

public class PlayerInput : MonoBehaviour
{
    private Animator _animator;
    public Direction _moveDirection { get; private set; }

    [SerializeField] private float _dashWaitTime;
    private float _lastDashTime;
    private bool _isDashNoticed;
    
    private void Awake()
    {
        _animator = gameObject.GetComponent<Animator>();
        _isDashNoticed = false;
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _moveDirection = Direction.Right;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _moveDirection = Direction.Left;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _moveDirection = Direction.Up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _moveDirection = Direction.Down;
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
