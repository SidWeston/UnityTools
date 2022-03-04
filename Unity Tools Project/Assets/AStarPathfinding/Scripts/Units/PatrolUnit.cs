using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolUnit : Unit
{
    [Space(20)]
    [Header("Patrol Information")]
    public GameObject[] patrolPoints;
    public float patrolWaitTime;

    private int currentPatrolPointIndex = 0;
    private bool followPath = true;

    // Start is called before the first frame update
    public override void Start()
    {
        //if there are set patrol points in the array
        if(patrolPoints.Length > 0)
        {
            //will start at the beginning of the array of point and generate a path to the first one
            target = patrolPoints[0].transform;
            PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
        }
    }

    public override void RequestNewPath()
    {
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }

    private void GetNextPoint()
    {
        //increment to the next patrol point in the array
        currentPatrolPointIndex++;
        target = patrolPoints[currentPatrolPointIndex].transform;
        //request a new path to the next point
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }

    private void ResetPatrolLoop()
    {
        //reset to the first patrol point in the array
        currentPatrolPointIndex = 0;
        target = patrolPoints[currentPatrolPointIndex].transform;
        //get a path back to the first patrol point in the array
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }

    public override IEnumerator FollowPath()
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
                    //if the unit is following a set path 
                    if(followPath)
                    {
                        //if that wasnt the last patrol point
                        if (currentPatrolPointIndex < patrolPoints.Length - 1)
                        {
                            //invoke calls a function after a set delay
                            Invoke("GetNextPoint", patrolWaitTime);
                        }
                        else if (currentPatrolPointIndex >= patrolPoints.Length - 1)
                        {
                            Invoke("ResetPatrolLoop", patrolWaitTime);
                        }
                    }
                    //break the loop
                    yield break;
                }
                //sets the next waypoint to the next index
                currentWaypoint = path[targetIndex];
            }

            //rotate towards next waypoint
            if(shouldRotateNextPoint)
            {
                Vector3 targetDir = currentWaypoint - this.transform.position;
                float step = this.rotationSpeed * Time.deltaTime;
                Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
                transform.rotation = Quaternion.LookRotation(newDir);
            }
            else
            {
                Vector3 targetDir = target.transform.position - this.transform.position;
                float step = this.rotationSpeed * Time.deltaTime;
                Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
                transform.rotation = Quaternion.LookRotation(newDir);
            }

            //move towards next waypoint
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public override void StartChaseObject(GameObject newTarget)
    {
        followPath = false;
        target = newTarget.transform;
        shouldRotateNextPoint = false;
        InvokeRepeating("RequestNewPath", 0, newPathRequestDelay);
    }

    public override void ResetFollowPath()
    {
        followPath = true;
        target = patrolPoints[currentPatrolPointIndex].transform;
        shouldRotateNextPoint = true;
        CancelInvoke("RequestNewPath");
    }

    private void OnTriggerEnter(Collider other)
    {
        //if player overlaps with collider they are in the units line of sight
        if (other.gameObject.tag == "Player")
        {
            //wont be following set path anymore
            followPath = false;
            //set player to be the target
            target = other.gameObject.transform;
            //request a path to the player and get a new path repeating on a set delay to account for the target moving
            InvokeRepeating("RequestNewPath", 0, newPathRequestDelay);
        }
    }

}
