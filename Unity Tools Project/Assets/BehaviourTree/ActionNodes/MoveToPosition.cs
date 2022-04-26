using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPosition : ActionNode
{

    private bool targetSet = false;
    private bool pathFound = false;

    protected override void OnStart()
    {
        targetSet = false;
        pathFound = false;
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {   
        if(blackboard.targetPosition != null && !targetSet && controller != null)
        {
            controller.SetTarget(blackboard.targetPosition);
            targetSet = true;
        }
        else if(blackboard.targetPosition == null)
        {
            return State.Failure;
        }

        if (!controller.pathfindingUnit.isMoving && pathFound)
        {
            return State.Success;
        }
        else if(controller.pathfindingUnit.isMoving)
        {
            pathFound = true;
        }


        return State.Running;

    }
}
