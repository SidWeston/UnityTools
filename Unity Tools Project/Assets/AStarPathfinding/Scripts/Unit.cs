using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public Transform target;
    private Vector3 oldPos;
    float speed = 5;

    public Vector3[] path;
    int targetIndex;
    float rotationSpeed = 5;

    // Start is called before the first frame update
    void Start()
    {
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
        oldPos = target.position;
    }

    public void Update()
    {
        if(target.position != oldPos)
        {
            RemovePath();
            PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
            oldPos = target.position;
        }
    }

    private void RemovePath()
    {
        StopCoroutine("FollowPath");
        Array.Clear(path, 0, path.Length);
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if(pathSuccessful)
        {
            path = newPath;
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        if(path.Length < 1)
        {
            RemovePath();
            PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
            yield return null;
        }
        else if(path.Length >= 1)
        {
            Vector3 currentWaypoint = path[0];
            while (true)
            {
                if (transform.position == currentWaypoint)
                {
                    targetIndex++;
                    if (targetIndex >= path.Length)
                    {
                        targetIndex = 0;
                        path = new Vector3[0];
                        yield break;
                    }
                    currentWaypoint = path[targetIndex];
                }

                //rotate towards next waypoint
                Vector3 targetDir = currentWaypoint - this.transform.position;
                float step = this.rotationSpeed * Time.deltaTime;
                Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
                transform.rotation = Quaternion.LookRotation(newDir);

                //move towards next waypoint
                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
                yield return null;
            }
        }

    }

    public void OnDrawGizmos()
    {
        if(path != null)
        {
            for(int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one);

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
