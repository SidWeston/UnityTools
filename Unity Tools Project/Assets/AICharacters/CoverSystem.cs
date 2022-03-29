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
            if (pathfindingRef.IsNodeWalkable(colliders[i].transform.position))
            {
                coverPoint.transform.position = colliders[i].transform.position;
                return coverPoint;
            }
        }

        return null;
    }

}
