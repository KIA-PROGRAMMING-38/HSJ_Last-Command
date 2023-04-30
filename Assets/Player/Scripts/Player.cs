using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using System;
using System.Runtime.CompilerServices;
using Random = UnityEngine.Random;
using UnityEditor;
using UnityEngine.Pool;
using static UnityEngine.ParticleSystem;
using UnityEngine.UIElements;
using Util.Enum;
using Unity.VisualScripting;

public class Player : MonoBehaviour
{
    public ObjectManager _objectManager { get; private set; }
    private PlayerEffect _playerEffect;

    [SerializeField] private GameObject _snakeSprite;
    public int _defaultLength;
    [SerializeField] private int _maxLength;
    public int maxLength { get { return _maxLength; } }
    public int _currentLength;
    [SerializeField] private int _frameDelay;
    private SpriteRenderer[] _energySprites;
    private GameObject[] _energies;
    private Stack<bool> _energy;
    private Animator _animator;
    private PlayerAnalyze _analyze;
    IObjectPool<LostEnergy> _pool;
    [SerializeField] private LostEnergy _lostEnergy;
    public event Action<int> OnEarnEnergy;
    public event Action OnEnergyBoxActivated;

    public event Action OnOverclock;
    public event Action OnOverclockEnd;
    [SerializeField] private Color _overclockColor;
    private bool _isOverclocking;

    [SerializeField] private float _invincibleTime;
    [SerializeField] private int _MaxHp;
    private int _Hp;

    public event Action OnHpDecrease;
    public event Action OnHpIncrease;
    public event Action OnDie;
    public event Action OnDieEnd;
    public event Action OnInvincibleStart;
    public event Action OnInvincibleEnd;
    public event Action OnGameStart;
    private void Awake()
    {
        _Hp = _MaxHp;
        _defaultLength = 3;
        _currentLength = _defaultLength;
        _energies = new GameObject[_maxLength];
        _energySprites = new SpriteRenderer[_maxLength];
        _energies[0] = gameObject;
        _energySprites[0] = gameObject.GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _analyze = _animator.GetBehaviour<PlayerAnalyze>();
        InitiatePool();
        _isOverclocking = false;
        _energy = new Stack<bool>();
        for (int i = 1; i < _maxLength; ++i)
        {
            GameObject newHead = Instantiate(_snakeSprite, transform);
            newHead.name = $"Head{i}";
            _energies[i] = newHead;
            _energySprites[i] = newHead.GetComponent<SpriteRenderer>();
            Head headComponent = newHead.AddComponent<Head>();
            headComponent._frameDelay = _frameDelay;
            headComponent._frontHead = _energies[i - 1].transform;
        }
        ChangeColor(Color.gray);
        for (int i = _maxLength; i > _defaultLength; --i)
        {
            transform.GetChild(i - 2).gameObject.SetActive(false);
        }
        _playerEffect = GetComponent<PlayerEffect>();
        _playerEffect.Init(this, _energySprites, _energies);
        StartCoroutine(StartTimer());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    public void Init(ObjectManager objectManager)
    {
        _objectManager = objectManager;
    }
    public void EarnEnergy()
    {
        SoundManager.instance.Play(SoundID.PlayerEarnEnergy);
        if (_currentLength < _maxLength)
        {
            _currentLength++;
            if (_animator.GetBool("isAnalyzing") == true)
            {
                _analyze.EarnEnergy();
            }
            else
            {
                transform.GetChild(_currentLength - 2).gameObject.SetActive(true);
                OnEarnEnergy?.Invoke(_currentLength - 3);
                OnEnergyBoxActivated?.Invoke();
            }
            _energy.Push(true);
        }

        if (_currentLength == _maxLength)
        {
            Overclock();
        }
    }

    public bool IsEnergyCharged()
    {
        if (_energy.Count > 0)
        {
            return true;
        }
        return false;

    }

    public void UseEnergy()
    {
        _energy.Pop();
        _currentLength--;
    }
    private void SpreadEnergy()
    {
        int random = Random.Range(0, _currentLength - _defaultLength);
        for (int i = 0; i < random; ++i)
        {
            LostEnergy lostEnergy = _pool.Get();
        }
    }

    private void Overclock()
    {
        if (!_isOverclocking)
        {
            SoundManager.instance.Play(SoundID.PlayerOverclock);
            OnOverclock?.Invoke();
            ChangeColor(_overclockColor);
            _isOverclocking = true;
        }
    }
    public void EndOverclock()
    {
        if (_isOverclocking)
        {
            OnOverclockEnd?.Invoke();
            ChangeColor(Color.gray);
            _isOverclocking = false;
        }
    }

    public void Invincible()
    {
        StartCoroutine("OnInvincible");
    }

    IEnumerator OnInvincible()
    {
        gameObject.layer = LayerMask.NameToLayer("Invincible");
        OnInvincibleStart?.Invoke();
        yield return new WaitForSeconds(_invincibleTime);
        gameObject.layer = LayerMask.NameToLayer("Player");
        OnInvincibleEnd?.Invoke();
    }

    public void Damaged()
    {
        _Hp--;
        if (_Hp <= 0)
        {
            SoundManager.instance.Play(SoundID.PlayerDead);
            Die();
            return;
        }
        SoundManager.instance.Play(SoundID.PlayerDamage);
        SpreadEnergy();
        ApplyDamage();
    }

    public int HP() => _Hp;
    public void HealHP()
    {
        if (_Hp < _MaxHp)
        {
            ++_Hp;
            OnHpIncrease?.Invoke();
        }
    }
    private void InitiatePool()
    {
        if (_pool == null)
        {
            _pool = new ObjectPool<LostEnergy>(SpawnEnergy, OnGetEnergy, OnReleaseEnergy, OnDestroyEnergy, maxSize: _maxLength);
        }
    }

    private LostEnergy SpawnEnergy()
    {
        LostEnergy lostEnergy = Instantiate(_lostEnergy, transform).GetComponent<LostEnergy>();
        lostEnergy.SetPool(_pool);
        return lostEnergy;
    }
    private void OnGetEnergy(LostEnergy lostEnergy)
    {
        lostEnergy.gameObject.SetActive(true);
    }

    private void OnReleaseEnergy(LostEnergy lostEnergy)
    {
        lostEnergy.gameObject.SetActive(false);
    }
    private void OnDestroyEnergy(LostEnergy lostEnergy)
    {
        Destroy(lostEnergy.gameObject);
    }

    private void Die()
    {
        GetComponent<PlayerMovement>().enabled = false;
        Head[] heads = GetComponentsInChildren<Head>();
        foreach (Head head in heads)
        {
            head.Die();
        }
        OnDie?.Invoke();
    }
    private void ApplyDamage()
    {
        for (int i = 0; i < _currentLength - _defaultLength; ++i)
        {
            _energies[_currentLength - i - 1].SetActive(false);
        }
        _currentLength = _defaultLength;
        _energy.Clear();
        OnHpDecrease?.Invoke();
        EndOverclock();
        Invincible();
    }

    private void ChangeColor(Color color)
    {
        for (int i = _defaultLength; i < _maxLength; ++i)
        {
            _energies[i].GetComponent<SpriteRenderer>().color = color;
        }
    }
    IEnumerator StartTimer()
    {
        GetComponent<PlayerInput>().enabled = false;
        GetComponent<PlayerMovement>().enabled = false;
        Invincible();
        yield return new WaitForSeconds(2);
        GetComponent<PlayerInput>().enabled = true;
        GetComponent<PlayerMovement>().enabled = true;
        Invincible();
        OnGameStart?.Invoke();
    }

    public void OnDieEffectEnd()
    {
        OnDieEnd?.Invoke();
    }
}
