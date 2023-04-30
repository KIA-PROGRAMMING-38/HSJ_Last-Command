using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Util.Enum;
using System;

public class PlayerAnalyze : PlayerState
{
    private int _activeEnergyCount;
    private Transform _playerTransform;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private float _moveSpeed;

    private float _attackStartTime;
    [SerializeField] private float _attackWaitTime;
    [SerializeField] private float _overclockWeight;

    private bool _attacked;
    private GameObject _followingHead;

    private IObjectPool<Bullet> _pool;

    public event Action<float> OnAnalyzing;
    public event Action OnAnalyze;
    public event Action OffAnalyzing;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        InitSettings(animator);
        InitiatePool();
        _playerTransform = _player.transform;
        _attackStartTime = 0;
        _attacked = false;
        _activeEnergyCount = 0;
        for (int i = 0; i < _player.maxLength - 1; ++i)
        {
            _followingHead = _playerTransform.GetChild(i).gameObject;
            _followingHead.transform.localPosition = new Vector2(0, 0);
            if (_followingHead.activeSelf)
            {
                ++_activeEnergyCount;
                _followingHead.SetActive(false);
            }
        }
        if (_movement != null)
        {
            _movement.ChangeState(this);
        }
        OnAnalyze?.Invoke();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_attackStartTime > _attackWaitTime)
        {
            Attack();
            _attackStartTime = 0;
        }
        else
        {
            _attackStartTime += Time.deltaTime;
        }
        if(_activeEnergyCount > 2)
        {
            OnAnalyzing?.Invoke(Mathf.Min(_attackStartTime / _attackWaitTime, 1));
        }
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        for (int i = 0; i < _activeEnergyCount; ++i)
        {
            _playerTransform.GetChild(i).gameObject.SetActive(true);
        }
        if (_attacked)
        {
            _player.EndOverclock();
        }
        OffAnalyzing?.Invoke();
    }
    void Attack()
    {
        if (_player.IsEnergyCharged())
        {
            SoundManager.instance.Play(SoundID.PlayerShoot);
            _player.UseEnergy();
            Bullet bullet = _pool.Get();
            --_activeEnergyCount;
            _attacked = true;
        }
    }


    public void OnOverclock()
    {
        _attackWaitTime *= _overclockWeight;
    }

    public void OnOverclockEnd()
    {
        _attackWaitTime /= _overclockWeight;
    }

    public override void Move()
    {
        float Horizontal = Input.GetAxisRaw("Horizontal");
        float Vertical = Input.GetAxisRaw("Vertical");
        _player.transform.position += new Vector3(Horizontal, Vertical, 0) * _moveSpeed * Time.fixedDeltaTime;
    }

    private void InitiatePool()
    {
        if (_pool == null)
        {
            _pool = new ObjectPool<Bullet>(ShootBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet, maxSize: 5);
        }
    }

    private Bullet ShootBullet()
    {
        Bullet bullet = Instantiate(_bullet, _playerTransform).AddComponent<Bullet>();
        bullet.SetPool(_pool);
        return bullet;
    }
    private void OnGetBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
    }

    private void OnReleaseBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }
    private void OnDestroyBullet(Bullet bullet)
    {
        Destroy(bullet.gameObject);
    }
    public void EarnEnergy()
    {
        ++_activeEnergyCount;
    }
    public void Damaged()
    {
       if(_player != null)
        {
            _activeEnergyCount = _player._defaultLength - 1;
        }
    }

}
