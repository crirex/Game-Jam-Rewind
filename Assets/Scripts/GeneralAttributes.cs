using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralAttributes
{
    public GridMap houseGrid = new GridMap(15, 9, 1.0f, new Vector2(1,1));

    public Inventory inventory = new Inventory();

    public Vector2 safePositionMax = new Vector2(11, 8);
    public Vector2 safePositionMin = new Vector2(6, 3);

    private static GeneralAttributes instance = null;
    public static GeneralAttributes Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new GeneralAttributes();
            }
            return instance;
        }
    }
}
