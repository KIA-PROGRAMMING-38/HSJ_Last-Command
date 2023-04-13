using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;

public class PlayerMovement : MonoBehaviour
{
    private PlayerState _currentState;
    private PlayerInput _input;
    [SerializeField] private float _bumpIntense;
    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
    }
    private void FixedUpdate()
    {
        if(_currentState != null)
        {
            _currentState.Move(gameObject);
        }
    }

    public void ChangeState(PlayerState changedState)
    {
        _currentState = changedState;
    }
    public void CollideWithBlock()
    {
        switch(_input._moveDirection)
        {
            case Direction.Right:
                gameObject.transform.position += (Vector3)Vector2.left * _bumpIntense;
                break;
            case Direction.Left:
                gameObject.transform.position += (Vector3)Vector2.right * _bumpIntense;
                break;
            case Direction.Up:
                gameObject.transform.position += (Vector3)Vector2.down * _bumpIntense;
                break;
            case Direction.Down:
                gameObject.transform.position += (Vector3)Vector2.up * _bumpIntense;
                break;
            default:
                break;
        }
    }
}
