using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using UnityEngine.Animations;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;
using UnityEngine.Pool;

public class PlayerDash : PlayerState
{
    [SerializeField] private float _dashDistance;
    [SerializeField] private float _bumpDistance;
    Transform _playerTransform;
    Vector2 _expectedDashAmount;
    Vector2 _expectedDashPoint;
    Collider2D _box;

    [SerializeField] private GameObject _circleEffect;
    private IObjectPool<Effect> _pool;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        InitSettings(animator);
        _movement.ChangeState(this);
        _player.EnableDashTrail();
        _playerTransform = _player.transform;
        InitPool();
        Effect circle = _pool.Get();
    }

    public override void Move()
    {
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

    private void InitPool()
    {
        if(_pool == null)
        {
            _pool = new ObjectPool<Effect>(Create, OnGet, OnRelease, OnDestroyParticle, maxSize: 2);
        }
    }

    private Effect Create()
    {
        Effect particle = Instantiate(_circleEffect, _playerTransform.position, _playerTransform.rotation).GetComponent<Effect>();
        particle.SetPool(_pool);
        return particle;
    }

    private void OnGet(Effect particle)
    {
        particle.gameObject.SetActive(true);
        particle.transform.position = _playerTransform.position;
        particle.transform.rotation = _playerTransform.rotation;
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
