using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindNextWaypoint : ActionNode
{
    private List<GameObject> patrolPoints;
    private int waypointIndex = 0;

    protected override void OnStart()
    {
        patrolPoints = controller.patrolPath.patrolPoints;
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

        if (patrolPoints == null)
        {
            //will fail if there is no array to get points from
            return State.Failure;
        }

        //set the target position to be the next waypoint position 
        blackboard.targetPosition = patrolPoints[waypointIndex].transform.position;
        
        if (waypointIndex >= patrolPoints.Count - 1)
        {
            //if the end of the list has been reached, reset back to the start
            waypointIndex = 0;
        }
        else 
        {
            //otherwise, increment to the next point on the list
            waypointIndex++;
        }

        return State.Success;
    }
}
