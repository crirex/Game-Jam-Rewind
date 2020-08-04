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
        if (IsQuickPressInteractAvailable && tag.Contains("Interactable"))
        {
            timeSinceLastInteractFinished = 0;
            //Throw object on the ground. Object is assignable
            Queue<GridItem> availableSpawnPoints = new Queue<GridItem>();
            GridItem spawnPointGridItem = GeneralAttributes.Instance.houseGrid.GetItemFromPosition(
                transform.position.x, transform.position.y);
            if(spawnPointGridItem == null)
            {
                Debug.LogWarning("Can't spawn object, this object isn't standing in a valid area.");
                return;
            }
            availableSpawnPoints.Enqueue(spawnPointGridItem);

            while(availableSpawnPoints.Count > 0 && availableSpawnPoints.Count < (GeneralAttributes.Instance.houseGrid.TotalSize / 2)) //Magical number
            {
                var currentGridItem = availableSpawnPoints.Dequeue();
                if (currentGridItem != null)
                {
                    if (currentGridItem.Placeable)
                    {
                        var newObject = Instantiate(spawnObject, new Vector3(currentGridItem.position.x,
                            currentGridItem.position.y, 0), Quaternion.identity);
                        currentGridItem.objectPlaced = newObject.transform;
                        return;
                    }
                    else
                    {
                        var currentGridItemIndex = GeneralAttributes.Instance.houseGrid.getIndexFromPosition(
                            new Vector2(currentGridItem.position.x, currentGridItem.position.y));
                        availableSpawnPoints.Enqueue(GeneralAttributes.Instance.houseGrid.GetItemFromIndex(
                            currentGridItemIndex + new Vector2Int(0, 1)));
                        availableSpawnPoints.Enqueue(GeneralAttributes.Instance.houseGrid.GetItemFromIndex(
                            currentGridItemIndex + new Vector2Int(0, -1)));
                        availableSpawnPoints.Enqueue(GeneralAttributes.Instance.houseGrid.GetItemFromIndex(
                            currentGridItemIndex + new Vector2Int(1, 0)));
                        availableSpawnPoints.Enqueue(GeneralAttributes.Instance.houseGrid.GetItemFromIndex(
                            currentGridItemIndex + new Vector2Int(-1, 0)));

                    }
                }
                
            }
            Debug.LogWarning("Can't spawn object, this object isn't standing in a valid area.");
        }
    }

    public void LongPressInteract(PlayerController player)
    {
        if (tag.Contains("Pickable"))
        {
            QuickPressInteract();
            GeneralAttributes.Instance.inventory.items.Add(this);
            gameObject.SetActive(false);
        }
    }
}
