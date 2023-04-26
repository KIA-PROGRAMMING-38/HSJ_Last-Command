using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using System;

public class PatternManager : MonoBehaviour
{
    public GameManager _gameManager { get; private set; }

    private ObjectPool<Missle> _pool;
    private List<Missle> _currentMissles = new List<Missle>();
    private IEnumerator[] _pattern;
    [SerializeField] private Transform[][] _patternSpawnPoint;
    [SerializeField] private GameObject[] _misslePrefabs;
    [SerializeField] private float[] _spawnTimes;
    private float _spawnTime;
    private int _currentPattern;

    public event Action OnBossAttack;
    public event Action OnBlockPatternStart;

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
        for(int i = 0; i < 2; ++i)
        {
            _pattern[i] = Pattern();
        }
        _pattern[2] = CreateBlockPattern();
        for (int i = 3; i < _pattern.Length; ++i)
        {
            _pattern[i] = BossMovePattern();
        }
        _spawnTime = _spawnTimes[0];
    }

    private void InitiatePattern()
    {
        _pool = new ObjectPool<Missle>(StartPattern, OnGetMissle, OnReleaseMissle, OnDestroyMissle, maxSize: 20);
    }
    private Missle StartPattern()
    {
        int randomPosition = UnityEngine.Random.Range(0, _patternSpawnPoint[_currentPattern].Length);
        Missle missle = Instantiate(_misslePrefabs[_currentPattern], _patternSpawnPoint[_currentPattern][randomPosition].position, _patternSpawnPoint[_currentPattern][randomPosition].rotation).GetComponent<Missle>();
        missle.SetPool(_pool);
        return missle;
    }
    private void OnGetMissle(Missle missle)
    {
        int randomPosition = UnityEngine.Random.Range(0, _patternSpawnPoint[_currentPattern].Length);
        missle.gameObject.transform.position = _patternSpawnPoint[_currentPattern][randomPosition].position;
        missle.SetDirection(Quaternion.Euler(_patternSpawnPoint[_currentPattern][randomPosition].eulerAngles) * Vector2.left);
        missle.gameObject.SetActive(true);
        _currentMissles.Add(missle);
    }

    private void OnReleaseMissle(Missle missle)
    {
        missle.gameObject.SetActive(false);
        _currentMissles.Remove(missle);
    }
    private void OnDestroyMissle(Missle missle)
    {
        Destroy(missle.gameObject);
    }

    IEnumerator Pattern()
    {
        WaitForSeconds spawnTimer = new WaitForSeconds(_spawnTime);
        while (true)
        {
            Missle missle = _pool.Get();
            yield return spawnTimer;
        }
    }

    IEnumerator CreateBlockPattern()
    {
        OnBlockPatternStart?.Invoke();
        WaitForSeconds spawnTimer = new WaitForSeconds(_spawnTime);
        while (true)
        {
            Missle missle = _pool.Get();
            yield return spawnTimer;
        }
    }
    IEnumerator BossMovePattern()
    {
        float elapsedTime = 0;
        float bossElapsedTime = 0;
        float bossMoveTime = 5;
        while (true)
        {
            elapsedTime += Time.deltaTime;
            bossElapsedTime += Time.deltaTime;
            if (elapsedTime >= _spawnTime)
            {
                Missle missle = _pool.Get();
                elapsedTime = 0;
            }
            if(bossElapsedTime > bossMoveTime)
            {
                OnBossAttack?.Invoke();
                bossElapsedTime = 0;
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
        foreach (Missle missle in _currentMissles)
        {
            Destroy(missle.gameObject);
        }
        _currentMissles.Clear();
    }
    public void StartGame()
    {
        StartCoroutine(_pattern[_currentPattern]);
    }
}
