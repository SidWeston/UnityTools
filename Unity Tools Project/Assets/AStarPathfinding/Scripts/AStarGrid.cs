using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AStarGrid : MonoBehaviour
{
    [Tooltip("Should the navigation grid be displayed in the game view?")]
    public bool displayGridGizmos;

    [Header("Grid Info")]
    [Tooltip("What layer(s) the path-finding grid will mark as un-walkable.")]
    public LayerMask unwalkableMask;
    public LayerMask whatIsGround;
    public Vector3 gridWorldSize;
    public float nodeRadius;
    AStarNode[,] nodeGrid;

    private float nodeDiameter;
    private int gridSizeX, gridSizeZ;

    public List<AStarNode> path;

    private void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeZ = Mathf.RoundToInt(gridWorldSize.z / nodeDiameter);
        CreateGrid();
    }

    public int MaxHeapSize
    {
        get
        {
            return gridSizeX * gridSizeZ;
        }
    }

    public void CreateGrid()
    {
        //setup the array of nodes with specified size
        nodeGrid = new AStarNode[gridSizeX, gridSizeZ];
        //get the position of the bottom left corner of the node grid
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.z / 2;

        //loop through both x and z axis to create grid of nodes
        for(int x = 0; x < gridSizeX; x++)
        {
            for(int z = 0; z < gridSizeZ; z++)
            {
                //get the vector3 coords of the current node based on the bottom left of the grid
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (z * nodeDiameter + nodeRadius);
                //check if there is terrain in the x and z coordinates of the grid
                worldPoint.y = CheckNodeHeight(worldPoint);
                //check if the current node overlaps with any unwalkable obstacle
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                //construct a node with these values
                nodeGrid[x, z] = new AStarNode(walkable, worldPoint, x, z);
            }
        }
    }

    private float CheckNodeHeight(Vector3 position)
    {
        //store original y value temporarily
        float tempStore = position.y;
        //set y value to be at the top of the bounding box
        position.y = gridWorldSize.y;
        //perform a raycast to test if there is ground beneath the current node
        RaycastHit hit;
        if(Physics.Raycast(position, Vector3.down, out hit, gridWorldSize.y, whatIsGround))
        {
            //if there is ground hit return the y value of the hit position
            return hit.point.y;
        }
        //if nothing is hit return original value
        return tempStore;
    }

    public List<AStarNode> GetNeighbours(AStarNode currentNode)
    {
        //list to contain all the neighbouring nodes of the current node
        List<AStarNode> neighbours = new List<AStarNode>();
        for(int x = -1; x <= 1; x++)
        {
            for(int z = -1; z <= 1; z++)
            {
                //if both equal 0 the node being checked is the current node
                if(x == 0 && z == 0)
                {
                    continue;
                }
                int checkX = currentNode.gridX + x;
                int checkZ = currentNode.gridZ + z;

                if(checkX >= 0 && checkX < gridSizeX && checkZ >= 0 && checkZ < gridSizeZ)
                {
                    neighbours.Add(nodeGrid[checkX, checkZ]);
                }
            }
        }
        return neighbours;
    }

    public AStarNode GetNodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentZ = (worldPosition.z + gridWorldSize.z / 2) / gridWorldSize.z;
        percentX = Mathf.Clamp01(percentX);
        percentZ = Mathf.Clamp01(percentZ);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int z = Mathf.RoundToInt((gridSizeZ - 1) * percentZ);
        return nodeGrid[x, z];
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, gridWorldSize.z));
        if (nodeGrid != null && displayGridGizmos)
        {
            foreach (AStarNode node in nodeGrid)
            {
                Gizmos.color = (node.walkable) ? Color.white : Color.red;
                Gizmos.DrawCube(new Vector3(node.worldPosition.x, node.worldPosition.y - 0.4f, node.worldPosition.z), new Vector3(nodeDiameter - 0.1f, 0.1f, nodeDiameter - 0.1f));
            }
        }
    }
}
