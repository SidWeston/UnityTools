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
    COVER,
    COMBAT
}

public class AIController : MonoBehaviour
{
    public AIState currentState = AIState.PATROL;

    //references to the view sensor and pathfinding unit for easy communication
    [HideInInspector] public AISensor viewSensor;
    [HideInInspector] public Unit pathfindingUnit;
    [HideInInspector] public AIWorldInfo worldInfo;
    [HideInInspector] public AIHealth unitHealth;
    [HideInInspector] public AIWeapon unitWeapon;
    [HideInInspector] public CoverSystem coverSystem;

    public PatrolPath patrolPath;
    private int patrolPathIndex = 0;
    public float pointWaitTimer = 1.0f;

    public bool hasWeapon = true;

    //potential target for the ai to chase, will be set upon a target entering line of sight of the ai
    private GameObject aiTarget;
    private GameObject tempTarget;
    private float distanceToTarget;
    private bool hasCoverPoint = false;

    [Tooltip("How quickly should the ai evaluate a choice? Lower value = faster evaluation")]
    public float evaluationTimer = 0.1f;
    private float maxEvaluationTimer;

    public MeshRenderer aiMesh;
    [Tooltip("What colour the mesh will be depending on the state")]
    public List<Material> stateMaterials;

    private void Awake()
    {
        worldInfo = GameObject.FindGameObjectWithTag("WorldInfo").GetComponent<AIWorldInfo>();
        unitHealth = GetComponent<AIHealth>();
        viewSensor = GetComponent<AISensor>();
        unitWeapon = GetComponent<AIWeapon>();
        coverSystem = GetComponent<CoverSystem>();
        pathfindingUnit = GetComponent<Unit>();
        
        //set the pathfinding target to be an empty game object
        tempTarget = new GameObject();
        tempTarget.name = "Temp AI Target";
        pathfindingUnit.target = tempTarget.transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        maxEvaluationTimer = evaluationTimer;
    }

    // Update is called once per frame
    void Update()
    {
        //if(evaluationTimer > 0)
        //{
        //    evaluationTimer -= Time.deltaTime;
        //}
        //else if(evaluationTimer <= 0)
        //{
        //    EvaluateAIChoice();
        //    evaluationTimer = maxEvaluationTimer;
        //}
    }

    public void SetTarget(Vector3 newTarget)
    {
        pathfindingUnit.SetTargetVector(newTarget);
        pathfindingUnit.RequestNewPath();
    }

    private void EvaluateAIChoice()
    {
        //if there is a target, get the distance to the target
        if (aiTarget)
        {
            distanceToTarget = Vector3.Distance(transform.position, aiTarget.transform.position);
        }

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
                            if(hasWeapon)
                            {
                                currentState = AIState.COMBAT;
                                aiMesh.material = stateMaterials[1];
                            }
                            else
                            {
                                currentState = AIState.CHASE;
                                aiMesh.material = stateMaterials[1];
                            }
                        }
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
                        if(hasWeapon)
                        {
                            currentState = AIState.COMBAT;
                            aiMesh.material = stateMaterials[1];
                        }
                        else
                        {
                            currentState = AIState.CHASE;
                            aiMesh.material = stateMaterials[1];
                        }
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
                        aiMesh.material = stateMaterials[0];
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
                    if(!hasCoverPoint)
                    {
                        pathfindingUnit.target = coverSystem.GetNearestCoverPoint(this.transform.position).transform;
                        pathfindingUnit.RequestNewPath();
                        if(pathfindingUnit.target != null)
                        {
                            hasCoverPoint = true;
                        }
                    }

                    if(!pathfindingUnit.isMoving && Vector3.Distance(transform.position, pathfindingUnit.target.position) >= 0.1f)
                    {
                        pathfindingUnit.RequestNewPath();
                    }

                    //if(!pathfindingUnit.isMoving && !IsInvoking() && Vector3.Distance(transform.position, pathfindingUnit.target.position) <= 1.5f)
                    //{
                    //    Invoke("GetNextPatrolPoint", pointWaitTimer * 2);
                    //}

                    break;
                }
            case AIState.COMBAT:
                {
                    if(GetSightTarget())
                    {
                        unitWeapon.StartCoroutine("FireWeapon");
                        pathfindingUnit.lookTarget = aiTarget.transform.position;
                        pathfindingUnit.shouldRotateNextPoint = false;
                    }
                    else
                    {
                        unitWeapon.StopCoroutine("FireWeapon");
                    }

                    if(distanceToTarget > viewSensor.sensorDistance - (viewSensor.sensorDistance / 10))
                    {
                        //if the target gets too far away, move towards it
                        tempTarget.transform.position = Vector3.Lerp(transform.position, aiTarget.transform.position, 0.5f);
                        pathfindingUnit.target = tempTarget.transform;
                        pathfindingUnit.RequestNewPath();
                    }
                    else if(distanceToTarget < viewSensor.sensorDistance / 2)
                    {
                        //move away if the target gets too close
                        tempTarget.transform.position = -(transform.forward * (viewSensor.sensorDistance / 2));
                        pathfindingUnit.target = tempTarget.transform;
                        pathfindingUnit.RequestNewPath();
                    }

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

    public bool GetSightTarget()
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
            return true;
        }
        return false;
    }

    private void DamageTaken()
    {
        currentState = AIState.COVER;
        aiMesh.material = stateMaterials[2];
    }

}
