using UnityEngine;
using Util.Direction;

public class Up : Direction
{
    public Up()
    {
        this._moveDirection = Vector2.up;
        this._rotation = Quaternion.Euler(0, 0, 0);
    }
}
