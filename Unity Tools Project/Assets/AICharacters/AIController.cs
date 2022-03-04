using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ControllerMode
{
    GUARD,
    WANDER,
    PATROL,
}

public class AIController : MonoBehaviour
{

    public ControllerMode baseAIMode = ControllerMode.GUARD;

    private AISensor viewSensor;
    private Unit pathfindingUnit;

    private GameObject aiTarget;

    private bool chasing;

    // Start is called before the first frame update
    void Start()
    {
        viewSensor = GetComponent<AISensor>();
        pathfindingUnit = GetComponent<Unit>();

    }

    // Update is called once per frame
    void Update()
    {
        if(!chasing)
        {
            chasing = GetSightTarget();
        }
        else
        {
            if (!viewSensor.currentObjects.Contains(aiTarget))
            {
                chasing = false;
                switch (baseAIMode)
                {
                    case ControllerMode.PATROL:
                        {
                            pathfindingUnit.ResetFollowPath();
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
        }

    }


    private bool GetSightTarget()
    {
        if(viewSensor.currentObjects.Count > 0)
        { 
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
