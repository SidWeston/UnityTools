using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartShooting : ActionNode
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
            return State.Success;
        }

        if (!blackboard.shooting)
        {
            controller.unitWeapon.StartCoroutine("FireWeapon");
            blackboard.shooting = true;
        }
        return State.Success;
    }
}
