using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralAttributes
{
    private static GeneralAttributes instance = null;

    public GridMap houseGrid = new GridMap(10, 1.0f);

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
