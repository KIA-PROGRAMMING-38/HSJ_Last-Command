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
    public Vector2 CollideWithBlock(Vector2 position)
    {
        switch(_input._moveDirection)
        {
            case Direction.Right:
                position += Vector2.left * _bumpIntense;
                break;
            case Direction.Left:
                position += Vector2.right * _bumpIntense;
                break;
            case Direction.Up:
                position += Vector2.down * _bumpIntense;
                break;
            case Direction.Down:
                position += Vector2.up * _bumpIntense;
                break;
            default:
                break;
        }
        return position;
    }
}
