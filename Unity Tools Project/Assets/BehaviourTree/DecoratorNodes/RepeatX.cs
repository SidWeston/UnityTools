using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatX : DecoratorNode
{
    public int timesToRepeat = 1;
    private int current;

    protected override void OnStart()
    {
        //ensure current starts at 0
        current = 0;
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        
        if(timesToRepeat == 0)
        {
            //cant repeat 0 times
            return State.Failure;
        }

        if(current <= timesToRepeat)
        {
            current++;
            child.Update();
            return State.Running;
        }

        return State.Success;
    }
}