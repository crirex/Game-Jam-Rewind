using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static float SpeedModifier { get; set; }

    [SerializeField]
    private float speed = 0.2f;
    private Transform playerTrasform;

    // Start is called before the first frame update
    void Start()
    {
        playerTrasform = gameObject.transform;
        SpeedModifier = 1.0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
        Vector3 moveDirection = new Vector3(moveHorizontal, moveVertical, 0);
        playerTrasform.position += moveDirection.normalized * speed * SpeedModifier;
    }
}
