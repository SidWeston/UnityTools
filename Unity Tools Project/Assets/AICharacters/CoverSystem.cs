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

    public GameObject GetNearestCoverPoint(Vector3 scanOrigin)
    {
        int hits = Physics.OverlapSphereNonAlloc(scanOrigin, GetComponent<AISensor>().sensorDistance, colliders, coverMask);

        for(int i = 0; i < hits; i++)
        {

            Vector3 pointToTest = colliders[i].ClosestPointOnBounds(this.transform.position);

            if (pathfindingRef.IsNodeWalkable(pointToTest))
            {
                coverPoint.transform.position = pointToTest;    
                if(!CoverPointHasSight(coverPoint.transform.position))
                {
                    return coverPoint;
                }
                else
                {
                    //calculate point to the other side 
                    pointToTest = colliders[i].ClosestPointOnBounds(pointToTest + (colliders[i].transform.position + colliders[i].transform.localScale));
                    coverPoint.transform.position = pathfindingRef.GetNearestWalkableNode(pointToTest);
                    return coverPoint;
                }
            }
            else
            {
                coverPoint.transform.position = pathfindingRef.GetNearestWalkableNode(pointToTest);
                if(!CoverPointHasSight(coverPoint.transform.position))
                {
                    return coverPoint;
                }
                else
                {
                    //calculate point to the other side
                    pointToTest = colliders[i].ClosestPointOnBounds(pointToTest + (colliders[i].transform.position + colliders[i].transform.localScale));
                    coverPoint.transform.position = pathfindingRef.GetNearestWalkableNode(pointToTest);
                    return coverPoint;
                }
            }
        }
        return null;
    }

    private bool CoverPointHasSight(Vector3 coverPointPos)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (!player)
        {
            return false; //early out
        }
        Debug.DrawLine(coverPointPos, player.transform.position, Color.red, 10.0f);

        if (Physics.Linecast(coverPointPos, player.transform.position, coverMask))
        {
            Debug.Log("Hit Something");
            return false;
        }

        return true;
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

    private void OnDrawGizmos()
    {
        
    }

}
