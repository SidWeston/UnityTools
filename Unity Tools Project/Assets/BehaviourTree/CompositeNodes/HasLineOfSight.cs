using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasLineOfSight : CompositeNode
{
    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if(controller.GetSightTarget() == true)
        {
            if(children[0] != null)
            {
                children[0].Update();
            }
            else
            {
                return State.Failure; 
            }
        }
        else
        {
            if(children[1] != null)
            {
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
