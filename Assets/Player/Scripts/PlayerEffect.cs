using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PlayerEffect : MonoBehaviour
{
    Player _player;
    private SpriteRenderer[] _energySprites;
    private GameObject[] _energies;

    [SerializeField] private GameObject _dieParticle;
    IEnumerator _dieEffectTimer;
    private TrailRenderer _trail;
    [SerializeField] private GameObject _hurtEffect;
    private GameObject[] _hurtParticle;
    [SerializeField] private GameObject _overclockEffectPrefab;
    private GameObject _overclockEffect;
    [SerializeField] private GameObject _dashChargeEffectPrefab;
    private GameObject _dashChargeEffect;
    [SerializeField] private GameObject _dashWaitBoxPrefab;
    private GameObject _dashWaitBox;
    [SerializeField] private GameObject _overclockBoxPrefab;
    private GameObject _overclockBox;
    [SerializeField] private GameObject _earnEnergyBoxPrefab;
    private GameObject _earnEnergyBox;
    [SerializeField] private GameObject _invinciblePrefab;
    private GameObject _invincible;
    [SerializeField] private GameObject _analyzeBoxPrefab;
    private GameObject _analyzeBox;
    [SerializeField] private GameObject _startPointPrefab;
    private GameObject _startPoint;
    [SerializeField] private GameObject _startTextPrefab;
    private GameObject _startText;
    private IObjectPool<SpreadEffect> _healPool;
    [SerializeField] private GameObject _healEffect;
    private IObjectPool<Effect> _dashStartPool;
    [SerializeField] private GameObject _dashStartEffect;
    private void InitiatePool()
    {
        if (_healPool == null)
        {
            _healPool = new ObjectPool<SpreadEffect>(CreateHeal, OnGet, OnRelease, OnDestroyParticle, maxSize: 25);
        }
        if (_dashStartPool == null)
        {
            _dashStartPool = new ObjectPool<Effect>(CreateCircle, OnGet, OnRelease, OnDestroyParticle, maxSize: 2);
        }
    }

    public void SetAnalyzeBoxTrue() => _analyzeBox.SetActive(true);
    public void SetAnalyzeBoxFalse() => _analyzeBox.SetActive(false);

    IEnumerator DashTrail()
    {
        _trail.enabled = true;
        foreach (SpriteRenderer sprites in _energySprites)
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
    IEnumerator HealEffect()
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < 8; ++i)
        {
            SpreadEffect healEffect = _healPool.Get();
        }
    }
    IEnumerator PlayerDieEffect()
    {
        WaitForSeconds wait = new WaitForSeconds(0.3f);

        for (int i = 1; i <= _energies.Length; ++i)
        {
            if (_energies[_energies.Length - i].activeSelf)
            {
                Instantiate(_dieParticle, _energies[_energies.Length - i].transform.position, _energies[_energies.Length - i].transform.rotation);
                _energies[_energies.Length - i].SetActive(false);
                if (i == _energies.Length)
                {
                    _player.OnDieEffectEnd();
                }
                yield return wait;
            }
            else
            {
                yield return null;
            }

        }
    }
    private SpreadEffect CreateHeal()
    {
        SpreadEffect effect = Instantiate(_healEffect, transform.position, transform.rotation).GetComponent<SpreadEffect>();
        effect.SetPool(_healPool, transform);
        return effect;
    }
    private Effect CreateCircle()
    {
        Effect particle = Instantiate(_dashStartEffect, transform.position, transform.rotation).GetComponent<Effect>();
        particle.SetPool(_dashStartPool);
        return particle;
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

    public void Init(Player player, SpriteRenderer[] energySprites, GameObject[] energies)
    {
        _player = player;
        if(_energySprites == null)
        {
            _energySprites = new SpriteRenderer[energySprites.Length];
            _energySprites = energySprites;
        }
        if(_energies == null)
        {
            _energies = new GameObject[energies.Length];
            _energies = energies;
        }

        if (_player != null)
        {
            _player.OnGameStart -= CreateStartEffect;
            _player.OnGameStart += CreateStartEffect;
            _player.OnEnergyBoxActivated -= () => _earnEnergyBox.SetActive(true);
            _player.OnEnergyBoxActivated += () => _earnEnergyBox.SetActive(true);
            _player.OnOverclock -= () => _overclockBox.SetActive(true);
            _player.OnOverclock += () => _overclockBox.SetActive(true);
            _player.OnOverclock -= () => _overclockEffect.SetActive(true);
            _player.OnOverclock += () => _overclockEffect.SetActive(true);
            _player.OnOverclockEnd -= () => _overclockEffect.SetActive(false);
            _player.OnOverclockEnd += () => _overclockEffect.SetActive(false);
            _player.OnInvincibleStart -= () => _invincible.SetActive(true);
            _player.OnInvincibleStart += () => _invincible.SetActive(true);
            _player.OnInvincibleEnd -= () => _invincible.SetActive(false);
            _player.OnInvincibleEnd += () => _invincible.SetActive(false);
            _player.OnHpDecrease -= CreateHurtEffect;
            _player.OnHpDecrease += CreateHurtEffect;
            _player.OnHpIncrease -= () => StartCoroutine(HealEffect());
            _player.OnHpIncrease += () => StartCoroutine(HealEffect());
            _player.OnDie -= () => StartCoroutine(_dieEffectTimer);
            _player.OnDie += () => StartCoroutine(_dieEffectTimer);
            _player.GetComponent<PlayerInput>().OnDashChargeInformed -= () => _dashWaitBox.SetActive(true);
            _player.GetComponent<PlayerInput>().OnDashChargeInformed += () => _dashWaitBox.SetActive(true);
        }
        _dieEffectTimer = PlayerDieEffect();
        _trail = GetComponent<TrailRenderer>();
        _hurtParticle = new GameObject[3];
        _hurtParticle[0] = Instantiate(_hurtEffect);
        _hurtParticle[1] = _hurtParticle[0].transform.GetChild(0).gameObject;
        _hurtParticle[2] = _hurtParticle[0].transform.GetChild(1).gameObject;
        for (int i = 1; i < 3; ++i)
        {
            _hurtParticle[i].SetActive(false);
        }
        for (int i = 1; i < _player.maxLength; ++i)
        {
            _invincible = Instantiate(_invinciblePrefab, _energies[i].transform);
            _invincible.SetActive(false);
        }
        _invincible = Instantiate(_invinciblePrefab, transform);
        _invincible.SetActive(false);
        _overclockEffect = Instantiate(_overclockEffectPrefab, transform);
        _overclockEffect.SetActive(false);
        _overclockBox = Instantiate(_overclockBoxPrefab, transform);
        _overclockBox.SetActive(false);
        _earnEnergyBox = Instantiate(_earnEnergyBoxPrefab, transform);
        _earnEnergyBox.SetActive(false);
        _dashChargeEffect =Instantiate(_dashChargeEffectPrefab, transform);
        _earnEnergyBox.name = "Earn Energy Box";
        _analyzeBox = Instantiate(_analyzeBoxPrefab, transform);
        _analyzeBox.name = "Analyzing Box";
        _analyzeBox.SetActive(false);
        _startPoint = Instantiate(_startPointPrefab);
        _dashWaitBox = Instantiate(_dashWaitBoxPrefab, transform);
        _dashWaitBox.SetActive(false);
        InitiatePool();
    }

    private void CreateHurtEffect()
    {
        for (int i = 1; i < 3; ++i)
        {
            _hurtParticle[i].transform.position = transform.position;
            _hurtParticle[i].SetActive(true);
        }
    }
    private void CreateStartEffect()
    {
        _startPoint.SetActive(false);
        _startText = Instantiate(_startTextPrefab);
    }

    public void EnableDashTrail() => StartCoroutine(DashTrail());
    public Effect EnableCircleEffect()
    {
        return _dashStartPool.Get();
    }
}
