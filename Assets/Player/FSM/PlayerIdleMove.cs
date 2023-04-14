using UnityEngine;
using Util.Direction;

public class PlayerIdleMove : PlayerState
{
    private Vector3 _moveVector = Vector2.right;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _overclockWeight;
    private Player _player;
    private PlayerInput _input;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _input = animator.GetComponent<PlayerInput>();
        _player = animator.GetComponent<Player>();
        _playerMovement = animator.GetComponent<PlayerMovement>();
        if (_playerMovement != null)
        {
            _playerMovement.ChangeState(this);
        }
        if (_player != null)
        {
            _player.OnOverclock -= OnOverclock;
            _player.OnOverclock += OnOverclock;
            _player.OnOverclockEnd -= OnOverclockEnd;
            _player.OnOverclockEnd += OnOverclockEnd;
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

    private void OnOverclock()
    {
        _moveSpeed *= _overclockWeight;
    }
    private void OnOverclockEnd()
    {
        _moveSpeed /= _overclockWeight;
    }

    public override void Move(GameObject player)
    {
        _moveVector = _input._playerDirection._moveDirection;
        player.transform.position += (_moveVector * (Time.deltaTime * _moveSpeed));
    }
}
