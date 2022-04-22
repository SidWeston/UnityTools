using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPosition : ActionNode
{

    private bool targetSet = false;

    protected override void OnStart()
    {
        targetSet = false;
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {   
        if(blackboard.targetObject != null && !targetSet)
        {
            controller.SetTarget(blackboard.targetObject.transform);    
        }
        else
        {
            return State.Failure;
        }

        if(controller.pathfindingUnit.isMoving)
        {
            return State.Running;
        }

        return State.Success;

    }
}
