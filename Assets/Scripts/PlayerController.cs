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

    [SerializeField]
    private Material glowObjectWhenCloseMaterial;

    [SerializeField]
    private Material glowTileWhenCloseMaterial;

    [SerializeField]
    private Material defaultMaterial;

    private bool isActionPressed = false;

    private GameObject placementVision;

    [SerializeField]
    private float timeForLongPressActionButton = 1;

    private float howMuchTimeTheActionButtonWasPressed = 0;

    private bool isReseting;
    private float resetTimer;
    public float goalResetTimer;

    public SliderFill dyingSlider;

    [SerializeField]
    private Vector2 maxPositionRange;
    [SerializeField]
    private Vector2 minPositionRange;
    [SerializeField]
    private SliderFill sliderActionButtonLongPress;

    Transform latestClosestObjectTransform = null;

    // Start is called before the first frame update
    void Start()
    {
        placementVision = new GameObject();
        placementVision.transform.parent = transform;
        placementVision.transform.localPosition = new Vector3(0,0,0);
        GeneralAttributes.Instance.ResetGame(transform);
        AudioManager.instance.Play("Background");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveCharacter();
        GetCurrentObjectOnButtonPressed();
        PlaceCurrentObjectOnButtonPressed();
        ClampPlayerInRange();
        if (Input.GetKey(KeyCode.Return))
        {
            foreach (Fading currentObject in FindObjectsOfType<Fading>())
            {
                currentObject.isDissolving = true;
                
            }
            isReseting = true;
        }
        CombineElements();
        TimeToReset();
        if(dyingSlider.value <= 0)
        {
            Destroy(gameObject);
        }
    }

    void GetCurrentObjectOnButtonPressed()
    {
        ///////////////////////////////////
        Transform closestObjectTransform = GetClosestObject();
        if(closestObjectTransform != latestClosestObjectTransform)
        {
            if (latestClosestObjectTransform != null)
            {
                Renderer latestClosestObjectRenderer = latestClosestObjectTransform.GetComponent<Renderer>();
                if (latestClosestObjectRenderer != null)
                {
                    InteractItem closestObject = latestClosestObjectRenderer.GetComponent<InteractItem>();
                    if (closestObject != null)
                    {
                        latestClosestObjectRenderer.material = closestObject.originalMaterial;
                    }
                    else
                    {
                        latestClosestObjectRenderer.material = defaultMaterial;
                    }
                }
            }

            if (closestObjectTransform != null)
            {
                Renderer closestObjectRenderer = closestObjectTransform.GetComponent<Renderer>();
                if (closestObjectRenderer != null)
                {
                    closestObjectRenderer.material = glowObjectWhenCloseMaterial;
                }
            }
            latestClosestObjectTransform = closestObjectTransform;
        }
        ///////////////////////////////////////////////////////////// 

        float actionButtonPressed = Input.GetAxisRaw("Action");

        if (actionButtonPressed > 0.3f)
        {
            isActionPressed = true;
        }
        else
        {
            isActionPressed = false;
        }

        if(howMuchTimeTheActionButtonWasPressed > timeForLongPressActionButton)
        {
            isActionPressed = false;
        }

        if (closestObjectTransform == null || GeneralAttributes.Instance.inventory.items.Contains(
            closestObjectTransform.GetComponent<InteractItem>()) 
            || !closestObjectTransform.tag.Contains("Pickable") 
            || GeneralAttributes.Instance.inventory.InventoryFull)
        {
            sliderActionButtonLongPress.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            sliderActionButtonLongPress.transform.parent.gameObject.SetActive(isActionPressed);
            sliderActionButtonLongPress.value = howMuchTimeTheActionButtonWasPressed;
        }
            

        if (isActionPressed == false)
        {
            if (howMuchTimeTheActionButtonWasPressed != 0)
            {
                //You should be able maybe to get a list of a close objects and change between them
                if (closestObjectTransform != null 
                    && !GeneralAttributes.Instance.inventory.items.Contains(closestObjectTransform.GetComponent<InteractItem>()))
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
                howMuchTimeTheActionButtonWasPressed = 0;
            }
        }
        else
        {
            howMuchTimeTheActionButtonWasPressed += Time.deltaTime;
        }
    }
    
    void PlaceCurrentObjectOnButtonPressed()
    {
        /////////////////////////////////////////////////////////////
        GridItem curentGridItem = GeneralAttributes.Instance.houseGrid.GetItemFromPosition(
                        placementVision.transform.position.x, placementVision.transform.position.y);
        if (!GeneralAttributes.Instance.inventory.InventoryEmpty)
        {
            if (curentGridItem != null && curentGridItem.Placeable)
            {
                if (GeneralAttributes.Instance.inventory.peekInteractItem() == null)
                {
                    GeneralAttributes.Instance.inventory.popInteractItem();
                    return;
                }
                var selectedInventoryItem = GeneralAttributes.Instance.inventory.peekInteractItem().transform;
                selectedInventoryItem.gameObject.SetActive(true);
                selectedInventoryItem.position = new Vector3(
                    curentGridItem.position.x, curentGridItem.position.y, selectedInventoryItem.position.z);

                Renderer closestTileRenderer = selectedInventoryItem.gameObject.GetComponent<Renderer>();
                if (closestTileRenderer != null)
                {
                    closestTileRenderer.material = glowTileWhenCloseMaterial;
                }
            }
            else
            {
                GeneralAttributes.Instance.inventory.peekInteractItem().gameObject.SetActive(false);
            }
        }
        ////////////////////////////////////////////////////////////

        float placeButtonPressed = Input.GetAxisRaw("Place");


        if (placeButtonPressed > 0.3f)
        {
            if (curentGridItem != null)
            {
                if (curentGridItem.Placeable)
                {
                    if (!GeneralAttributes.Instance.inventory.InventoryEmpty)
                    {
                        curentGridItem.objectPlaced = GeneralAttributes.Instance.inventory.popInteractItem().transform;
                        Renderer closestTileRenderer = curentGridItem.objectPlaced.GetComponent<Renderer>();
                        if (closestTileRenderer != null)
                        {
                            InteractItem closestObject = curentGridItem.objectPlaced.GetComponent<InteractItem>();
                            if (closestObject != null)
                            {
                                closestTileRenderer.material = closestObject.originalMaterial;
                            }
                            else
                            {
                                closestTileRenderer.material = defaultMaterial;
                            }
                        }
                        curentGridItem.objectPlaced.gameObject.SetActive(true);
                        if (curentGridItem.objectPlaced.gameObject.GetComponent<BoxCollider2D>() != null)
                        {
                            curentGridItem.objectPlaced.gameObject.GetComponent<BoxCollider2D>().enabled = true;
                        }
                        curentGridItem.objectPlaced.position = new Vector3(curentGridItem.position.x, curentGridItem.position.y,
                            curentGridItem.objectPlaced.transform.position.z);
                        AudioManager.instance.Play("Click");
                    }
                }
            }
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

    void ClampPlayerInRange()
    {
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, minPositionRange.x, maxPositionRange.x),
            Mathf.Clamp(transform.position.y, minPositionRange.y, maxPositionRange.y),
            transform.position.z
            );
    }

    void CombineElements()
    {
        if (Input.GetKey(KeyCode.C))
        {
            GeneralAttributes generalAttributes = GeneralAttributes.Instance;
            GridItem firstCombinationObject = generalAttributes.houseGrid.GetItemFromIndex(generalAttributes.firstElementToCombineIndex);
            GridItem secondCombinationObject = generalAttributes.houseGrid.GetItemFromIndex(generalAttributes.secondElementToCombineIndex);
            GridItem resultCombinationObject = generalAttributes.houseGrid.GetItemFromIndex(generalAttributes.resultElementIndex);

            if (firstCombinationObject.objectPlaced != null && secondCombinationObject.objectPlaced != null
                && resultCombinationObject.Placeable)
            {
                InteractItem firstCombinationItem = firstCombinationObject.objectPlaced.GetComponent<InteractItem>();
                InteractItem secondCombinationItem = secondCombinationObject.objectPlaced.GetComponent<InteractItem>();
                if (firstCombinationItem != null && secondCombinationItem != null)
                {
                    GameObject newGameObject;
                    if (!generalAttributes.combinationDictionary.TryGetValue(
                        new KeyValuePair<string, string>(firstCombinationItem.idName, secondCombinationItem.idName), 
                        out newGameObject))
                    {
                        if(!generalAttributes.combinationDictionary.TryGetValue(
                            new KeyValuePair<string, string>(secondCombinationItem.idName, firstCombinationItem.idName),
                            out newGameObject))
                        {
                            return;
                        }
                    }
                    var newObject = Instantiate(newGameObject, new Vector3(resultCombinationObject.position.x,
                            resultCombinationObject.position.y, transform.position.z), Quaternion.identity);
                    resultCombinationObject.objectPlaced = newObject.transform;
                    Destroy(firstCombinationItem.gameObject);
                    Destroy(secondCombinationItem.gameObject);
                    AudioManager.instance.Play("Click");
                }
            }
        }
    }

    void TimeToReset()
    {
        if(isReseting)
        {
            resetTimer += Time.deltaTime;
            foreach (Fading currentObject in FindObjectsOfType<Fading>())
            {
                if(!currentObject.isDissolving)
                {
                    currentObject.isDissolving = true;
                    currentObject.fade -= resetTimer/5;
                }
            }

            if (resetTimer > goalResetTimer)
            {
                dyingSlider.ResetSlider();
                GeneralAttributes.Instance.ResetGame(transform);
                isReseting = false;
                resetTimer = 0.0f;
            }
        }
    }

    private void OnDestroy()
    {
        //GameOver
    }
}
