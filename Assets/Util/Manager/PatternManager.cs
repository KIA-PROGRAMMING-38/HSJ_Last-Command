using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using System;

public class PatternManager : MonoBehaviour
{
    public GameManager _gameManager { get; private set; }

    private ObjectPool<Missile> _pool;
    private List<Missile> _currentMissles = new List<Missile>();
    private IEnumerator[] _pattern;
    [SerializeField] private Transform[][] _patternSpawnPoint;
    [SerializeField] private GameObject[] _misslePrefabs;
    [SerializeField] private float[] _spawnTimes;
    private float _spawnTime;
    private int _currentPattern;

    public event Action OnBossMove;
    public event Action OnBossAttack;
    public event Action OnBlockPatternStart;
    public event Action OnWheelPatternStart;

    private Transform _bossTransform;
    private Transform _playerTransform;
    public void Init(GameManager gameManager)
    {
        _gameManager = gameManager;
    }
    private void Awake()
    {
        _currentPattern = 0;
        _pattern = new IEnumerator[6];
        SetTransform();
        InitiatePattern();
        _pattern[0] = BossAttackPattern();
        _pattern[1] = DefaultPattern();
        _pattern[2] = CreateBlockPattern();
        _pattern[3] = CreateWheelPattern();
        _pattern[4] = BossMovePattern();
        _spawnTime = _spawnTimes[0];
    }

    private void InitiatePattern()
    {
        _pool = new ObjectPool<Missile>(StartPattern, OnGetMissle, OnReleaseMissle, OnDestroyMissle, maxSize: 20);
    }
    private Missile StartPattern()
    {
        int randomPosition = UnityEngine.Random.Range(0, _patternSpawnPoint[_currentPattern].Length);
        Missile missle = Instantiate(_misslePrefabs[_currentPattern], _patternSpawnPoint[_currentPattern][randomPosition].position, _patternSpawnPoint[_currentPattern][randomPosition].rotation).GetComponent<Missile>();
        missle.SetPool(_pool);
        return missle;
    }
    private void OnGetMissle(Missile missle)
    {
        int randomPosition = UnityEngine.Random.Range(0, _patternSpawnPoint[_currentPattern].Length);
        missle.gameObject.transform.position = _patternSpawnPoint[_currentPattern][randomPosition].position;
        missle.SetDirection(Quaternion.Euler(_patternSpawnPoint[_currentPattern][randomPosition].eulerAngles) * Vector2.left);
        missle.gameObject.SetActive(true);
        _currentMissles.Add(missle);
    }

    private void OnReleaseMissle(Missile missle)
    {
        missle.gameObject.SetActive(false);
        _currentMissles.Remove(missle);
    }
    private void OnDestroyMissle(Missile missle)
    {
        Destroy(missle.gameObject);
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
        OnBlockPatternStart?.Invoke();
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
        _patternSpawnPoint[_currentPattern] = new Transform[1];
        _patternSpawnPoint[_currentPattern][0] = _bossTransform;
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
                OnBossAttack?.Invoke();
                _patternSpawnPoint[_currentPattern][0] = _bossTransform;
            }
            if(bossElapsedTime > bossMoveTime)
            {
                OnBossMove?.Invoke();
                bossElapsedTime = 0;
            }
            yield return null;
        }
    }
    IEnumerator CreateWheelPattern()
    {
        OnWheelPatternStart?.Invoke();
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
        _patternSpawnPoint[_currentPattern] = new Transform[1];
        _patternSpawnPoint[_currentPattern][0] = _bossTransform;
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
                OnBossAttack?.Invoke();
                _patternSpawnPoint[_currentPattern][0] = _bossTransform;
            }
            yield return null;
        }
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

    private void SetTransform()
    {
        _patternSpawnPoint = new Transform[gameObject.transform.childCount - 1][];
        int childId = 0;
        for (int i = 0; i < gameObject.transform.childCount - 1; ++i)
        {
            Transform Pattern = transform.GetChild(childId);
            _patternSpawnPoint[childId] = new Transform[Pattern.childCount];
            for (int j = 0; j < Pattern.childCount; ++j)
            {
                _patternSpawnPoint[childId][j] = Pattern.GetChild(j);
            }
            ++childId;
        }
    }
    public void DestroyMissiles()
    {
        StopCoroutine(_pattern[_currentPattern]);
        _pool.Clear();
        foreach (Missile missle in _currentMissles)
        {
            Destroy(missle.gameObject);
        }
        _currentMissles.Clear();
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
}
