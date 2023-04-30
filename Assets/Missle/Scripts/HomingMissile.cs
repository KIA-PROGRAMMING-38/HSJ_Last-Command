using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class HomingMissile : Missile
{
    private float _speed;
    private Transform _playerTransform;
    private Vector2 _targetPosition;
    private Vector2 _homingDirection;
    private float _elapsedTime;
    private bool _isReturned;
    private void OnEnable()
    {
        _speed = 30;
        if(_playerTransform != null)
        {
            _targetPosition = (Vector2)_playerTransform.position + Random.insideUnitCircle;
        }
        else
        {
            _targetPosition = (Vector2)GameObject.FindObjectOfType<Player>().transform.position + Random.insideUnitCircle;
        }
        _homingDirection = _targetPosition - (Vector2)transform.position;
        _homingDirection.Normalize();
        float angle = Mathf.Atan2(_homingDirection.y, _homingDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.GetChild(0).gameObject.SetActive(true);
        _elapsedTime = 0;
        transform.localScale = new Vector2(transform.localScale.x, 0);
        _isReturned = false;
    }
    
    private void FixedUpdate()
    {
        if(_elapsedTime < 0.6f)
        {
            _elapsedTime += Time.deltaTime;
            transform.localScale = new Vector2(transform.localScale.x, _elapsedTime / 0.6f);
        }
        else
        {
            transform.position += (Vector3)_homingDirection * Time.fixedDeltaTime * _speed;
        }
    }

    protected override void ReturnMissle()
    {
        if(!_isReturned)
        {
            _currentPool.Release(this);
            _isReturned = true;
        }
    }

    public void SetTarget(Transform playerTransform)
    {
        _playerTransform = playerTransform;
        _targetPosition = (Vector2)_playerTransform.position;
    }
}
