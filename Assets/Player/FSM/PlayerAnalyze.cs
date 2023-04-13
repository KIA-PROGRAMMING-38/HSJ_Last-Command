using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnalyze : StateMachineBehaviour
{
    private int _activeEnergyCount;
    private Transform _playerTransform;
    public float Vertical;
    public float Horizontal;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private GameObject _bullet;

    private float _attackStartTime;
    [SerializeField] private float _attackWaitTime;
    [SerializeField] private float _overclockWeight;
    private Player _player;
    private bool _attacked;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerTransform = animator.transform;
        _attackStartTime = 0;
        _attacked = false;

        _activeEnergyCount = 0;
        for (int i = 0; i < _playerTransform.childCount; ++i)
        {
            GameObject followingHead = _playerTransform.GetChild(i).gameObject;
            followingHead.transform.localPosition = new Vector2(0, 0);
            if (followingHead.activeSelf)
            {
                ++_activeEnergyCount;
                followingHead.SetActive(false);
            }
        }
        _player = animator.GetComponent<Player>();

        if(_player != null)
        {
            _player.OnOverclock -= OnOverclock;
            _player.OnOverclock += OnOverclock;
            _player.OnOverclockEnd -= OnOverclockEnd;
            _player.OnOverclockEnd += OnOverclockEnd;
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Horizontal = Input.GetAxisRaw("Horizontal");
        Vertical = Input.GetAxisRaw("Vertical");
        _playerTransform.Translate(new Vector3(Horizontal, Vertical, 0) * _moveSpeed * Time.deltaTime);

        if (_attackStartTime > _attackWaitTime)
        {
            Attack(animator.GetComponent<Player>());
            _attackStartTime = 0;
        }
        else
        {
            _attackStartTime += Time.deltaTime;
        }

    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        for (int i = 0; i < _activeEnergyCount; ++i)
        {
            GameObject activeEnergy = _playerTransform.GetChild(i).gameObject;
            activeEnergy.SetActive(true);
        }
        if(_attacked)
        {
            _player.EndOverclock();
        }
    }
    private void OnDestroy()
    {
        if (_player != null)
        {
            _player.OnOverclock -= OnOverclock;
            _player.OnOverclockEnd -= OnOverclockEnd;
        }
    }
    void Attack(Player player)
    {
        if(player.IsEnergyCharged())
        {
            Debug.Log("АјАн!");
            player.UseEnergy();
            ShootBullet();
            --_activeEnergyCount;
            _attacked = true;
        }
    }

    void ShootBullet()
    {
        GameObject bullet = Instantiate(_bullet, _playerTransform.position, new Quaternion(0,0,0,0));
        bullet.AddComponent<Bullet>();
    }

    void OnOverclock()
    {
        _attackWaitTime *= _overclockWeight;
    }

    void OnOverclockEnd()
    {
        _attackWaitTime /= _overclockWeight;
    }
}
