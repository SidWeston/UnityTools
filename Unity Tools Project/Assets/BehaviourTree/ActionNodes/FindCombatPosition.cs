using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindCombatPosition : ActionNode
{

    private float distanceToTarget;

    protected override void OnStart()
    {
        if(blackboard.aiTarget != null)
        {
            distanceToTarget = Vector3.Distance(controller.transform.position, blackboard.aiTarget.transform.position);
        }
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {

        if (distanceToTarget > controller.viewSensor.sensorDistance - (controller.viewSensor.sensorDistance / 10))
        {
            blackboard.targetPosition = Vector3.Lerp(controller.transform.position, blackboard.aiTarget.transform.position, 0.5f);
        }
        else if (distanceToTarget < controller.viewSensor.sensorDistance / 2)
        {
            blackboard.targetPosition = (controller.transform.forward * (controller.viewSensor.sensorDistance / 2));
        }

        return State.Success;
    }
}
