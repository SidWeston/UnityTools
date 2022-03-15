using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//state based AI
public enum AIState
{
    WANDER,
    PATROL,
    GUARD,
    CHASE,
    SEARCH
}

/// <summary>
/// THIS IMPLEMENTATION IS FOR A GUARD AI
/// IF NEEDED FOR ANOTHER KIND OF AI, COPY THE SCRIPT AND CHANGE IT TO FIT THE NEEDS OF THE PROJECT
/// DONT BE A CUNT SID SAVE THE ORIGINAL STUFF 
/// </summary>


public class AIController : MonoBehaviour
{
    public AIState currentState = AIState.PATROL;

    //references to the view sensor and pathfinding unit for easy communication
    private AISensor viewSensor;
    private Unit pathfindingUnit;
    private AIWorldInfo worldInfo;

    public PatrolPath patrolPath;
    private int patrolPathIndex = 0;
    public float pointWaitTimer = 1.0f;

    //potential target for the ai to chase, will be set upon a target entering line of sight of the ai
    private GameObject aiTarget;

    [Tooltip("How quickly should the ai evaluate a choice? Lower value = faster evaluation")]
    public float evaluationTimer = 0.1f;
    private float maxEvaluationTimer;

    private bool hasNextPoint = false;

    // Start is called before the first frame update
    void Start()
    {
        viewSensor = GetComponent<AISensor>();
        pathfindingUnit = GetComponent<Unit>();

        maxEvaluationTimer = evaluationTimer;

        pathfindingUnit.target = patrolPath.patrolPoints[0].transform;

    }

    // Update is called once per frame
    void Update()
    {
        if(evaluationTimer > 0)
        {
            evaluationTimer -= Time.deltaTime;
        }
        else if(evaluationTimer <= 0)
        {
            EvaluateAIChoice();
            evaluationTimer = maxEvaluationTimer;
        }
    }

    private void EvaluateAIChoice()
    {
        switch (currentState)
        {
            case AIState.WANDER:
                {
                    break;
                }
            case AIState.PATROL:
                {
                    //if the unit is moving to a point already no need to do anything
                    if(pathfindingUnit.isMoving)
                    {
                        //if(GetSightTarget())
                        //{
                        //    currentState = AIState.CHASE;
                        //}
                        break;
                    }
                    else if(!pathfindingUnit.isMoving)
                    {
                        //if not moving, transition to guard
                        currentState = AIState.GUARD;
                    }

                    break;
                }
            case AIState.GUARD:
                {
                    if(GetSightTarget())
                    {
                        currentState = AIState.CHASE;
                    }

                    if(!IsInvoking())
                    {
                        Invoke("GetNextPatrolPoint", pointWaitTimer);
                        hasNextPoint = true;
                    }
                    
                    break;
                }
            case AIState.CHASE:
                {
                    break;
                }
            case AIState.SEARCH:
                {
                    break;
                }
        }
    }

    private void GetNextPatrolPoint()
    {
        if(patrolPathIndex >= patrolPath.patrolPoints.Count - 1)
        {
            //reset path index
            patrolPathIndex = 0;
        }
        else
        {
            patrolPathIndex++;
        }
        //set the target to be the next patrol point in the array
        pathfindingUnit.target = patrolPath.patrolPoints[patrolPathIndex].transform;
        //request a new path to that target
        pathfindingUnit.RequestNewPath();
        currentState = AIState.PATROL;
    }

    private void GetTargetLastLocation()
    {
        GameObject targetLastLocation = aiTarget;
        pathfindingUnit.StartChaseObject(targetLastLocation);

    }

    private void ResetPath()
    {
        pathfindingUnit.ResetFollowPath();
    }

    private bool GetSightTarget()
    {
        //if there is an object in the view sensor
        if(viewSensor.currentObjects.Count > 0)
        { 
            //find the parent object and set that as the target object
            if(viewSensor.currentObjects[0].transform.parent == null)
            {
                aiTarget = viewSensor.currentObjects[0];
            }
            else
            {
                aiTarget = viewSensor.currentObjects[0].transform.parent.gameObject;
            }
            pathfindingUnit.StartChaseObject(aiTarget);
            return true;
        }
        return false;
    }

    public void ObjectLeftSight()
    {
        
    }

}
