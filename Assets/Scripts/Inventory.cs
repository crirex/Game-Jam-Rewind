using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private int maximumInventory = 1;

    public List<InteractItem> items = new List<InteractItem>();

    public bool InventoryFull => items.Count >= maximumInventory;

    public bool InventoryEmpty => items.Count <= 0;

    public Inventory(int maxInventoryCount = 1)
    {
        maximumInventory = maxInventoryCount;
    }

    public InteractItem popInteractItem(int index = 0)
    {
        InteractItem interactItem = null;
        if (index < items.Count)
        {
            interactItem = items[index];
            items.RemoveAt(index);
        }
        return interactItem;
    }

    public InteractItem peekInteractItem(int index = 0)
    {
        InteractItem interactItem = null;
        if (index < items.Count)
        {
            interactItem = items[index];
        }
        return interactItem;
    }
}
