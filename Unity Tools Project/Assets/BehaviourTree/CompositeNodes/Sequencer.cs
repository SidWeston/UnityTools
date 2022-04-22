using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequencer : CompositeNode
{

    int current;

    protected override void OnStart()
    {
        //ensure current index is at 0 when node starts
        current = 0;
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        var child = children[current];
        switch (child.Update())
        {
            //switch based on the state of the current child
            case State.Running:
                {
                    return State.Running;
                }
            case State.Failure:
                {
                    return State.Failure;
                }
            case State.Success:
                {
                    //when one child finishes, increment to the next child
                    current++;
                    break;
                }
        }

        //if finsished the final child
        return current == children.Count ? State.Success : State.Running;

    }
}
