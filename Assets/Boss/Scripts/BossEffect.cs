using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEngine.ParticleSystem;

public class BossEffect : MonoBehaviour
{
    private Boss _boss;
    private BossGroggy _groggy;

    private GameObject _groggyEffect;
    [SerializeField] private GameObject _hitEffect;
    [SerializeField] private GameObject _spreadEffect;
    [SerializeField] private GameObject _slashEffect;
    private IObjectPool<Effect> _hitPool;
    private IObjectPool<SpreadEffect> _spreadPool;
    private IObjectPool<Effect> _slashPool;

    private int _spreadEffectNum = 15;

    public void Init(Boss boss)
    {
        _boss = boss;
        _groggy = GetComponent<Animator>().GetBehaviour<BossGroggy>();
        _groggyEffect = transform.Find("BossGroggy").gameObject;

        _boss.OnAttackSuccess -= CreateSpreadEffect;
        _boss.OnAttackSuccess += CreateSpreadEffect;
        _boss.OnGetTempDamage -= CreateHitEffect;
        _boss.OnGetTempDamage += CreateHitEffect;
        _boss.OnGroggy -= () => _groggyEffect.SetActive(true);
        _boss.OnGroggy += () => _groggyEffect.SetActive(true);
        _groggy.OnGroggyEnd -= () => _groggyEffect.SetActive(false);
        _groggy.OnGroggyEnd += () => _groggyEffect.SetActive(false);
        _groggy.OnSlashEffectCreate -= CreateSlashEffect;
        _groggy.OnSlashEffectCreate += CreateSlashEffect;
        InitPool();
    }
    private void CreateSpreadEffect()
    {
        for (int i = 0; i < _spreadEffectNum; ++i)
        {
            SpreadEffect spreadEffect = _spreadPool.Get();
        }
    }
    private Effect CreateHitEffect()
    {
        return _hitPool.Get();
    }
    private Effect CreateSlashEffect()
    {
        return _slashPool.Get();
    }
    private void InitPool()
    {
        if (_slashPool == null)
        {
            _slashPool = new ObjectPool<Effect>(CreateSlash, OnGetRandom, OnRelease, OnDestroyEffect, maxSize: 2);
        }
        if (_spreadPool == null)
        {
            _spreadPool = new ObjectPool<SpreadEffect>(CreateSpread, OnGet, OnRelease, OnDestroyEffect, maxSize: 25);
        }
        if(_hitPool == null)
        {
            _hitPool = new ObjectPool<Effect>(CreateHit, OnGet, OnRelease, OnDestroyEffect, maxSize: 10);
        }
    }

    private Effect CreateSlash()
    {
        Effect particle = Instantiate(_slashEffect, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360))).GetComponent<Effect>();
        particle.SetPool(_slashPool);
        return particle;
    }

    private void OnGetRandom(Effect particle)
    {
        particle.gameObject.SetActive(true);
        particle.transform.position = transform.position;
        particle.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
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

    private void OnDestroyEffect(Effect particle)
    {
        Destroy(particle.gameObject);
    }

    private Effect CreateHit()
    {
        Effect particle = Instantiate(_hitEffect, transform.position, transform.rotation).GetComponent<Effect>();
        particle.SetPool(_hitPool);
        return particle;
    }
    private SpreadEffect CreateSpread()
    {
        SpreadEffect effect = Instantiate(_spreadEffect, transform.position, transform.rotation).GetComponent<SpreadEffect>();
        effect.SetPool(_spreadPool, transform);
        return effect;
    }
}
