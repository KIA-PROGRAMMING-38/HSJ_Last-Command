using Move;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnalyze : StateMachineBehaviour
{
    private int _activeHeadCount;
    private Transform _playerTransform;
    public float Vertical;
    public float Horizontal;
[SerializeField] private float _moveSpeed;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerTransform = animator.transform;
        _activeHeadCount = 0;
        for (int i = 0; i < _playerTransform.childCount; ++i)
        {
            GameObject followingHead = _playerTransform.GetChild(i).gameObject;
            followingHead.transform.localPosition = new Vector2(0, 0);
            if (followingHead.activeSelf)
            {
                ++_activeHeadCount;
                followingHead.SetActive(false);
            }
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Horizontal = Input.GetAxisRaw("Horizontal");
        Vertical = Input.GetAxisRaw("Vertical");
        _playerTransform.Translate(new Vector3(Horizontal, Vertical, 0) * _moveSpeed * Time.deltaTime);
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        for (int i = 0; i < _activeHeadCount; ++i)
        {
            GameObject activeHead = _playerTransform.GetChild(i).gameObject;
            activeHead.SetActive(true);
        }
    }
}
