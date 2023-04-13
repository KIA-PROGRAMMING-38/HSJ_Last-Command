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
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerInput input = animator.GetComponent<PlayerInput>();
        Transform playerTransform = animator.transform;


        switch (input._moveDirection)
        {
            case Direction.Right:
                playerTransform.position += (Vector3)Vector2.right * _dashDistance;
                break;
            case Direction.Left:
                playerTransform.position += (Vector3)Vector2.left * _dashDistance;
                break;
            case Direction.Up:
                playerTransform.position += (Vector3)Vector2.up * _dashDistance;
                break;
            case Direction.Down:
                playerTransform.position += (Vector3)Vector2.down * _dashDistance;
                break;
            default:
                break;
        }

        for (int i = 0; i < playerTransform.childCount; ++i)
        {
            GameObject followingHead = playerTransform.GetChild(i).gameObject;
            followingHead.transform.localPosition = new Vector2(0, 0);
            followingHead.GetComponent<Head>()?.ClearPath();
        }

        animator.SetBool("isDashing", false);
    }
}
