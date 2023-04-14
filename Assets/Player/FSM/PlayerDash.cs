using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using UnityEngine.Animations;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;

public class PlayerDash : StateMachineBehaviour
{
    [SerializeField] private float _dashDistance;
    [SerializeField] private float _bumpDistance;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerInput input = animator.GetComponent<PlayerInput>();
        Transform playerTransform = animator.transform;
        Vector2 _expectedDashPoint = Vector2.zero;
        Vector2 _expectedDashAmount = Vector2.zero;

        _expectedDashAmount = input._playerDirection._moveDirection * _dashDistance;
        _expectedDashPoint = (Vector2)playerTransform.position + _expectedDashAmount;
        
        bool isOnBlock = true;
        while (isOnBlock)
        {
            Collider2D box = Physics2D.OverlapCircle(_expectedDashPoint, animator.GetComponent<CircleCollider2D>().radius, LayerMask.GetMask("Block"));
            if (box != null)
            {
                _expectedDashPoint += input._playerDirection._moveDirection * -_bumpDistance;
            }
            else
            {
                isOnBlock = false;
            }
        }

        playerTransform.position = _expectedDashPoint;

        for (int i = 0; i < playerTransform.childCount; ++i)
        {
            GameObject followingHead = playerTransform.GetChild(i).gameObject;
            followingHead.transform.localPosition = new Vector2(0, 0);
            followingHead.GetComponent<Head>()?.ClearPath();
        }

        animator.SetBool("isDashing", false);
    }
}
