using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Unit : MonoBehaviour
{

    [Header("Unit Movement")]
    [Tooltip("The GameObject that the AI unit will generate paths to and move towards.")]
    public Transform target;
    //what the unit will look towards
    public Vector3 lookTarget; 
    //the delay between the unit requesting new a new updated path from the path request manager
    public bool doesUpdatePath = false;
    [Range(0, 2)]
    [Tooltip("The delay between the unit requesting a new updated path from the path request manager. Accounts for moving targets.")]
    public float newPathRequestDelay;
    [Tooltip("How fast the unit will move towards it's target.")]
    public float moveSpeed = 5;

    [HideInInspector]
    public Vector3[] path;
    protected int targetIndex;
    [HideInInspector]
    public bool shouldRotateNextPoint = true;
    protected float rotationSpeed = 5;

    [HideInInspector]
    public bool isMoving = false;

    // Start is called before the first frame update
    public virtual void Start()
    {
        if(doesUpdatePath)
        {
            //gets initial path and then gets a new one after a set delay and repeats until game stops
            InvokeRepeating("RequestNewPath", 0, newPathRequestDelay);
        }
        else
        {
            RequestNewPath();
        }

    }

    private void Update()
    {
        LookTowards(lookTarget);
    }

    public void UpdatePath(bool shouldUpdate)
    {
        if(shouldUpdate)
        {
            CancelInvoke("RequestNewPath");
            InvokeRepeating("RequestNewPath", 0, newPathRequestDelay);
        }
        else
        {
            CancelInvoke("RequestNewPath");
        }
        
    }

    public virtual void RequestNewPath()
    {
        //request a new path from the path request manager
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }

    public virtual void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if(!GameObject.FindGameObjectWithTag("AStar").GetComponent<AStarGrid>().IsNodeWalkable(target.transform.position))
        {
            GameObject.FindGameObjectWithTag("AStar").GetComponent<AStarGrid>().GetNearestWalkableNode(target.transform.position);
        }

        if(pathSuccessful)
        {
            //pass through the path to follow
            path = newPath;
            isMoving = true;
            //stops the coroutine to get rid of an old path if there was one
            StopCoroutine("FollowPath");
            //start the coroutine to follow the path
            StartCoroutine("FollowPath");
        }
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
                    //unit is no longer moving
                    isMoving = false;
                    //break the loop
                    yield break;
                }
                //sets the next waypoint to the next index
                currentWaypoint = path[targetIndex];
            }

            if (shouldRotateNextPoint)
            {
                lookTarget = currentWaypoint;
            }

            //move towards next waypoint
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public void LookTowards(Vector3 target)
    {
        Vector3 targetDir = target - this.transform.position;
        float step = this.rotationSpeed;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
    }

    public virtual void StartChaseObject(GameObject newTarget)
    {
        target = newTarget.transform;
        InvokeRepeating("RequestNewPath", 0, newPathRequestDelay);
    }

    public virtual void ResetFollowPath()
    {
        CancelInvoke("RequestNewPath");
    }

    public virtual void OnDrawGizmos()
    {
        //draw a line along the path the ai unit will follow
        if(path != null)
        {
            for(int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one/2);

                if(i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }

}
