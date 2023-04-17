using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using UnityEngine.Animations;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;

public class PlayerDash : PlayerState
{
    [SerializeField] private float _dashDistance;
    [SerializeField] private float _bumpDistance;
    Transform _playerTransform;
    Vector2 _expectedDashAmount;
    Vector2 _expectedDashPoint;
    Collider2D _box;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        InitSettings(animator);
        _movement.ChangeState(this);
    }

    public override void Move()
    {
        _playerTransform = _player.transform;
        _expectedDashPoint = Vector2.zero;
        _expectedDashAmount = Vector2.zero;

        _expectedDashAmount = _input._playerDirection._moveDirection * _dashDistance;
        _expectedDashPoint = (Vector2)_playerTransform.position + _expectedDashAmount;

        bool isOnBlock = true;
        while (isOnBlock)
        {
            _box = Physics2D.OverlapCircle(_expectedDashPoint, _player.GetComponent<CircleCollider2D>().radius, LayerMask.GetMask("Block"));
            if (_box != null)
            {
                _expectedDashPoint += _input._playerDirection._moveDirection * -_bumpDistance;
            }
            else
            {
                isOnBlock = false;
            }
        }

        _playerTransform.position = _expectedDashPoint;

        for (int i = 0; i < _playerTransform.childCount; ++i)
        {
            GameObject followingHead = _playerTransform.GetChild(i).gameObject;
            followingHead.transform.localPosition = new Vector2(0, 0);
            followingHead.GetComponent<Head>()?.ClearPath();
        }
        _player.GetComponent<Animator>().SetBool("isDashing", false);
    }
}
