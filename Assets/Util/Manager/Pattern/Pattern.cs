using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using System;

public class Pattern : MonoBehaviour
{
    public StageManager _stageManager { get; private set; }

    protected ObjectPool<Missile> _pool;
    private List<Missile> _currentMissiles = new List<Missile>();

    protected IEnumerator[] _pattern;
    protected Transform[][] _spawnPoint;
    protected GameObject[] _missilePrefabs;
    protected float[] _spawnTimes;
    protected float _spawnTime;
    protected int _currentPattern;
    protected Transform _bossTransform;
    protected Transform _playerTransform;

    public event Action OnBossMove;
    public event Action OnBossAttack;
    public event Action OnBlockPatternStart;
    public event Action OnWheelPatternStart;
    public void Init(StageManager gameManager)
    {
        _stageManager = gameManager;
    }

    protected void InitiatePattern()
    {
        _pool = new ObjectPool<Missile>(StartPattern, OnGetMissile, OnReleaseMissile, OnDestroyMissile, maxSize: 20);
    }

    private Missile StartPattern()
    {
        int randomPosition = UnityEngine.Random.Range(0, _spawnPoint[_currentPattern].Length);
        Missile missile = Instantiate(_missilePrefabs[_currentPattern], _spawnPoint[_currentPattern][randomPosition].position, _spawnPoint[_currentPattern][randomPosition].rotation).GetComponent<Missile>();
        missile.SetPool(_pool);
        return missile;
    }

    private void OnGetMissile(Missile missile)
    {
        int randomPosition = UnityEngine.Random.Range(0, _spawnPoint[_currentPattern].Length);
        missile.gameObject.transform.position = _spawnPoint[_currentPattern][randomPosition].position;
        missile.SetDirection(Quaternion.Euler(_spawnPoint[_currentPattern][randomPosition].eulerAngles) * Vector2.left);
        missile.gameObject.SetActive(true);
        _currentMissiles.Add(missile);
    }

    private void OnReleaseMissile(Missile missile)
    {
        missile.gameObject.SetActive(false);
        _currentMissiles.Remove(missile);
    }

    private void OnDestroyMissile(Missile missile)
    {
        Destroy(missile.gameObject);
    }

    public void ChangePattern()
    {
        StopCoroutine(_pattern[_currentPattern]);
        DestroyMissiles();
        ++_currentPattern;
        _spawnTime = _spawnTimes[_currentPattern];
        InitiatePattern();
        StartCoroutine(_pattern[_currentPattern]);
    }

    protected void SetTransform()
    {
        _spawnPoint = new Transform[gameObject.transform.childCount - 1][];
        int childId = 0;
        for (int i = 0; i < gameObject.transform.childCount - 1; ++i)
        {
            Transform Pattern = transform.GetChild(childId);
            _spawnPoint[childId] = new Transform[Pattern.childCount];
            for (int j = 0; j < Pattern.childCount; ++j)
            {
                _spawnPoint[childId][j] = Pattern.GetChild(j);
            }
            ++childId;
        }
    }
    public void DestroyMissiles()
    {
        StopCoroutine(_pattern[_currentPattern]);
        _pool.Clear();
        foreach (Missile missile in _currentMissiles)
        {
            Destroy(missile.gameObject);
        }
        _currentMissiles.Clear();
    }
    public void StartGame()
    {
        StartCoroutine(_pattern[_currentPattern]);
    }
    public void SetTransform(Transform bossTransform, Transform playerTransform)
    {
        _bossTransform = bossTransform;
        _playerTransform = playerTransform;
    }
    protected void OnAttack()
    {
        OnBossAttack?.Invoke();
    }
    protected void OnMove()
    {
        OnBossMove?.Invoke();
    }
    protected void OnCreateBlock()
    {
        OnBlockPatternStart?.Invoke();
    }
    protected void OnCreateWheel()
    {
        OnWheelPatternStart?.Invoke();
    }
}
