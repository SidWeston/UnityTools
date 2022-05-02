using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopShooting : ActionNode
{
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if(blackboard.shooting)
        {
            controller.unitWeapon.StopCoroutine("FireWeapon");
            blackboard.shooting = false;
        }
        return State.Success;
    }
}
