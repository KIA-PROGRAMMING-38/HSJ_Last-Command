using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState : StateMachineBehaviour
{
    protected PlayerMovement _movement;
    protected Player _player;
    protected PlayerInput _input;
    public abstract void Move(GameObject Player);
}
