using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum MoveMode
{ 
    WALK,
    HOVER
}

public class PowersuitController : BaseCharacterMovement
{

    public MoveMode currentMoveMode;

    private float targetSpeed;
    public float walkSpeed = 5.0f, sprintSpeed = 12.0f;
    public float accelerationRate = 20.0f;

    private Vector3 lastMoveDirection;

    [HideInInspector] public bool lockSpeed;

    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform meshTransform;
    [SerializeField] private float meshRotationSpeed = 5.0f;
    private float turnSmoothVelocity;

    private bool hoverUp = false;
    private float maxHoverUpSpeed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        currentMoveMode = MoveMode.WALK;
    }

    // Update is called once per frame
    protected override void Update()
    {
        //base handles gravity
        base.Update();

        if (currentMoveMode == MoveMode.WALK)
        {
            HandleWalk();
        }
        else if(currentMoveMode == MoveMode.HOVER)
        {
            if(hoverUp)
            {
                velocity.y = Mathf.Sqrt(jumpPower * -2f * gravity);
                if (velocity.y > maxHoverUpSpeed)
                {
                    velocity.y = maxHoverUpSpeed;
                }
            }
            else
            {
                //stop character from falling in hover mode
                if (velocity.y < 0)
                {
                    velocity.y = 0;
                }
            }

            HandleHover();
        }
    }

    private void HandleWalk()
    {
        //calculate the direction the player should move in based on the camera direction and WASD input
        float targetAngle = Mathf.Atan2(moveVector.x, moveVector.y) * Mathf.Rad2Deg + playerCamera.transform.eulerAngles.y;
        Vector3 directionMovement = Quaternion.Euler(0.0f, targetAngle, 0.0f) * Vector3.forward;

        //calculate the angle that the player should rotate to when moving, and smoothly rotate the player over time
        float angle = Mathf.SmoothDampAngle(meshTransform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 0.1f);

        //check if any movement keys are down before trying to add input direction
        if (movementInputDown)
        {
            if (moveSpeed < targetSpeed)
            {
                AccelerateSpeed();
            }
            else if (moveSpeed > targetSpeed)
            {
                DecelerateSpeed();
            }

            //set the player's rotation to be the direction they are moving in
            meshTransform.rotation = Quaternion.Euler(0f, angle, 0f);

            //move character along vector
            MoveCharacter(directionMovement);
            lastMoveDirection = directionMovement;
        }
        else if (!movementInputDown)
        {
            if (moveSpeed > 0)
            {
                DecelerateSpeed();
                MoveCharacter(lastMoveDirection);
            }
        }
    }

    private void HandleHover()
    {
        //calculate the direction the player should move in based on the camera direction and WASD input
        float targetAngle = Mathf.Atan2(moveVector.x, moveVector.y) * Mathf.Rad2Deg + playerCamera.transform.eulerAngles.y;
        Vector3 directionMovement = Quaternion.Euler(0.0f, targetAngle, 0.0f) * Vector3.forward;

        //calculate the angle that the player should rotate to when moving, and smoothly rotate the player over time
        float angle = Mathf.SmoothDampAngle(meshTransform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 0.1f);

        //check if any movement keys are down before trying to add input direction
        if (movementInputDown)
        {
            if (moveSpeed < targetSpeed)
            {
                AccelerateSpeed();
            }
            else if (moveSpeed > targetSpeed)
            {
                DecelerateSpeed();
            }

            //set the player's rotation to be the direction they are moving in
            meshTransform.rotation = Quaternion.Euler(0f, angle, 0f);

            //move character along vector
            MoveCharacter(directionMovement);
            lastMoveDirection = directionMovement;
        }
        else if (!movementInputDown)
        {
            if (moveSpeed > 0)
            {
                DecelerateSpeed();
                MoveCharacter(lastMoveDirection);
            }
        }
    }

    private void AccelerateSpeed()
    {
        moveSpeed += accelerationRate * Time.deltaTime;
        if (moveSpeed > targetSpeed)
        {
            moveSpeed = targetSpeed;
        }
    }

    private void DecelerateSpeed()
    {
        moveSpeed -= accelerationRate * 2 * Time.deltaTime;
        if (moveSpeed < targetSpeed)
        {
            moveSpeed = targetSpeed;
        }
    }

    protected override void HandleMove()
    {
        if(currentMoveMode == MoveMode.WALK)
        {
            if (targetSpeed == 0)
            {
                targetSpeed = walkSpeed;
            }

            if (sprintButtonDown)
            {
                targetSpeed = sprintSpeed;
            }
        }
        else if(currentMoveMode == MoveMode.HOVER)
        {
            if (targetSpeed == 0)
            {
                targetSpeed = walkSpeed;
            }

            if (sprintButtonDown)
            {
                targetSpeed = sprintSpeed;
            }
        }
    }

    protected override void HandleUnMove()
    {
        targetSpeed = 0;
    }

    protected override void HandleDoubleJumpPress()
    {
        if(currentMoveMode == MoveMode.WALK)
        {
            currentMoveMode = MoveMode.HOVER;
        }
        else if(currentMoveMode == MoveMode.HOVER)
        {
            currentMoveMode = MoveMode.WALK;
        }
    }

    protected override void HandleJump()
    {
        if(currentMoveMode == MoveMode.WALK)
        {
            if (jumpButtonDown && grounded)
            {
                velocity.y = Mathf.Sqrt(jumpPower * -2f * gravity);
            }
        }
        else if(currentMoveMode == MoveMode.HOVER)
        {
            hoverUp = true;
        }

    }

    protected override void HandleUnJump()
    {
        if(currentMoveMode == MoveMode.WALK)
        {

        }
        else if(currentMoveMode == MoveMode.HOVER)
        {
            hoverUp = false;
        }
    }

    protected override void HandleCrouch()
    {
        
    }

    protected override void HandleUnCrouch()
    {
        
    }

    protected override void HandleSprint()
    {
        if (movementInputDown)
        {
            targetSpeed = sprintSpeed;
        }
    }

    protected override void HandleUnSprint()
    {
        if (movementInputDown)
        {
            targetSpeed = walkSpeed;
        }
    }

}
