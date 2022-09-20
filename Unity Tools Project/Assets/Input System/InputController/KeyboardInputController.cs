using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyboardInputController : MonoBehaviour
{

    public bool movementButtonDown;
    public bool sprintButtonDown;
    public bool crouchButtonDown;
    public bool jumpButtonDown;

    public Vector2 moveVector;

    public void OnMove(InputAction.CallbackContext context)
    {
        moveVector = context.ReadValue<Vector2>();
        if(moveVector.x > 0 || moveVector.y > 0)
        {
            movementButtonDown = true;
        }
        else
        {
            movementButtonDown = false;
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        float input = context.ReadValue<float>();
        if(input > 0)
        {
            sprintButtonDown = true;
        }
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        float input = context.ReadValue<float>();
        if (input > 0)
        {
            crouchButtonDown = true;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        float input = context.ReadValue<float>();
        if (input > 0)
        {
            jumpButtonDown = true;
        }
    }

}
