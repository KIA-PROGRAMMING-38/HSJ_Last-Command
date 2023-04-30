using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState : StateMachineBehaviour
{
    protected ObjectManager _objectManager;
    protected PlayerMovement _movement;
    protected Player _player;
    protected PlayerInput _input;
    protected PlayerEffect _effect;

    public void InitSettings(Animator player)
    {
        if (_player == null)
        {
            _player = player.GetComponent<Player>();
        }
        if (_objectManager == null)
        {
            _objectManager = _player._objectManager;
        }
        if (_input == null)
        {
            _input = _player.GetComponent<PlayerInput>();
        }
        if (_movement == null)
        {
            _movement = _player.GetComponent<PlayerMovement>();
        }
        if(_effect == null)
        {
            _effect = _player.GetComponent<PlayerEffect>();
        }
    }

    public abstract void Move();
}
