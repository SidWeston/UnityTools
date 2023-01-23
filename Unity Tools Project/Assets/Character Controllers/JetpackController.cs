using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
enum MovementMode
{
    WALK,
    HOVER,
    NUMOFMODES
}

public class JetpackController : MonoBehaviour
{
    [Header("Physics Stuff")]
    public Rigidbody playerRB;

    [Header("Character Speeds")]
    public float walkSpeed, hoverSpeed, flySpeed;
    public float hoverUpSpeed, hoverDownSpeed;
    public float jumpPower;
    private float turnSmoothTime = 0.1f, turnSmoothVelocity;

    [Header("Camera")]
    public Camera playerCamera;

    [Header("Ground Check")]
    //ground check
    private bool grounded = false, hasJumped = false;
    [SerializeField]
    private LayerMask whatIsGround;
    [SerializeField]
    private GameObject groundCheckLocation;

    private MovementMode currentMoveMode;

    //input variables
    private Vector2 inputVector;
    private bool jumpButtonDown;
    private int downCounter = 0;
    private float lastJumpPress = 0, doublePressInterval = 0.25f;
    private bool crouchButtonDown;
    private bool sprintButtonDown;
    private float jumping;
    private float crouching;
    private float sprinting;

    // Start is called before the first frame update
    void Start()
    {
        currentMoveMode = MovementMode.WALK;
    }

    //fixed update for physics stuff
    private void FixedUpdate()
    {
        //ground check
        //if player is on the ground they are able to jump
        grounded = Physics.CheckSphere(groundCheckLocation.transform.position, 0.25f, whatIsGround);

        MoveCharacter(inputVector);
    }

    public void MoveCharacter(Vector2 movementInput)
    {
        Vector3 movementVector = new Vector3(movementInput.x, 0, movementInput.y);

        switch (currentMoveMode)
        {
            case MovementMode.WALK:
                {
                    Walk(movementVector);
                    break;
                }
            case MovementMode.HOVER:
                {
                    Hover(movementVector);
                    break;
                }
            default:
                {
                    break;
                }
        }

    }

    private void Walk(Vector3 movementVector)
    {
        float targetAngle = Mathf.Atan2(movementVector.x, movementVector.z) * Mathf.Rad2Deg + playerCamera.transform.eulerAngles.y;
        Vector3 directionMovement = Quaternion.Euler(0.0f, targetAngle, 0.0f) * Vector3.forward;
        //float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        //SetPlayerRotation(Quaternion.Euler(0f, angle, 0f));
        if (inputVector.x != 0 || inputVector.y != 0)
        {
            //multiplied by mass to move at the same speed regardless of mass
            playerRB.AddForce(directionMovement * walkSpeed * playerRB.mass);
        }

    }

    private void Hover(Vector3 movementVector)
    {
        //if the player is falling fast enough get rid of hovering upward force to improve momentum
        if (playerRB.velocity.y > -10)
        {
            float yVel = playerRB.velocity.y + Physics.gravity.y;
            playerRB.AddForce(0, -yVel, 0, ForceMode.Acceleration); //applies a force equal to the y velocity and gravity to make sure the player floats
        }

        //if holding the sprint button
        if (sprinting > 0 && inputVector.y > 0)
        {
            
            playerRB.AddForce(playerCamera.transform.forward * flySpeed * playerRB.mass);
        }
        //if not holding the sprint button
        else
        {
            if (jumping > 0)
            {
                playerRB.AddForce(0, hoverUpSpeed, 0, ForceMode.Acceleration);
            }
            else if (crouching > 0)
            {
                playerRB.AddForce(0, -hoverDownSpeed, 0, ForceMode.Acceleration);
            }

            float targetAngle = Mathf.Atan2(movementVector.x, movementVector.z) * Mathf.Rad2Deg + playerCamera.transform.eulerAngles.y;
            Vector3 directionMovement = Quaternion.Euler(0.0f, targetAngle, 0.0f) * Vector3.forward;
            if (inputVector.x != 0 || inputVector.y != 0)
            {
                playerRB.AddForce(directionMovement * hoverSpeed * playerRB.mass);
            }
        }

    }

    private void SetPlayerRotation(Quaternion targetRotation)
    {
        transform.rotation = targetRotation;
    }

    private void HandleJump()
    {
        if(currentMoveMode == MovementMode.WALK)
        {
            if (grounded && !hasJumped)
            {
                //jump will be the same height regardless of mass
                playerRB.AddForce(0, jumpPower * playerRB.mass, 0, ForceMode.Impulse);
                hasJumped = true;
                //reset jump after 1 second has passed
                Invoke("ResetJump", 1);
            }
        }
        else if(currentMoveMode == MovementMode.HOVER)
        {
            playerRB.AddForce(0, hoverUpSpeed, 0, ForceMode.Acceleration);
        }
    }

    private void HandleDoubleJumpPress()
    {
        if (currentMoveMode == MovementMode.WALK)
        {
            currentMoveMode = MovementMode.HOVER;
        }
        else if (currentMoveMode == MovementMode.HOVER)
        {
            currentMoveMode = MovementMode.WALK;
        }
    }

    private void ResetJump()
    {
        hasJumped = false;
    }

    //player movement input function
    public void OnMove(InputAction.CallbackContext context)
    {
        inputVector = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        float input = context.ReadValue<float>();
        if (input > 0 && downCounter == 0)
        {
            jumpButtonDown = true;
            HandleJump();
            downCounter++;
            //if lastJumpPress is 0 the current press cannot be the second press, therefore no double press possible
            if (lastJumpPress != 0)
            {
                //check if the current press was pressed within the time interval of the last press
                if (Time.time - lastJumpPress <= doublePressInterval)
                {
                    HandleDoubleJumpPress();
                }
            }
        }
        else if (input <= 0)
        {
            lastJumpPress = Time.time;
            jumpButtonDown = false;
            //HandleUnJump();
            downCounter = 0;
        }
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        crouching = context.ReadValue<float>();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        sprinting = context.ReadValue<float>();
    }

    public void ChangeMoveMode(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0)
        {
            currentMoveMode = MovementMode.HOVER;
        }
        else if (context.ReadValue<float>() < 0)
        {
            currentMoveMode = MovementMode.WALK;
        }
    }
}
