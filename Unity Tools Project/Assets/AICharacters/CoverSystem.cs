using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverSystem : MonoBehaviour
{

    private Collider[] colliders = new Collider[10];
    private GameObject coverPoint;
    private AStarGrid pathfindingRef;


    public LayerMask coverMask;

    // Start is called before the first frame update
    void Start()
    {
        coverPoint = new GameObject();
        coverPoint.name = "Cover Point";

        pathfindingRef = GameObject.FindGameObjectWithTag("AStar").GetComponent<AStarGrid>();
        

    }

    public GameObject GetNearestCoverPoint()
    {
        int hits = Physics.OverlapSphereNonAlloc(transform.position, GetComponent<AISensor>().sensorDistance, colliders, coverMask);

        for(int i = 0; i < hits; i++)
        {

            Vector3 pointToTest = colliders[i].ClosestPointOnBounds(this.transform.position);

            if (pathfindingRef.IsNodeWalkable(pointToTest))
            {
                coverPoint.transform.position = pointToTest;    
                if(LostLOS())
                {
                    return coverPoint;
                }
                else
                {
                    Debug.Log("here");
                    //calculate point to the other side 
                    pointToTest.x += colliders[i].transform.localScale.x;
                    coverPoint.transform.position = pathfindingRef.GetNearestWalkableNode(pointToTest);
                    return coverPoint;
                }
            }
            else
            {
                coverPoint.transform.position = pathfindingRef.GetNearestWalkableNode(pointToTest);
                if(LostLOS())
                {
                    return coverPoint;
                }
                else
                {
                    Debug.Log("here");
                    //calculate point to the other side
                    pointToTest.x += colliders[i].transform.localScale.x;
                    coverPoint.transform.position = pathfindingRef.GetNearestWalkableNode(pointToTest);
                    return coverPoint;
                }
            }
        }
        return null;
    }

    private bool LostLOS()
    {

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if(!player)
        {
            return false; //early out
        }

        Vector3 directionToPlayer = (transform.position - player.transform.position).normalized;

        RaycastHit hit;
        if(Physics.Raycast(player.transform.position, directionToPlayer, out hit, Mathf.Infinity))
        {
            if(hit.collider.gameObject == this.gameObject)
            {
                return false;
            }
        }
        return true;
    }

}
