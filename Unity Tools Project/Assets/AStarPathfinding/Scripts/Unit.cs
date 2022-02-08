using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public Transform target;
    //the delay between the unit requesting new a new updated path from the path request manager
    [Range(0, 5)]
    public float newPathRequestDelay;
    public float moveSpeed = 5;

    public Vector3[] path;
    int targetIndex;
    float rotationSpeed = 5;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("RequestNewPath", 0, newPathRequestDelay);
    }

    public void RequestNewPath()
    {
        //request a new path from the path request manager
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if(pathSuccessful)
        {
            //pass through the path to follow
            path = newPath;
            //stops the coroutine to get rid of an old path if there was one
            StopCoroutine("FollowPath");
            //start the coroutine to follow the path
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
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

    public void OnDrawGizmos()
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
