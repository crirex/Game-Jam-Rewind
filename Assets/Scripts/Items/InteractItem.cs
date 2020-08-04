using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractItem : MonoBehaviour
{
    [SerializeField]
    private bool isQuickPressInteractAvailable = false;

    public GameObject spawnObject;

    public bool IsQuickPressInteractAvailable
    {
        get => isQuickPressInteractAvailable;
        private set => isQuickPressInteractAvailable = value;
    }

    [SerializeField]
    private float secondsBetweenInteracts = 0.5f;

    private float timeSinceLastInteractFinished;

    private void Start()
    {

    }

    private void FixedUpdate()
    {
        if (!IsQuickPressInteractAvailable)
        {
            if (timeSinceLastInteractFinished + secondsBetweenInteracts < Time.time)
            {
                IsQuickPressInteractAvailable = true;
            }
        }
        else
        {
            timeSinceLastInteractFinished = Time.time;
        }
    }

    public void QuickPressInteract()
    {
        if(IsQuickPressInteractAvailable)
        {
            //Throw object on the ground. Object is assignable
        }
    }

    public void LongPressInteract(PlayerController player)
    {
        QuickPressInteract();
        player.inventory = this;
        //Debug.Log(player.inventory.transform.position);
        player.inventory.gameObject.SetActive(false);
    }
}
