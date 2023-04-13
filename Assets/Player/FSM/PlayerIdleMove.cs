using UnityEngine;
using Enum;

public class PlayerIdleMove : StateMachineBehaviour
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
        if(_player != null)
        {
            _player.OnOverclock -= OnOverclock;
            _player.OnOverclock += OnOverclock;
            _player.OnOverclockEnd -= OnOverclockEnd;
            _player.OnOverclockEnd += OnOverclockEnd;
        }
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        switch(_input._moveDirection)
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

        animator.transform.position += (_moveVector * (Time.deltaTime * _moveSpeed));
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
}
