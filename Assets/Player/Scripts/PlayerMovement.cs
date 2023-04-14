using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util.Direction;

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
        position += _input._playerDirection._moveDirection * -_bumpIntense;
        return position;
    }
}
