using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class FirstPersonRBController : MonoBehaviour
{

    [Header("First Person Movement Rigidbody")]
    [Header("Rigidbody Component")]
    public Rigidbody playerRb;

    [Header("Movement Variables")]
    public float walkSpeed = 5.0f;
    public float sprintSpeed = 12.0f;
    private float moveSpeed;
    public float maxPlayerVelocity = 12.0f;
    public float jumpHeight = 2.0f;

    private float dragForce;

    //input variables
    private Vector2 moveVector;
    private bool hasJumped;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = walkSpeed;
        dragForce = playerRb.drag;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = transform.right * moveVector.x + transform.forward * moveVector.y;
        if (movement.magnitude > 0) //if the movement vector is greater than 0, means one of the movement buttons is down
        {
            playerRb.AddForce(((movement * moveSpeed) * (100 * playerRb.mass)) * Time.deltaTime);
        }

        if(playerRb.velocity.magnitude > maxPlayerVelocity)
        {
            playerRb.drag = 100.0f;
        }
        else
        {
            if(playerRb.drag != dragForce)
            {
                playerRb.drag = dragForce;
            }
        }
    }

    private void FixedUpdate()
    {
        
    }

    private void Jump()
    {
        playerRb.AddForce(Vector3.up * jumpHeight * playerRb.mass);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveVector = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.ReadValue<float>() > 0)
        {
            if(!hasJumped)
            {
                hasJumped = true;
                Jump();
            }
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if(context.ReadValue<float>() > 0)
        {
            moveSpeed = sprintSpeed;
        }
        else
        {
            moveSpeed = walkSpeed;
        }
    }

}
