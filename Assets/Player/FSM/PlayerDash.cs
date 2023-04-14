using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;
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

        switch (input._moveDirection)
        {
            case Direction.Right:
                _expectedDashAmount = (Vector3)Vector2.right * _dashDistance;
                break;
            case Direction.Left:
                _expectedDashAmount = (Vector3)Vector2.left * _dashDistance;
                break;
            case Direction.Up:
                _expectedDashAmount = (Vector3)Vector2.up * _dashDistance;
                break;
            case Direction.Down:
                _expectedDashAmount = (Vector3)Vector2.down * _dashDistance;
                break;
            default:
                break;
        }
        _expectedDashPoint = (Vector2)playerTransform.position + _expectedDashAmount;
        
        bool isOnBlock = true;
        while (isOnBlock)
        {
            Collider2D box = Physics2D.OverlapCircle(_expectedDashPoint, animator.GetComponent<CircleCollider2D>().radius, LayerMask.GetMask("Block"));
            if (box != null)
            {
                    switch (input._moveDirection)
                    {
                        case Direction.Right:
                        _expectedDashPoint += Vector2.left * _bumpDistance;
                            break;
                        case Direction.Left:
                        _expectedDashPoint += Vector2.right * _bumpDistance;
                        break;
                        case Direction.Up:
                        _expectedDashPoint += Vector2.down * _bumpDistance;
                        break;
                        case Direction.Down:
                        _expectedDashPoint += Vector2.up * _bumpDistance;
                        break;
                        default:
                            break;
                    }
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
