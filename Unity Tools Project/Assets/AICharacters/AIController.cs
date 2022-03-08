using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//allows the controller to perform different actions based on the current mode of the controller
public enum ControllerMode
{
    GUARD,
    WANDER,
    PATROL,
}

public class AIController : MonoBehaviour
{

    public ControllerMode baseAIMode = ControllerMode.GUARD;

    //references to the view sensor and pathfinding unit for easy communication
    private AISensor viewSensor;
    private Unit pathfindingUnit;

    //potential target for the ai to chase, will be set upon a target entering line of sight of the ai
    private GameObject aiTarget;

    //is the ai currently chasing anything?
    private bool chasing;
    //the amount of time the ai will spend searching for the target once the target leaves line of sight
    public float chaseSearchTimer;
    private float maxSearchTimer;

    // Start is called before the first frame update
    void Start()
    {
        viewSensor = GetComponent<AISensor>();
        pathfindingUnit = GetComponent<Unit>();

        maxSearchTimer = chaseSearchTimer;

    }

    // Update is called once per frame
    void Update()
    {
        //if the ai isnt chasing the target, it will keep scanning until it finds a target to chase
        if(!chasing)
        {
            chasing = GetSightTarget();
        }
        else 
        {
            //if the ai is chasing a target and the target leaves line of sight
            if (!viewSensor.currentObjects.Contains(aiTarget))
            {

                Invoke("ResetPath", chaseSearchTimer);

                chasing = false;
                
            }
        }

    }

    private void GetTargetLastLocation()
    {
        GameObject targetLastLocation = aiTarget;
        pathfindingUnit.StartChaseObject(targetLastLocation);
        Invoke("ResetPath", chaseSearchTimer);
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
