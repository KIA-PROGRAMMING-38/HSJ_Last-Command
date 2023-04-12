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
    [SerializeField] private float _createRadius;
    [SerializeField] private GameObject _bullet;

    private float _attackStartTime;
    [SerializeField] private float _attackWaitTime;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerTransform = animator.transform;
        _attackStartTime = 0;
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
        for (int i = 0; i < _activeHeadCount; ++i)
        {
            GameObject activeHead = _playerTransform.GetChild(i).gameObject;
            activeHead.SetActive(true);
        }
    }
    void Attack(Player player)
    {
        if(player.IsEnergyCharged())
        {
            Debug.Log("АјАн!");
            player.UseEnergy();
            ShootBullet();
            --_activeHeadCount;
        }
    }

    void ShootBullet()
    {
        Vector2 createPosition = (Vector2)_playerTransform.position + (Random.insideUnitCircle.normalized * _createRadius);
        GameObject bullet = Instantiate(_bullet, createPosition, new Quaternion(0,0,0,0));
        bullet.AddComponent<Bullet>();
    }
}
