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
    public event Func<Effect> OnSlashEffectCreate;
    private Transform _transform;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_attackZone == null)
        {
            _attackZone = animator.transform.Find("Circle").gameObject.GetComponent<AttackZone>();
            _transform = animator.transform;
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
            Effect SlashEffect = OnSlashEffectCreate?.Invoke();
            animator.GetComponent<Boss>().Damaged();
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
}
