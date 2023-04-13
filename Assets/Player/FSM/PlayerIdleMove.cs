using UnityEngine;
using Enum;

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
        if(_playerMovement != null)
        {
            _playerMovement.ChangeState(this);
        }
        if(_player != null)
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
        switch (_input._moveDirection)
        {
            case Direction.Right:
                _moveVector = Vector2.right;
                break;
            case Direction.Left:
                _moveVector = Vector2.left;
                break;
            case Direction.Up:
                _moveVector = Vector2.up;
                break;
            case Direction.Down:
                _moveVector = Vector2.down;
                break;
            default:
                break;
        }

        player.transform.position += (_moveVector * (Time.deltaTime * _moveSpeed));
    }
}
