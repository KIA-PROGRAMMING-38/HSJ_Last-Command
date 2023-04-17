using UnityEngine;
using Util.Direction;

public class PlayerIdleMove : PlayerState
{
    private Vector3 _moveVector = Vector2.right;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _overclockWeight;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        InitSettings(animator);
        if(_player != null)
        {
            _player.OnOverclock -= OnOverclock;
            _player.OnOverclock += OnOverclock;
            _player.OnOverclockEnd -= OnOverclockEnd;
            _player.OnOverclockEnd += OnOverclockEnd;
        }
        _movement?.ChangeState(this);
    }
    private void OnDestroy()
    {
        if(_player != null)
        {
            _player.OnOverclock -= OnOverclock;
            _player.OnOverclockEnd -= OnOverclockEnd;
        }
    }

    private void OnOverclock()
    {
        _moveSpeed *= _overclockWeight;
    }
    private void OnOverclockEnd()
    {
        _moveSpeed /= _overclockWeight;
    }

    public override void Move()
    {
        _moveVector = _input._playerDirection._moveDirection;
        _player.transform.position += (_moveVector * (Time.deltaTime * _moveSpeed));
    }
}
