using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementAbility : AbilityBase
{

    protected GameObject movementObj;

    public virtual void Awake()
    {
        movementObj = transform.root.gameObject;
        if(!movementObj)
        {
            Debug.LogError("The movement object cannot be found");
        }
    }

    public override void ActivateAbility()
    {
        
    }

    public override void DeactivateAbility()
    {
        
    }
}
