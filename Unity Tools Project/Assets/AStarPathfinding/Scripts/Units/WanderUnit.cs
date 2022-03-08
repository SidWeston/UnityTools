using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderUnit : Unit
{
    [Header("Wander Information")]
    public float pointWaitTime; //how long will the unit wait once its reached the target point
    public float maxWanderDistance; //the distance that the unit will search for a point 
    public float minWanderDistance;

    private GameObject targetObject;

    public override void Start()
    {
        targetObject = new GameObject();
        targetObject.name = "Wander Target";
        NewWander();
    }

    public virtual IEnumerator FollowPath()
    {
        //first waypoint is 0 in the array
        Vector3 currentWaypoint = path[0];
        while (true)
        {
            //if the unit has reached the position of the current waypoint
            if (transform.position == currentWaypoint)
            {
                targetIndex++;
                //if there are no more waypoints in the array
                //break the loop and stop following the path
                if (targetIndex >= path.Length)
                {
                    //reset the target index
                    targetIndex = 0;
                    //empty the path
                    path = new Vector3[0];
                    CancelInvoke("NewWander");
                    Invoke("NewWander", pointWaitTime);
                    //break the loop
                    yield break;
                }
                //sets the next waypoint to the next index
                currentWaypoint = path[targetIndex];
            }

            //rotate towards next waypoint
            Vector3 targetDir = currentWaypoint - this.transform.position;
            float step = this.rotationSpeed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDir);

            //move towards next waypoint
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private void NewWander()
    {
        targetObject = GetRandomPointInRange();
        target = targetObject.transform;
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }

    private GameObject GetRandomPointInRange()
    {
        Vector3 randomVector = Random.insideUnitSphere * Random.Range(minWanderDistance, maxWanderDistance);
        randomVector.y = 0;
        GameObject pointTransform = targetObject;
        pointTransform.transform.position = randomVector;
        return pointTransform;
    }



    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();


    }
}
