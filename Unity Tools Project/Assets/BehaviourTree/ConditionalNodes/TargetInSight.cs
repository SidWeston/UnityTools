using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetInSight : ConditionalNode
{
    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if (shouldFinish)
        {
            for (int i = 0; i < children.Count; i++)
            {
                children[i].ForceFinish();
            }
            return State.Success;
        }

        if (controller.GetSightTarget() == true)
        {
            blackboard.aiTarget = controller.viewSensor.currentObjects[0];
            if (children[0] != null)
            {
                if(children[1].started)
                {
                    children[1].ForceFinish();
                }
                children[0].Update();
            }
            else
            {
                return State.Failure;
            }
        }
        else
        {
            if (children[1] != null)
            {
                if(children[0].started)
                {
                    children[0].ForceFinish();
                }
                children[1].Update();
            }
            else
            {
                return State.Failure;
            }
        }

        return State.Success;
    }
}
