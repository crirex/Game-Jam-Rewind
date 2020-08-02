using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridItem
{
    public Transform objectPlaced = null;
    public Vector2 position;
    public bool Placeable
    {
        get
        {
            return objectPlaced == null;
        }
    }
    public GridItem(Vector2 pos)
    {
        position = pos;
    }

    public GridItem(float x, float y)
    {
        position = new Vector2(x, y);
    }
}
