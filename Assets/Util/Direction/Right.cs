using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util.Direction;

public class Right : Direction
{
    public Right()
    {
        this._moveDirection = Vector2.right;
        this._rotation = Quaternion.Euler(0,0,270);
    }
}