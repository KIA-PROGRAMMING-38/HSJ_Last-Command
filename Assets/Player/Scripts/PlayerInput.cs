using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Util.Direction;
using Util.Enum;
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
    private bool _isDashNotReadyNoticed;

    [SerializeField] private GameObject _dashWaitBoxPrefab;
    private GameObject _dashWaitBox;
    public event Action OnDirectionChanged;
    public event Action OnDashReady;
    public event Action<float, float> OnWaitDash;
    public event Action OnDashUsed;
  
    private void Awake()
    {
        _playerDirection = _right;
        _animator = gameObject.GetComponent<Animator>();
        _isDashNoticed = false;
        _isDashNotReadyNoticed = true;
        _elapsedTime = 0;
        _dashWaitBox = Instantiate(_dashWaitBoxPrefab, transform);
        _dashWaitBox.SetActive(false);
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
            SoundManager.instance.Play(SoundID.PlayerDashRestore);
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
                SoundManager.instance.Play(SoundID.PlayerDash);
                _elapsedTime = 0;
                OnDashUsed?.Invoke();
                _animator.SetBool("isDashing", true);
                _isDashNoticed = false;
                _isDashNotReadyNoticed = true;
            }
            else if(_isDashNotReadyNoticed)
            {
                _dashWaitBox.SetActive(true);
                _isDashNotReadyNoticed = false;
            }
        }
    }
}
