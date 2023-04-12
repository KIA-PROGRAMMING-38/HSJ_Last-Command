using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector2 _bossPosition;
    private Vector2 _waypointPosition;
    private float _waypointRadius;
    
    private float _lerpSpeed1;
    private float _lerpSpeed2;
    private float _moveTime;
    private float _distance;

    private float _coroutineWaitTime;
    private IEnumerator _bulletMove;

    void Awake()
    {
        GameObject Boss = GameObject.FindWithTag("Boss");
        GameObject Player = GameObject.FindWithTag("Player");
        _waypointRadius = 1;

        _bossPosition = (Vector2)Boss.transform.position;
        _waypointPosition = (Vector2)Player.transform.position + (Random.insideUnitCircle.normalized * _waypointRadius);

        _lerpSpeed1 = 20f;
        _lerpSpeed2 = 50f;

        ResetSettings(_waypointPosition, _lerpSpeed1);

        _coroutineWaitTime = 0.5f;
        _bulletMove = BulletMove();
        StartCoroutine(_bulletMove);
    }

    IEnumerator BulletMove()
    {
        WaitForSeconds wait = new WaitForSeconds(_coroutineWaitTime);
        while(Vector2.Distance(transform.position,_waypointPosition) >= 0.1f)
        {
            float ratio = Mathf.Clamp01(Time.deltaTime / _moveTime);
            transform.position = Vector3.Lerp(transform.position, _waypointPosition, ratio);
            yield return null;
        }

        yield return wait;
        ResetSettings(_bossPosition, _lerpSpeed2);

        while (Vector2.Distance(transform.position, _bossPosition) >= 0.1f)
        {
            float ratio = Mathf.Clamp01(Time.deltaTime / _moveTime);
            transform.position = Vector3.Lerp(transform.position, _bossPosition, ratio);
            yield return null;
        }

        gameObject.SetActive(false);
    }

    void ResetSettings(Vector2 targetPosition, float lerpSpeed)
    {
        _distance = Vector3.Distance(transform.position, targetPosition);
        _moveTime = _distance / lerpSpeed;
    }

    private void OnDisable()
    {
        StopCoroutine(_bulletMove);
    }
}
