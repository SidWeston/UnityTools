using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AStarPathfinding : MonoBehaviour
{

    PathRequestManager requestManager;

    private AStarGrid nodeGrid;

    private void Awake()
    {
        requestManager = GetComponent<PathRequestManager>();
        nodeGrid = GetComponent<AStarGrid>();
    }

    public void StartFindPath(Vector3 startPos, Vector3 targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos));
    }

    IEnumerator FindPath(Vector3 startPos, Vector3 endPos)
    {

        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        AStarNode startNode = nodeGrid.GetNodeFromWorldPoint(startPos);
        AStarNode targetNode = nodeGrid.GetNodeFromWorldPoint(endPos);

        if(targetNode.walkable)
        {
            Heap<AStarNode> openSet = new Heap<AStarNode>(nodeGrid.MaxHeapSize);
            HashSet<AStarNode> closedSet = new HashSet<AStarNode>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                AStarNode currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    pathSuccess = true;
                    break;
                }

                foreach (AStarNode neighbour in nodeGrid.GetNeighbours(currentNode))
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int movementCost = currentNode.gCost + GetDistance(currentNode, neighbour);
                    if (movementCost < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = movementCost;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;
                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                        else
                        {
                            openSet.UpdateItem(neighbour);
                        }
                    }

                }

            }
        }

        yield return null;
        if(pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
            pathSuccess = waypoints.Length > 0;
        }
        requestManager.FinishedProcessingPath(waypoints, pathSuccess);
    }

    private Vector3[] RetracePath(AStarNode startNode, AStarNode targetNode)
    {
        //list is used as it is impossible to know how many nodes are needed to get to the target,
        //lists can have their length changed, unlike arrays
        List<AStarNode> path = new List<AStarNode>();
        //start at the target node and work backwards
        AStarNode currentNode = targetNode;
        
        //not at start node means there are still nodes in-between the start and end nodes
        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        //convert the list of nodes into a vector3 list with the node positions
        List<Vector3> waypoints = new List<Vector3>();
        //loop through and add the node positions to the array
        for(int i = 1; i < path.Count; i++)
        {
            waypoints.Add(path[i].worldPosition);
        }
        //reverse the waypoints so the first point is closest to the unit
        waypoints.Reverse();
        //convert list to an array
        return waypoints.ToArray();

    }

    //simplify path will cut out a grid node if the direction doesn't change
    Vector3[] SimplifyPath(List<AStarNode> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 oldDirection = Vector2.zero;

        for(int i = 1; i < path.Count; i++)
        {
            Vector2 newDirection = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridZ - path[i].gridZ);
            if (newDirection != oldDirection)
            {
                waypoints.Add(path[i].worldPosition);
            }
            oldDirection = newDirection;
        }

        return waypoints.ToArray();
    }

    private int GetDistance(AStarNode nodeA, AStarNode nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridZ - nodeB.gridZ);

        if(distX > distY)
        {
            return 14 * distY + 10 * (distX - distY);
        }
        else
        {
            return 14 * distX + 10 * (distY - distX);
        }
    }

}
