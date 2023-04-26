using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.TextCore.Text;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;
using Util.Enum;

public class Boss : MonoBehaviour
{
    public ObjectManager _objectManager { get; private set; }
    public int _temporaryDamageGain { get; private set; }
    public int _confirmedDamageGain { get; private set; }
    public int _totalDamageGain { get; private set; }

    [SerializeField] private float _damageTreshold;

    [SerializeField] private float _diminishingTime;
    private float _elapsedTime;
    private bool _isOnGroggy;

    [SerializeField] private int _Hp;

    public event Action OnAttackSuccess;

    public event Action<int, int, float> OnTempChange;
    public event Action<int, float> OnConfChange;
    public event Action OnDamageChange;
    public event Action OnGroggy;
    public event Action OnDie;

    private GameObject _groggyEffect;

    [SerializeField] private GameObject _damageEffect;
    [SerializeField] private GameObject _hitEffect;
    private IObjectPool<Effect> _pool;
    private IObjectPool<HitEffect> _hitPool;
    private int _hitEffectNum = 15;

    public Test _bossAttack { get; private set; }
    void Awake()
    {
        ResetSettings();
        InitPool();
        _bossAttack = GetComponentInChildren<Test>();
        _groggyEffect = transform.Find("BossGroggy").gameObject;
        _isOnGroggy = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (_temporaryDamageGain > 0)
            {
                SoundManager.instance.Play(SoundID.BossDamaged);
                ChangeDamageType();
            }
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Bullet") && !_isOnGroggy)
        {
            SoundManager.instance.Play(SoundID.BossDamaged);
            GetTempDamage();
        }

        if (_totalDamageGain >= _damageTreshold)
        {
            EnterGroggyState();
        }
    }

    private void Update()
    {
        if (_temporaryDamageGain > 0)
        {
            if (_elapsedTime >= _diminishingTime)
            {
                DecreaseTempDamage();
            }
            else
            {
                _elapsedTime += Time.deltaTime;
            }
        }
    }
    public void Damaged()
    {
        _Hp--;
        if (_Hp <= 0)
        {
            SoundManager.instance.Play(SoundID.BossDead);
            OnDie?.Invoke();
            transform.Find("BossDie").gameObject.SetActive(true);
            return;
        }
        OnAttackSuccess?.Invoke();
        for(int i = 0; i < _hitEffectNum; ++ i)
        {
            HitEffect hitEffect = _hitPool.Get();
        }
    }

    private void ChangeDamageType()
    {
        _confirmedDamageGain += _temporaryDamageGain;
        _temporaryDamageGain = 0;
        _totalDamageGain = _temporaryDamageGain + _confirmedDamageGain;
        OnConfChange?.Invoke(_totalDamageGain, _damageTreshold);
        OnDamageChange?.Invoke();
    }

    private void GetTempDamage()
    {
        Effect particle = _pool.Get();
        _temporaryDamageGain += 5;
        _totalDamageGain = _temporaryDamageGain + _confirmedDamageGain;
        OnTempChange?.Invoke(_temporaryDamageGain, _confirmedDamageGain, _damageTreshold);
    }

    private void DecreaseTempDamage()
    {
        _temporaryDamageGain--;
        _elapsedTime = 0;
        OnTempChange?.Invoke(_temporaryDamageGain, _confirmedDamageGain, _damageTreshold);
    }

    private void EnterGroggyState()
    {
        SoundManager.instance.Play(SoundID.BossGroggy);
        GetComponent<Animator>().SetBool("isGroggy", true);
        transform.Find("Circle").gameObject.SetActive(true);
        _isOnGroggy = true;
        _groggyEffect.SetActive(true);
        ResetSettings();
        OnGroggy?.Invoke();
    }
    public int HP()
    {
        return _Hp;
    }

    public void Init(ObjectManager objectManager)
    {
        _objectManager = objectManager;
    }
    public void EndGroggy()
    {
        _isOnGroggy = false;
        _groggyEffect.SetActive(false);
    }
    private void ResetSettings()
    {
        _temporaryDamageGain = 0;
        _confirmedDamageGain = 0;
        _totalDamageGain = 0;
        _elapsedTime = 0;
    }

    private void InitPool()
    {
        if (_pool == null || _hitPool == null)
        {
            _pool = new ObjectPool<Effect>(Create, OnGet, OnRelease, OnDestroyParticle, maxSize: 8);
            _hitPool = new ObjectPool<HitEffect>(CreateHit, OnGet, OnRelease, OnDestroyParticle, maxSize: 25);
        }
    }

    private Effect Create()
    {
        Effect particle = Instantiate(_damageEffect, transform.position, transform.rotation).GetComponent<Effect>();
        particle.SetPool(_pool);
        return particle;
    }
    private HitEffect CreateHit()
    {
        HitEffect effect = Instantiate(_hitEffect, transform.position, transform.rotation).GetComponent<HitEffect>();
        effect.SetPool(_hitPool, transform);
        return effect;
    }

    private void OnGet(Effect particle)
    {
        particle.gameObject.SetActive(true);
        particle.transform.position = transform.position;
        particle.transform.rotation = transform.rotation;
    }
    private void OnRelease(Effect particle)
    {
        particle.gameObject.SetActive(false);
    }

    private void OnDestroyParticle(Effect particle)
    {
        Destroy(particle.gameObject);
    }
}
