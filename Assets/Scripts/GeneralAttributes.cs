using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralAttributes
{
    public GridMap houseGrid = new GridMap(16, 1.0f);

    public Inventory inventory = new Inventory();

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
