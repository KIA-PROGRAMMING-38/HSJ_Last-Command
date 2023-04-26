using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    private ObjectManager _objectManager;

    private Player _player;
    private Boss _boss;

    private Vector2 _waypointPosition;
    private float _waypointRadius;

    private float _lerpSpeed1;
    private float _lerpSpeed2;
    private float _moveTime;
    private float _distance;

    private float _coroutineWaitTime;
    private IEnumerator _bulletMove;
    private TrailRenderer _trail;

    private IObjectPool<Bullet> _currentPool;
    void Awake()
    {
        if(_boss == null || _player == null)
        {
            InitialSettings();
        }
    }
    private void OnEnable()
    {
        transform.position = (Vector2)_player.transform.position;
        _waypointPosition = (Vector2)_player.transform.position + (Random.insideUnitCircle.normalized * _waypointRadius);

        ResetSettings(_waypointPosition, _lerpSpeed1);

        _bulletMove = BulletMove();
        StartCoroutine(_bulletMove);
    }
    IEnumerator BulletMove()
    {
        _trail.Clear();
        _trail.enabled = true;
        while(Vector2.Distance(transform.position,_waypointPosition) >= 0.1f)
        {
            float ratio = Mathf.Clamp01(Time.deltaTime / _moveTime);
            transform.position = Vector3.Lerp(transform.position, _waypointPosition, ratio);
            yield return null;
        }

        yield return new WaitForSeconds(_coroutineWaitTime);

        while (Vector2.Distance(transform.position, (Vector2)_boss.transform.position) >= 0.2f)
        {
            ResetSettings(_boss.transform.position, _lerpSpeed2);
            float ratio = Mathf.Clamp01(Time.deltaTime / _moveTime);
            transform.position = Vector3.Lerp(transform.position, _boss.transform.position, ratio);
            yield return null;
        }
        ReturnBullet();
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
    private void InitialSettings()
    {
        _objectManager = gameObject.GetComponentInParent<Player>()._objectManager;
        _boss = _objectManager._boss;
        _player = _objectManager._player;
        _waypointRadius = 1;
        _lerpSpeed1 = 20f;
        _lerpSpeed2 = 50f;
        _coroutineWaitTime = 0.2f;
        transform.SetParent(default);
        _trail = GetComponent<TrailRenderer>();
    }

    public void SetPool(IObjectPool<Bullet> pool)
    {
        _currentPool = pool;
    }
    private void ReturnBullet()
    {
        _currentPool.Release(this);
    }
    public void Init(ObjectManager objectManager)
    {
        _objectManager = objectManager;
    }
}
