using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class BossGroggy : StateMachineBehaviour
{
    [SerializeField] private float _groggyTime;
    private float _elapsedTime;
    private AttackZone _attackZone;
    private bool _isOnZone;

    public event Action OnGroggyEnd;
    public event Action<float> OnGroggyTime;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_attackZone == null)
        {
            _attackZone = animator.transform.Find("Circle").gameObject.GetComponent<AttackZone>();
        }
        _attackZone.OnZone -= isOnZone;
        _attackZone.OffZone -= isOffZone;
        _attackZone.OnZone += isOnZone;
        _attackZone.OffZone += isOffZone;
        _elapsedTime = 0;
        Debug.Log("그로기!");
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       if (Input.GetKeyDown(KeyCode.F) && _isOnZone)
        {
            animator.SetBool("isGroggy", false);
            Debug.Log("공격!");
            animator.GetComponent<Boss>().Damaged();
            OnGroggyEnd?.Invoke();
        }

        _elapsedTime += Time.deltaTime;
        OnGroggyTime?.Invoke(_groggyTime - _elapsedTime);

        if(_elapsedTime >= _groggyTime)
        {
            animator.SetBool("isGroggy", false);
            OnGroggyEnd?.Invoke();
            Debug.Log("그로기 끝..");
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
