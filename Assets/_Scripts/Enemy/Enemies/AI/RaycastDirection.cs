using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class RaycastDirection{
    
    public Direction direction;
    [NonSerialized] public Vector2 vector;
    public RaycastHit2D raycast;
    public bool isWallColliding = false;

    public RaycastDirection(Direction direction)
    {
        this.direction = direction;

        switch (this.direction)
        {
            case Direction.up:
                vector = Vector2.up;
                break;
            case Direction.down:
                vector = Vector2.down;
                break;
            case Direction.left:
                vector = Vector2.left;
                break;
            case Direction.right:
                vector = Vector2.right;
                break;
        }
    }
    
}
