using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarNode : IHeapItem<AStarNode>
{
    public bool walkable;
    public Vector3 worldPosition;
    public int gridX, gridY;

    public int gCost, hCost;
    public AStarNode parent;
    int heapIndex;

    //Constructor
    public AStarNode(bool a_walkable, Vector3 a_worldPosition, int a_gridX, int a_gridY)
    {
        walkable = a_walkable;
        worldPosition = a_worldPosition;
        worldPosition.y += 0.5f; //set slightly higher on y axis so point isn't inside the ground
        gridX = a_gridX;
        gridY = a_gridY;
    }

    //fCost is gCost + hCost
    //fCost is used to help determine the shortest path
    public int fCost 
    {
        get
        {
            return gCost + hCost;
        }
    }

    //implement heap index to node 
    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }

        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(AStarNode nodeToCompare)
    {
        //int.CompareTo returns 0 if the int is equal to what it is being compared to
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        //if the fCost of both nodes are equal
        if(compare == 0) 
        {
            //compare the hCost to find the cheapest node
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }

}
