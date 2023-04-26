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

public class Player : MonoBehaviour
{
    public ObjectManager _objectManager { get; private set; }

    [SerializeField] private GameObject _snakeSprite;
    public int _defaultLength;
    [SerializeField] private int _maxLength;
    public int maxLength { get { return _maxLength; } }
    public int _currentLength;
    [SerializeField] private int _frameDelay;

    private GameObject[] _energies;
    private Stack<bool> _energy;
    private Animator _animator;
    private PlayerAnalyze _analyze;
    IObjectPool<LostEnergy> _pool;
    [SerializeField] private LostEnergy _lostEnergy;
    public event Action<int> OnEarnEnergy;

    public event Action OnOverclock;
    public event Action OnOverclockEnd;
    [SerializeField] private Color _overclockColor;
    [SerializeField] private GameObject _overclockEffectPrefab;
    private GameObject _overclockEffect;
    private bool _isOverclocking;

    [SerializeField] private float _invincibleTime;
    [SerializeField] private int _Hp;
    public event Action OnHpDecrease;
    public event Action OnDie;
    public event Action OnDieEnd;

    [SerializeField] private GameObject _dieParticle;
    IEnumerator _dieEffectTimer;

    private TrailRenderer _trail;
    private SpriteRenderer[] _energySprites;

    [SerializeField] private GameObject _hurtEffect;
    private GameObject[] _hurtParticle;

    [SerializeField] private GameObject _dashUIPrefab;
    private GameObject _dashUI;

    [SerializeField] private GameObject _overclockBoxPrefab;
    [SerializeField] private GameObject _earnEnergyBoxPrefab;
    private GameObject _overclockBox;
    private GameObject _earnEnergyBox;

    [SerializeField] private GameObject _invinciblePrefab;
    private GameObject _invincible;
    public event Action OnInvincibleStart;
    public event Action OnInvincibleEnd;

    [SerializeField] private GameObject _analyzeBoxPrefab;
    private GameObject _analyzeBox;

    [SerializeField] private GameObject _startPointPrefab;
    private GameObject _startPoint;
    [SerializeField] private GameObject _startTextPrefab;
    private GameObject _startText;
    public event Action OnGameStart;
    private void Awake()
    {
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
        _dieEffectTimer = PlayerDieEffect();
        _trail = GetComponent<TrailRenderer>();
        _hurtParticle = new GameObject[3];
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
        _overclockEffect = Instantiate(_overclockEffectPrefab, transform);
        ChangeColor(Color.gray);
        _overclockEffect.SetActive(false);
        _dashUI = Instantiate(_dashUIPrefab, transform);
        for (int i = _maxLength; i > _defaultLength; --i)
        {
            transform.GetChild(i - 2).gameObject.SetActive(false);
        }
        _hurtParticle[0] = Instantiate(_hurtEffect);
        _hurtParticle[1] = _hurtParticle[0].transform.GetChild(0).gameObject;
        _hurtParticle[2] = _hurtParticle[0].transform.GetChild(1).gameObject;
        for(int i = 1; i < 3; ++i)
        {
            _hurtParticle[i].SetActive(false);
        }
        for (int i = 1; i < _maxLength; ++i)
        {
            _invincible = Instantiate(_invinciblePrefab, _energies[i].transform);
            _invincible.SetActive(false);
        }
        _invincible = Instantiate(_invinciblePrefab, transform);
        _invincible.SetActive(false);

        _overclockBox = Instantiate(_overclockBoxPrefab, transform);
        _overclockBox.SetActive(false);
        _earnEnergyBox = Instantiate(_earnEnergyBoxPrefab, transform);
        _earnEnergyBox.SetActive(false);
        _earnEnergyBox.name = "Earn Energy Box";
        _analyzeBox = Instantiate(_analyzeBoxPrefab, transform);
        _analyzeBox.name = "Analyzing Box";
        _analyzeBox.SetActive(false);
        _startPoint = Instantiate(_startPointPrefab);
        StartCoroutine(StartTimer());
        
    }

    private void OnDestroy()
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
            if(_animator.GetBool("isAnalyzing") == true)
            {
                _analyze.EarnEnergy();
            }
            else
            {
                transform.GetChild(_currentLength - 2).gameObject.SetActive(true);
                OnEarnEnergy?.Invoke(_currentLength - 3);
                _earnEnergyBox.SetActive(true);
            }
            _energy.Push(true);
            Debug.Log(_currentLength);
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
        for(int i = 0; i < random; ++i)
        {
            LostEnergy lostEnergy = _pool.Get();
        }
    }

    private void Overclock()
    {
        if (!_isOverclocking)
        {
            SoundManager.instance.Play(SoundID.PlayerOverclock);
            _overclockBox.SetActive(true);
            OnOverclock?.Invoke();
            _overclockEffect.SetActive(true);
            ChangeColor(_overclockColor);
            _isOverclocking = true;
        }
    }
    public void EndOverclock()
    {
        if (_isOverclocking)
        {
            OnOverclockEnd?.Invoke();
            _overclockEffect.SetActive(false);
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
        _invincible.SetActive(true);
        OnInvincibleStart?.Invoke();
        yield return new WaitForSeconds(_invincibleTime);
        gameObject.layer = LayerMask.NameToLayer("Player");
        _invincible.SetActive(false);
        OnInvincibleEnd?.Invoke();
    }

    public void Damaged()
    {
        _Hp--;
        if(_Hp <= 0)
        {
            SoundManager.instance.Play(SoundID.PlayerDead);
            Die();
            return;
        }
        for (int i = 1; i < 3; ++i)
        {
            _hurtParticle[i].transform.position = transform.position;
            _hurtParticle[i].SetActive(true);
        }
        SoundManager.instance.Play(SoundID.PlayerDamage);
        SpreadEnergy();
        ApplyDamage();
    }

    public int HP()
    {
        return _Hp;
    }

    IEnumerator PlayerDieEffect()
    {
        WaitForSeconds wait = new WaitForSeconds(0.3f);

        for(int i = 1; i <= _energies.Length; ++i)
        {
            if(_energies[_energies.Length - i].activeSelf)
            {
                Instantiate(_dieParticle, _energies[_energies.Length - i].transform.position, _energies[_energies.Length - i].transform.rotation);
                _energies[_energies.Length - i].SetActive(false);
                if (i == _energies.Length)
                {
                    OnDieEnd?.Invoke();
                }
                yield return wait;
            }
            else
            {
                yield return null;
            }
            
        }
    }

    private void InitiatePool()
    {
        if(_pool == null)
        {
            _pool = new ObjectPool<LostEnergy>(SpawnEnergy,OnGetEnergy,OnReleaseEnergy,OnDestroyEnergy, maxSize: _maxLength);
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
        StartCoroutine(_dieEffectTimer);
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

    public void EnableDashTrail()
    {
        StartCoroutine(DashTrail());
    }

    IEnumerator DashTrail()
    {
        _trail.enabled = true;
        foreach(SpriteRenderer sprites in _energySprites)
        {
            sprites.enabled = false;
        }
        yield return new WaitForSeconds(_trail.time);
        foreach (SpriteRenderer sprites in _energySprites)
        {
            sprites.enabled = true;
        }
        _trail.enabled = false;
    }

    private void ChangeColor(Color color)
    {
        for (int i = _defaultLength; i < _maxLength; ++i)
        {
            _energies[i].GetComponent<SpriteRenderer>().color = color;
        }
    }

    public void SetAnalyzeBoxTrue() => _analyzeBox.SetActive(true);
    public void SetAnalyzeBoxFalse() => _analyzeBox.SetActive(false);

    IEnumerator StartTimer()
    {
        GetComponent<PlayerInput>().enabled = false;
        GetComponent<PlayerMovement>().enabled = false;
        Invincible();
        yield return new WaitForSeconds(2);
        GetComponent<PlayerInput>().enabled = true;
        GetComponent<PlayerMovement>().enabled = true;
        _startPoint.SetActive(false);
        _startText = Instantiate(_startTextPrefab);
        Invincible();
        OnGameStart?.Invoke();
    }

}
