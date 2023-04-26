using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class BossAttack : StateMachineBehaviour
{
    private Vector2 _targetPosition;
    private Vector2 _currentPosition;
    private float _moveSpeed = 8f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _targetPosition = new Vector2(-animator.transform.parent.position.x, animator.transform.parent.position.y);
        
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _currentPosition = animator.transform.parent.position;

        float distance = Vector2.Distance(_currentPosition, _targetPosition);
        float moveDistance = _moveSpeed * Time.deltaTime;

        if (distance >= 0.5f)
        {
            Vector2 moveDirection = (_targetPosition - _currentPosition).normalized;
            animator.transform.parent.position += (Vector3)moveDirection * moveDistance;
        }
        else
        {
            animator.transform.parent.position = _targetPosition;
            animator.SetTrigger("isMoved");
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
