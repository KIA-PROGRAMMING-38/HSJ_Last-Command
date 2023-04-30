using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SpreadEffect : Effect
{
    private IObjectPool<SpreadEffect> _currentPool;
    private Transform _startTransform;

    private Vector2 _targetPosition;
    private IEnumerator _effectMove;
    [SerializeField] private float _radius;

    [SerializeField] private float _lerpSpeed;
    private float _moveTime;
    private float _distance;

    private void OnEnable()
    {
        if(_startTransform != null)
        {
            transform.position = _startTransform.position;
        }
        _targetPosition = (Vector2)transform.position + (Random.insideUnitCircle.normalized * _radius);
        _distance = Vector2.Distance(transform.position, _targetPosition);
        _moveTime = _distance / _lerpSpeed;
        _effectMove = EffectMove();
        StartCoroutine(_effectMove);
    }

    public void SetPool(IObjectPool<SpreadEffect> pool, Transform transform)
    {
        _currentPool = pool;
        _startTransform = transform;
    }
    private void ReturnEffect()
    {
        _currentPool.Release(this);
    }
    IEnumerator EffectMove()
    {
        _targetPosition = (Vector2)transform.position + (Random.insideUnitCircle.normalized * _radius);
        while (Vector2.Distance(transform.position, _targetPosition) >= 0.1f)
        {
            float ratio = Mathf.Clamp01(Time.deltaTime / _moveTime);
            transform.position = Vector3.Lerp(transform.position, _targetPosition, ratio);
            yield return null;
        }
        ReturnEffect();
        yield return null;
    }
}
