using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item: MonoBehaviour
{
    [SerializeField]
    protected bool isQuickPressInteractAvailable = false;
    [SerializeField]
    protected bool isLongPressInteractAvailable = false;

    public bool IsQuickPressInteractAvailable
    {
        get => isQuickPressInteractAvailable;
        private set => isQuickPressInteractAvailable = value;
    }

    public bool IsLongPressInteractAvailable
    {
        get => isLongPressInteractAvailable;
        private set => isLongPressInteractAvailable = value;
    }

    public abstract void QuickPressInteract();
    public abstract void LongPressInteract();

}
