using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static float SpeedModifier { get; set; }

    [SerializeField]
    private float speed = 0.2f;
    [SerializeField]
    private float maximumDistance = 0.3f;

    private Transform pickedObject = null;
    private bool isActionPressed = false;


    // Start is called before the first frame update
    void Start()
    {
        SpeedModifier = 1.0f;
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
            if (isActionPressed == false)
            {
                if (pickedObject != null)
                {
                    pickedObject.parent = null;
                    pickedObject = null;
                }
                else if (closestObjectTransform != null)
                {
                    pickedObject = closestObjectTransform;
                    closestObjectTransform.parent = transform;
                }
                isActionPressed = true;
            }
        }
        else
        {
            isActionPressed = false;
        }
    }

    void MoveCharacter()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
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
            float currentDistance = Vector2.Distance(new Vector2(transform.position.x, transform.position.y),
                new Vector2(currentObject.transform.position.x, currentObject.transform.position.y));
            
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
