using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static float SpeedModifier { get; set; } = 1.0f;

    [SerializeField]
    private float speed = 0.2f;
    [SerializeField]
    private float maximumDistance = 0.3f;

    public InteractItem inventory = null;
    private bool isActionPressed = false;

    private bool InventoryFull => inventory != null;

    private GameObject placementVision;

    [SerializeField]
    private float timeForLongPressActionButton = 1;

    private float howMuchTimeTheActionButtonWasPressed = 0;

    // Start is called before the first frame update
    void Start()
    {
        placementVision = new GameObject();
        placementVision.transform.parent = transform;
        placementVision.transform.localPosition = new Vector3(0,0,0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveCharacter();
        GetCurrentObjectOnButtonPressed();
    }

    void GetCurrentObjectOnButtonPressed()
    {
        Transform closestObjectTransform = GetClosestObject();
        float actionButtonPressed = Input.GetAxisRaw("Action");

        if (actionButtonPressed > 0.3f)
        {
            isActionPressed = true;
        }
        else
        {
            isActionPressed = false;
        }

        if (isActionPressed == false)
        {
            if (howMuchTimeTheActionButtonWasPressed != 0)
            {
                //You should be able maybe to get a list of a close objects and change between them
                if (closestObjectTransform != null)
                {
                    var closestObjectInteractable = closestObjectTransform.GetComponent<InteractItem>();
                    if (closestObjectInteractable == null)
                    {
                        Debug.LogWarning("The object is interactive but it doesn't have any interact sript." +
                            "This might be a missuse of tags or the object is missing a script.");
                    }
                    else
                    {
                        if (howMuchTimeTheActionButtonWasPressed > timeForLongPressActionButton)
                        {
                            if (closestObjectInteractable.tag.Contains("Pickable"))
                            {
                                closestObjectInteractable.LongPressInteract(this);
                            }
                        }
                        else
                        {
                            if (closestObjectInteractable.tag.Contains("Interactable"))
                            {
                                closestObjectInteractable.QuickPressInteract();
                            }
                        }
                    }
                }
                else
                {
                    GridItem curentGridItem = GeneralAttributes.Instance.houseGrid.GetItemFromPosition(
                        placementVision.transform.position.x, placementVision.transform.position.y);
                    if ((curentGridItem?.Placeable).Value)
                    {
                        if (inventory != null)
                        {
                            curentGridItem.objectPlaced = inventory.transform;
                            curentGridItem.objectPlaced.gameObject.SetActive(true);
                            curentGridItem.objectPlaced.position = new Vector3(curentGridItem.position.x, curentGridItem.position.y, inventory.transform.position.z);
                            inventory = null;
                        }
                    }
                }
                howMuchTimeTheActionButtonWasPressed = 0;
            }
        }
        else
        {
            howMuchTimeTheActionButtonWasPressed += Time.deltaTime;
        }
    }

    void MoveCharacter()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
        if (moveHorizontal != 0 || moveVertical != 0)
        {
            Vector2 facingPosition = new Vector2(Mathf.RoundToInt(moveHorizontal),
                Mathf.RoundToInt(moveVertical));
            placementVision.transform.position = new Vector3(
                transform.position.x + GeneralAttributes.Instance.houseGrid.CellSize * facingPosition.x,
                transform.position.y + GeneralAttributes.Instance.houseGrid.CellSize * facingPosition.y,
                placementVision.transform.localPosition.z
                );
        }

        Vector3 moveDirection = new Vector3(moveHorizontal, moveVertical, 0);
        transform.position += moveDirection.normalized * speed * SpeedModifier;
    }

    Transform GetClosestObject()
    {
        List<GameObject> objects = new List<GameObject>();
        objects.AddRange(GameObject.FindGameObjectsWithTag("InteractableObject"));
        objects.AddRange(GameObject.FindGameObjectsWithTag("PickableObject"));
        objects.AddRange(GameObject.FindGameObjectsWithTag("PickableInteractableObject"));
        float distance = float.MaxValue;
        GameObject closestGameObject = null;
        foreach(GameObject currentObject in objects)
        {
            float currentDistance = Vector2.Distance(
                new Vector2(transform.position.x, transform.position.y),
                new Vector2(currentObject.transform.position.x, currentObject.transform.position.y)
                );
            
            if (currentDistance < distance)
            {
                closestGameObject = currentObject;
                distance = currentDistance;
            }
        }
        if (distance < maximumDistance)
        {
            return closestGameObject.transform;
        }
        else
        {
            return null;
        }
    }
}
