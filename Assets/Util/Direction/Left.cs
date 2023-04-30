using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util.Direction;

public class Left : Direction
{
    public Left()
    {
        this._moveDirection = Vector2.left;
        this._rotation = Quaternion.Euler(0, 0, 90);
    }
}
