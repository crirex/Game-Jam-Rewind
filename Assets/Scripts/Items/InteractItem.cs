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

        if (!isQuickPressInteractAvailable)
        {
            timeSinceLastInteractFinished += Time.deltaTime;
        }
        if (timeSinceLastInteractFinished > secondsBetweenInteracts)
        {
            isQuickPressInteractAvailable = true;
            timeSinceLastInteractFinished = 0.0f;
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
                            currentGridItem.position.y, transform.position.z), Quaternion.identity);
                        currentGridItem.objectPlaced = newObject.transform;
                        isQuickPressInteractAvailable = false;
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
            if (!GeneralAttributes.Instance.inventory.InventoryFull)
            {
                GeneralAttributes.Instance.inventory.items.Add(this);
                if(gameObject.GetComponent<BoxCollider2D>() != null)
                {
                    gameObject.GetComponent<BoxCollider2D>().enabled = false;
                }
                gameObject.SetActive(false);
            }
        }
    }
}
