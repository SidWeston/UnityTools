using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ParkourController : MonoBehaviour
{

    //components
    private CharacterController playerController;

    //variables
    private float moveSpeed, targetSpeed;
    public float walkSpeed = 8.0f, sprintSpeed = 12.0f;
    public float accerlationRate = 20.0f, decelerationRate = 25.0f;
    public float jumpPower = 10.0f;
    private Vector3 lastMoveDirection;

    //ground check
    private bool grounded = false;
    [SerializeField] private Transform groundCheckLocation;
    [SerializeField] private LayerMask whatIsGround;
    private Vector3 velocity;
    private float gravity = -9.81f;

    //jumping/double jumping
    private int jumpCounter;

    //input variables
    private Vector2 moveVector;
    private bool movementInputDown = false;
    private bool jumpButtonDown = false;
    private bool sprintButtonDown = false;

    private void Awake()
    {
        SetupComponents();
    }

    // Start is called before the first frame update
    void Start()
    {
        //assuming player starts stationary
        moveSpeed = 0;
        targetSpeed = 0;
    }

    private void SetupComponents()
    {
        playerController = GetComponent<CharacterController>();
        if(!playerController)
        {
            Debug.LogError("There is no character controller associated with this class");
        }
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.CheckSphere(groundCheckLocation.position, 0.15f, whatIsGround);
        //increase y velocity by gravity every frame
        velocity.y += gravity * Time.deltaTime;
        //if the player is grounded, reset y velocity
        if(grounded && velocity.y < 0)
        {
            velocity.y = 0f;
            jumpCounter = 0;
        }
        //apply gravity/velocity
        playerController.Move(velocity * Time.deltaTime);

        HandleWalk();
    }

    private void HandleWalk()
    {
        //move character
        Vector3 movement = transform.right * moveVector.x + transform.forward * moveVector.y;
        if(movementInputDown)
        {
            //accelerate speed if needed
            if(moveSpeed < targetSpeed)
            {
                AccelerateSpeed();
            }
            //move character
            playerController.Move(movement * moveSpeed * Time.deltaTime);
            //record last movement direction if player stops giving input
            lastMoveDirection = movement;
        }
        else if(!movementInputDown)
        {
            //if player still needs to decelerate, decrease speed but still move character slightly
            if(moveSpeed > 0)
            {
                DecelerateSpeed();
                playerController.Move(lastMoveDirection * moveSpeed * Time.deltaTime);
            }
        }

    }

    private void AccelerateSpeed()
    {
        moveSpeed += accerlationRate * Time.deltaTime;
        Mathf.Clamp(moveSpeed, 0, targetSpeed);
    }

    private void DecelerateSpeed()
    {
        moveSpeed -= decelerationRate * 2 * Time.deltaTime;
        Mathf.Clamp(moveSpeed, 0, targetSpeed);
    }

    //input functions
    public void OnMove(InputAction.CallbackContext context)
    {
        moveVector = context.ReadValue<Vector2>();
        //check if any movement buttons are pressed down
        //x or y can be between -1 and 1, 
        //they will only be 0 if there is no input
        if (moveVector.x == 0 && moveVector.y == 0)
        {
            movementInputDown = false;
            targetSpeed = 0;
        }
        else
        {
            movementInputDown = true;
            if(targetSpeed == 0)
            {
                targetSpeed = walkSpeed;
            }

            if(sprintButtonDown)
            {
                targetSpeed = sprintSpeed;
            }
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();
        if(value > 0 && jumpCounter < 2 && !jumpButtonDown)
        {
            velocity.y = Mathf.Sqrt(jumpPower * -2f * gravity);
            jumpButtonDown = true;
            jumpCounter++;
        }
        else if(value <= 0)
        {
            jumpButtonDown = false;
        }
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        //crouching
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        //sprinting
        float value = context.ReadValue<float>();
        //value will only be greater than 0 when sprint button is pressed down
        if(value > 0)
        {
            targetSpeed = sprintSpeed;
            sprintButtonDown = true;
        }
        else
        {
            targetSpeed = walkSpeed;
            sprintButtonDown = false;
        }
    }

}
