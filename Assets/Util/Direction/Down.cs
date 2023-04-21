using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util.Direction;
public class Down : Direction
{
    public Down()
    {
        this._moveDirection = Vector2.down;
        this._rotation = Quaternion.Euler(0, 0, 180);
    }
}
