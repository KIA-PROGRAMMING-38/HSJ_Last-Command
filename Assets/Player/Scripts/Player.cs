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

public class Player : MonoBehaviour
{
    public ObjectManager _objectManager { get; private set; }

    [SerializeField] private GameObject _snakeSprite;
    private int _defaultLength;
    [SerializeField] private int _maxLength;
    public int _currentLength;
    [SerializeField] private int _frameDelay;

    private GameObject[] _energies;
    private Stack<bool> _energy;
    private Animator _animator;
    private PlayerAnalyze _analyze;
    IObjectPool<LostEnergy> _pool;
    [SerializeField] private LostEnergy _lostEnergy;

    public event Action OnOverclock;
    public event Action OnOverclockEnd;
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
        for (int i = _maxLength; i > _defaultLength; --i)
        {
            transform.GetChild(i - 2).gameObject.SetActive(false);
        }
    }
    public void Init(ObjectManager objectManager)
    {
        _objectManager = objectManager;
    }
    public void EarnEnergy()
    {
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
        for(int i = 0; i < random; ++i)
        {
            LostEnergy lostEnergy = _pool.Get();
        }
    }

    private void Overclock()
    {
        if (!_isOverclocking)
        {
            OnOverclock?.Invoke();
            _isOverclocking = true;
            Debug.Log("오버클럭!");
        }
    }
    public void EndOverclock()
    {
        if (_isOverclocking)
        {
            OnOverclockEnd?.Invoke();
            _isOverclocking = false;
            Debug.Log("오버클럭 해제");
        }
    }

    public void Invincible()
    {
        StartCoroutine("OnInvincible");
    }

    IEnumerator OnInvincible()
    {
        Debug.Log("무적 시작");
        gameObject.layer = LayerMask.NameToLayer("Invincible");
        yield return new WaitForSeconds(_invincibleTime);
        Debug.Log("무적 끝");
        gameObject.layer = LayerMask.NameToLayer("Player");
    }

    public void Damaged()
    {
        _Hp--;
        if(_Hp <= 0)
        {
            Die();
            return;
        }
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
}
