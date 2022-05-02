using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasLineOfSight : DecoratorNode
{
    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if (controller.GetSightTarget() == true)
        {
            if(child != null)
            {
                child.Update();
            }
            else
            {
                return State.Failure;
            }
        }
        else
        {
            if (child != null)
            {
                //if child is running end it
                if(child.state == State.Running)
                {
                    child.state = State.Success;
                }
            }
            else
            {
                return State.Failure;
            }
        }

        return State.Success;
    }
}
