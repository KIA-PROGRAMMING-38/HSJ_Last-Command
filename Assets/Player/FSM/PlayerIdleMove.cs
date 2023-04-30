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
        _movement?.ChangeState(this);
    }
    public void OnOverclock()
    {
        _moveSpeed *= _overclockWeight;
    }
    public void OnOverclockEnd()
    {
        _moveSpeed /= _overclockWeight;
    }

    public override void Move()
    {
        _moveVector = _input._playerDirection._moveDirection;
        _player.transform.position += (_moveVector * (Time.deltaTime * _moveSpeed));
    }
}
