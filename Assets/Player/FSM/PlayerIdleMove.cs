using UnityEngine;

public class PlayerIdleMove : StateMachineBehaviour
{
    private Vector3 _moveVector;
    [SerializeField] private float _moveSpeed;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _moveVector = Vector2.right;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            _moveVector = Vector2.right;
        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _moveVector = Vector2.left;
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            _moveVector = Vector2.up;
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            _moveVector = Vector2.down;
        }

        animator.transform.position = animator.transform.position + (_moveVector * (Time.deltaTime * _moveSpeed));
    }
}
