using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{
    private new Rigidbody2D rigidbody;
    [HideInInspector]
    public Vector3 movementVector;
    [HideInInspector]
    public float lastHorizontalVector;
    [HideInInspector]
    public float lastVerticalVector;

    [SerializeField]
    float speed;

    Animate animate;
    // Start is called before the first frame update
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        movementVector = new Vector3();
        animate = GetComponent<Animate>();
    }
    private void Start()
    {
        lastHorizontalVector = -1f;
        lastVerticalVector = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        movementVector.x = Input.GetAxisRaw("Horizontal");
        movementVector.y = Input.GetAxisRaw("Vertical");

        if(movementVector.x != 0)
        {
            lastHorizontalVector = movementVector.x; 
        }
        if(movementVector.y != 0)
        {
            lastVerticalVector = movementVector.y; 
        }

        animate.horizontal = movementVector.x;

        movementVector *= speed;
        rigidbody.velocity = movementVector;
    }
}
