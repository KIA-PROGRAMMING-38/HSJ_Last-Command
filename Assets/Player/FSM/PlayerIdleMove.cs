using UnityEngine;
using Enum;
using Move;

public class PlayerIdleMove : StateMachineBehaviour
{
    private Vector3 _moveVector = Vector2.right;
    [SerializeField] private float _moveSpeed;
    private GameObject _player;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _player = animator.gameObject;
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerInput input = animator.GetComponent<PlayerInput>();
        
        switch(input._moveDirection)
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
}
