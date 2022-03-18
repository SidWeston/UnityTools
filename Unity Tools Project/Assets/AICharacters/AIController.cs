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
    SEARCH,
    COVER
}

public class AIController : MonoBehaviour
{
    public AIState currentState = AIState.PATROL;

    //references to the view sensor and pathfinding unit for easy communication
    private AISensor viewSensor;
    private Unit pathfindingUnit;
    private AIWorldInfo worldInfo;
    private AIHealth unitHealth;

    public PatrolPath patrolPath;
    private int patrolPathIndex = 0;
    public float pointWaitTimer = 1.0f;

    //potential target for the ai to chase, will be set upon a target entering line of sight of the ai
    private GameObject aiTarget;

    [Tooltip("How quickly should the ai evaluate a choice? Lower value = faster evaluation")]
    public float evaluationTimer = 0.1f;
    private float maxEvaluationTimer;

    private void Awake()
    {
        worldInfo = GameObject.FindGameObjectWithTag("WorldInfo").GetComponent<AIWorldInfo>();
        viewSensor = GetComponent<AISensor>();
        pathfindingUnit = GetComponent<Unit>();
        pathfindingUnit.target = patrolPath.patrolPoints[0].transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        maxEvaluationTimer = evaluationTimer;
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
                        if(GetSightTarget())
                        {
                            currentState = AIState.CHASE;
                        }
                        break;
                    }
                    else if(!pathfindingUnit.isMoving)
                    {
                        //if not moving, transition to guard
                        currentState = AIState.GUARD;
                    }

                    if(unitHealth.currentHealth < unitHealth.maxHealth)
                    {
                        
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
                    }
                    
                    break;
                }
            case AIState.CHASE:
                {
                    //if there isnt a target in line of sight the ai will go to guard mode
                    if(!GetSightTarget())
                    {
                        currentState = AIState.GUARD;
                        pathfindingUnit.UpdatePath(false);
                        pathfindingUnit.shouldRotateNextPoint = true;
                    }

                    break;
                }
            case AIState.SEARCH:
                {
                    break;
                }
            case AIState.COVER:
                {
                    pathfindingUnit.target = GetNearestCoverPoint().transform;
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
            pathfindingUnit.target = aiTarget.transform;
            pathfindingUnit.UpdatePath(true);
            pathfindingUnit.shouldRotateNextPoint = false;
            return true;
        }
        return false;
    }

    private GameObject GetNearestCoverPoint()
    {
        GameObject coverPoint;

        coverPoint = worldInfo.GetComponent<AIWorldInfo>().coverPoints[0];

        return coverPoint;
    }

}
