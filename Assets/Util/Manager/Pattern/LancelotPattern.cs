using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LancelotPattern : Pattern
{

    [SerializeField] private GameObject[] _lancelotMisslePrefabs;
    [SerializeField] private float[] _lancelotSpawnTimes;

    private void Awake()
    {
        _currentPattern = 0;
        _pattern = new IEnumerator[6];
        SetTransform();
        InitiatePattern();
        _missilePrefabs = _lancelotMisslePrefabs;
        _spawnTimes = _lancelotSpawnTimes;
        _pattern[0] = BossAttackPattern();
        _pattern[1] = DefaultPattern();
        _pattern[2] = CreateBlockPattern();
        _pattern[3] = CreateWheelPattern();
        _pattern[4] = BossMovePattern();
        _spawnTime = _spawnTimes[0];
    }

    IEnumerator DefaultPattern()
    {
        WaitForSeconds spawnTimer = new WaitForSeconds(_spawnTime);
        yield return new WaitForSeconds(2);
        while (true)
        {
            Missile missle = _pool.Get();
            yield return spawnTimer;
        }
    }

    IEnumerator CreateBlockPattern()
    {
        OnCreateBlock();
        WaitForSeconds spawnTimer = new WaitForSeconds(_spawnTime);
        yield return new WaitForSeconds(2);
        while (true)
        {
            Missile missle = _pool.Get();
            yield return spawnTimer;
        }
    }
    IEnumerator BossMovePattern()
    {
        float elapsedTime = 0;
        float bossElapsedTime = 0;
        float bossMoveTime = _spawnTime * 2;
        _spawnPoint[_currentPattern] = new Transform[1];
        _spawnPoint[_currentPattern][0] = _bossTransform;
        yield return new WaitForSeconds(2);
        while (true)
        {
            elapsedTime += Time.deltaTime;
            bossElapsedTime += Time.deltaTime;
            WaitForSeconds wait = new WaitForSeconds(0.5f);
            if (elapsedTime >= _spawnTime)
            {
                for (int i = 0; i < 3; ++i)
                {
                    HomingMissile missle = _pool.Get() as HomingMissile;
                    missle.SetTarget(_playerTransform);
                    yield return wait;
                }
                elapsedTime = 0;
                OnAttack();
                _spawnPoint[_currentPattern][0] = _bossTransform;
            }
            if (bossElapsedTime > bossMoveTime)
            {
                OnMove();
                bossElapsedTime = 0;
            }
            yield return null;
        }
    }
    IEnumerator CreateWheelPattern()
    {
        OnCreateWheel();
        WaitForSeconds spawnTimer = new WaitForSeconds(_spawnTime);
        yield return new WaitForSeconds(2);
        while (true)
        {
            Missile missle = _pool.Get();
            yield return spawnTimer;
        }
    }
    IEnumerator BossAttackPattern()
    {
        float elapsedTime = 0;
        float bossElapsedTime = 0;
        _spawnPoint[_currentPattern] = new Transform[1];
        _spawnPoint[_currentPattern][0] = _bossTransform;
        yield return new WaitForSeconds(2);
        while (true)
        {
            elapsedTime += Time.deltaTime;
            bossElapsedTime += Time.deltaTime;
            WaitForSeconds wait = new WaitForSeconds(0.5f);
            if (elapsedTime >= _spawnTime)
            {
                for (int i = 0; i < 3; ++i)
                {
                    HomingMissile missle = _pool.Get() as HomingMissile;
                    missle.SetTarget(_playerTransform);
                    yield return wait;
                }
                elapsedTime = 0;
                OnAttack();
                _spawnPoint[_currentPattern][0] = _bossTransform;
            }
            yield return null;
        }
    }
}
