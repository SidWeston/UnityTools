using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonMovement : MonoBehaviour
{

    [Header("First Person Movement")]
    //ref to character controller component
    [Header("Character Controller")]
    public CharacterController playerController;


    //variables to affect movement e.g movespeed is how fast the character walks
    [Header("Movement Variables")]
    [Range(5.0f, 12.0f)]
    public float walkSpeed;
    [Range(15.0f, 25.0f)]
    public float sprintSpeed;
    //move speed will either be set to walk or sprint depending on input
    private float moveSpeed;
    [Range(0.1f, 5.0f)]
    public float jumpHeight;

    //test if the character is on the ground
    [Header("Ground Check")]
    [SerializeField] private Transform groundCheckPos;
    [SerializeField] private LayerMask whatIsGround;

    //gravity
    private Vector3 velocity;
    private float gravity = -9.81f;

    //input variables
    //WASD converted to a vector2
    private Vector2 moveVector;
    //button presses converted to a float value
    private float sprinting;
    private float jumping;

    // Update is called once per frame
    void Update()
    {
        bool grounded = GroundCheck();

        //gravity and ground check
        //multiply gravity by delta time to keep it consistent amongst different frame rates
        velocity.y += gravity * Time.deltaTime;
        if(grounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        //apply gravity
        playerController.Move(velocity * Time.deltaTime);

        //Character Walking
        Vector3 movement = transform.right * moveVector.x + transform.forward * moveVector.y;
        MoveCharacter(movement * moveSpeed * Time.deltaTime);

        //sprinting
        if(sprinting > 0)
        {
            moveSpeed = sprintSpeed;
        }
        else
        {
            moveSpeed = walkSpeed;
        }

        //jumping
        if(jumping > 0 && grounded)
        {
            Jump();
        }
    }

    private void MoveCharacter(Vector3 moveDirection)
    {
        playerController.Move(moveDirection);
    }

    private void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    private bool GroundCheck()
    {
        //perform a test to see if the player is overlapping with the ground
        return Physics.CheckSphere(groundCheckPos.position, 0.25f, whatIsGround);
    }

    //input functions
    //these need to be public to be found in the editor
    public void OnMove(InputAction.CallbackContext context)
    {
        //return the input as a vector 2
        moveVector = context.ReadValue<Vector2>();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        //return button press as float
        sprinting = context.ReadValue<float>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //return button press as a float
        jumping = context.ReadValue<float>();
    }

}
