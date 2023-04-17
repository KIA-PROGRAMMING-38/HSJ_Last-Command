using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Missle3 : Missle
{
    IObjectPool<Missle> _subPool;
    private Vector3 _direction;
    private float _elapsedTime;
    private float _explosionTime = 2;
    public int _angle { get; private set; }
    [SerializeField] private GameObject _subMissle;
    private void OnEnable()
    {
        _direction = transform.TransformDirection(Vector3.left);
        _elapsedTime = 0;
        _angle = 1;
        InitiatePattern();
    }
    private void FixedUpdate()
    {
        if (_elapsedTime >= _explosionTime)
        {
            for(int i = 0; i < 8; ++ i)
            {
                Missle subMissle = _subPool.Get();
            }
            ReturnMissle();
        }
        else
        {
            transform.position += _direction * Time.fixedDeltaTime * 2f;
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
            _subPool = new ObjectPool<Missle>(StartSubPattern, OnGetMissle, OnReleaseMissle, OnDestroyMissle, maxSize: 100);
        }
    }
    private Missle StartSubPattern()
    {
        Missle missle = Instantiate(_subMissle).GetComponent<Missle>();
        ++_angle;
        missle.SetPool(_subPool);
        return missle;
    }
    private void OnGetMissle(Missle missle)
    {
        missle.transform.position = transform.position;
        ++_angle;
        missle.gameObject.SetActive(true);
    }

    private void OnReleaseMissle(Missle missle)
    {
        missle.gameObject.SetActive(false);
    }
    private void OnDestroyMissle(Missle missle)
    {
        Destroy(missle.gameObject);
    }
}
