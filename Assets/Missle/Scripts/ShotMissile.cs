using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ShotMissile : Missile
{
    IObjectPool<Missile> _subPool;
    private List<MissileFragment> _currentSubMissle = new List<MissileFragment>();
    private float _elapsedTime;
    private float _explosionTime = 2;
    public int _angle { get; private set; }
    [SerializeField] private GameObject _subMissle;
    private float _speed;
    private void OnEnable()
    {
        _elapsedTime = 0;
        InitiatePattern();
        _speed = Random.Range(3, 6);
    }
    private void FixedUpdate()
    {
        if (_elapsedTime >= _explosionTime)
        {
            for(int i = 0; i < 8; ++ i)
            {
                MissileFragment subMissile = _subPool.Get() as MissileFragment;
                subMissile.transform.position = transform.position;
                subMissile.SetDirection(Quaternion.Euler(0, 0, i * 45f) * Vector3.left);
                _currentSubMissle.Add(subMissile);
            }
            ReturnMissle();
        }
        else
        {
            transform.position += _direction * Time.fixedDeltaTime * _speed;
            _elapsedTime += Time.deltaTime;
        }
    }
    protected override void ReturnMissle()
    {
        _currentPool.Release(this);
    }


    private void InitiatePattern()
    {
        if (_subPool == null)
        {
            _subPool = new ObjectPool<Missile>(StartSubPattern, OnGetMissle, OnReleaseMissle, OnDestroyMissle, maxSize: 100);
        }
    }
    private Missile StartSubPattern()
    {
        Missile missle = Instantiate(_subMissle).GetComponent<Missile>();
        ++_angle;
        missle.SetPool(_subPool as IObjectPool<Missile>);
        return missle;
    }
    private void OnGetMissle(Missile missle)
    {
        missle.transform.position = transform.position;
        ++_angle;
        missle.gameObject.SetActive(true);
    }

    private void OnReleaseMissle(Missile missle)
    {
        missle.gameObject.SetActive(false);
        _currentSubMissle.Remove(missle as MissileFragment);
    }
    private void OnDestroyMissle(Missile missle)
    {
        Destroy(missle.gameObject);
    }

    private void OnDestroy()
    {
        _subPool.Clear();
        foreach (Missile missle in _currentSubMissle)
        {
            Destroy(missle.gameObject);
        }
        _currentSubMissle.Clear();

    }
}
