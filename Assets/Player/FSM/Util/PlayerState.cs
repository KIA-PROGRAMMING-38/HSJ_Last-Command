using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState : StateMachineBehaviour
{
    protected PlayerMovement _playerMovement;
    public abstract void Move(GameObject Player);
}
