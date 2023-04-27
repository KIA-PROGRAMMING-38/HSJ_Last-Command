using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using Util.Enum;
using Random = UnityEngine.Random;

public class BossGroggy : StateMachineBehaviour
{
    [SerializeField] private float _groggyTime;
    private float _elapsedTime;
    private AttackZone _attackZone;
    private bool _isOnZone;

    public event Action OnGroggyEnd;
    public event Action<float> OnGroggyTime;
    public event Action OnAttackSuccess;

    private IObjectPool<Effect> _pool;
    [SerializeField] private GameObject _particle;
    private Transform _transform;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_attackZone == null)
        {
            _attackZone = animator.transform.Find("Circle").gameObject.GetComponent<AttackZone>();
            _transform = animator.transform;
            InitPool();
            _attackZone.OnZone -= isOnZone;
            _attackZone.OffZone -= isOffZone;
            _attackZone.OnZone += isOnZone;
            _attackZone.OffZone += isOffZone;
        }
        _elapsedTime = 0;
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       if (Input.GetKeyDown(KeyCode.F) && _isOnZone)
        {
            SoundManager.instance.Play(SoundID.PlayerAttack);
            SoundManager.instance.Play(SoundID.BossHit);
            animator.SetBool("isGroggy", false);
            OnGroggyEnd?.Invoke();
            OnAttackSuccess?.Invoke();
            animator.GetComponent<Boss>().Damaged();
            Effect particle = _pool.Get();
        }

        _elapsedTime += Time.deltaTime;
        OnGroggyTime?.Invoke(_groggyTime - _elapsedTime);

        if(_elapsedTime >= _groggyTime)
        {
            animator.SetBool("isGroggy", false);
            OnGroggyEnd?.Invoke();
        }
    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.Find("Circle").gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if(_attackZone != null)
        {
            _attackZone.OnZone -= isOnZone;
            _attackZone.OffZone -= isOffZone;
        }
    }
    private void isOnZone () => _isOnZone = true;
    private void isOffZone() => _isOnZone = false;

    private void InitPool()
    {
        if (_pool == null)
        {
            _pool = new ObjectPool<Effect>(Create, OnGet, OnRelease, OnDestroyParticle, maxSize: 2);
        }
    }

    private Effect Create()
    {
        Effect particle = Instantiate(_particle, _transform.position, Quaternion.Euler(0,0,Random.Range(0,360))).GetComponent<Effect>();
        particle.SetPool(_pool);
        return particle;
    }

    private void OnGet(Effect particle)
    {
        particle.gameObject.SetActive(true);
        particle.transform.position = _transform.position;
        particle.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
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
