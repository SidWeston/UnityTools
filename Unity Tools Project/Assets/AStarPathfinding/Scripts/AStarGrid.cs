using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private int gridSizeX, gridSizeY, gridSizeZ;

    public List<AStarNode> path;


    private void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeZ = Mathf.RoundToInt(gridWorldSize.z / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    public int MaxHeapSize
    {
        get
        {
            return gridSizeX * gridSizeZ;
        }
    }


    private void CreateGrid()
    {
        nodeGrid = new AStarNode[gridSizeX, gridSizeZ];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.z / 2;

        for(int x = 0; x < gridSizeX; x++)
        {
            for(int z = 0; z < gridSizeZ; z++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (z * nodeDiameter + nodeRadius);
                worldPoint.y = CheckNodeHeight(worldPoint);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                nodeGrid[x, z] = new AStarNode(walkable, worldPoint, x, z);
            }
        }
    }

    private float CheckNodeHeight(Vector3 position)
    {
        RaycastHit hit;
        if(Physics.Raycast(position, Vector3.up, out hit, gridWorldSize.y/2, whatIsGround))
        {
            Debug.Log("Checking Up");
            return hit.point.y;
        }
        else if(Physics.Raycast(position, Vector3.down, out hit, gridWorldSize.y/2, whatIsGround))
        {
            Debug.Log("Checking Down");
            return hit.point.y;
        }
        return position.y;
    }

    private float CheckNodeRotation()
    {
        return 0;
    }

    public List<AStarNode> GetNeighbours(AStarNode currentNode)
    {
        List<AStarNode> neighbours = new List<AStarNode>();
        for(int x = -1; x <= 1; x++)
        {
            for(int z = -1; z <= 1; z++)
            {
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
        if(nodeGrid != null && displayGridGizmos)
        {
            foreach (AStarNode node in nodeGrid)
            {
                Gizmos.color = (node.walkable) ? Color.white : Color.red;
                Gizmos.DrawCube(node.worldPosition, new Vector3(nodeDiameter - 0.1f, 0.1f, nodeDiameter - 0.1f));
            }
        }
    }
}
