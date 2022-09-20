using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MovementAbility
{

    public float boostFactor = 2.0f; //the amount that the current movement speed is multiplied by

    private ThirdPersonMovement movementComponent;

    public override void Awake()
    {
        base.Awake();
        movementComponent = movementObj.GetComponent<ThirdPersonMovement>();
        if(!movementComponent)
        {
            Debug.LogError("The gameobject does not have a ThirdPersonMovement Component!");
        }
    }

    public override void ActivateAbility()
    {
        movementComponent.moveSpeed = movementComponent.sprintSpeed *= boostFactor;
        movementComponent.lockSpeed = true;
    }

    public override void DeactivateAbility()
    {
        movementComponent.lockSpeed = false;
    }
}
