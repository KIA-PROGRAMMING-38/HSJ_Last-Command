using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Pool;

public class LostEnergy : Energy
{
    private IObjectPool<LostEnergy> _currentPool;
    private BoxCollider2D _collider; 
    private Player _player;
    private Vector3 _targetPosition;
    private float _waypointRadius = 2;
    private float _lerpSpeed = 10;
    private float _moveTime;
    private float _distance;

    private float _elapsedTime;
    private float _targetTime;
    private float _maxTargetTime = 5;
    private float _minTargetTime = 2;

    private IEnumerator _energyMoveTimer;
    private void OnEnable()
    {
        if(_player == null)
        {
            _player = GetComponentInParent<Player>();
            transform.SetParent(default);
            _collider = GetComponent<BoxCollider2D>();
        }
        transform.position = _player.transform.position;
        _targetPosition = (Vector2)transform.position + (Random.insideUnitCircle.normalized * _waypointRadius);
        _distance = Vector2.Distance(transform.position, _targetPosition);
        _targetTime = Random.Range(_minTargetTime, _maxTargetTime);
        _moveTime = _distance / _lerpSpeed;
        _energyMoveTimer = EnergyMove();
        _collider.enabled = false;
        StartCoroutine(_energyMoveTimer);
    }
    private void OnDisable()
    {
        StopCoroutine(_energyMoveTimer);
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            ReturnLostEnergy();
        }
    }
    private void FixedUpdate()
    {
        if(_elapsedTime >= _targetTime)
        {
            ReturnLostEnergy();
        }
        else
        {
            _elapsedTime += Time.fixedDeltaTime;
        }
    }
    IEnumerator EnergyMove()
    {
        while (Vector2.Distance(transform.position, _targetPosition) >= 0.1f)
        {
            float ratio = Mathf.Clamp01(Time.deltaTime / _moveTime);
            transform.position = Vector3.Lerp(transform.position, _targetPosition, ratio);
            yield return null;
        }
        _collider.enabled = true;
    }

    public void SetPool(IObjectPool<LostEnergy> pool)
    {
        _currentPool = pool;
    }

    private void ReturnLostEnergy()
    {
        _currentPool.Release(this);
    }

}
