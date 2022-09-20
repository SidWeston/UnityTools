using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{

    //references to the view sensor and pathfinding unit for easy communication
    [HideInInspector] public AISensor viewSensor;
    [HideInInspector] public Unit pathfindingUnit;
    [HideInInspector] public AIWorldInfo worldInfo;
    [HideInInspector] public AIHealth unitHealth;
    [HideInInspector] public AIWeapon unitWeapon;
    [HideInInspector] public CoverSystem coverSystem;

    public PatrolPath patrolPath;
    private int patrolPathIndex = 0;
    public float pointWaitTimer = 1.0f;

    public bool hasWeapon = true;

    //potential target for the ai to chase, will be set upon a target entering line of sight of the ai
    private GameObject aiTarget;
    private GameObject tempTarget;
    private float distanceToTarget;
    private bool hasCoverPoint = false;

    [Tooltip("How quickly should the ai evaluate a choice? Lower value = faster evaluation")]
    public float evaluationTimer = 0.1f;
    private float maxEvaluationTimer;

    public MeshRenderer aiMesh;
    [Tooltip("What colour the mesh will be depending on the state")]
    public List<Material> stateMaterials;

    private void Awake()
    {
        worldInfo = GameObject.FindGameObjectWithTag("WorldInfo").GetComponent<AIWorldInfo>();
        unitHealth = GetComponent<AIHealth>();
        viewSensor = GetComponent<AISensor>();
        unitWeapon = GetComponent<AIWeapon>();
        coverSystem = GetComponent<CoverSystem>();
        pathfindingUnit = GetComponent<Unit>();
        
        //set the pathfinding target to be an empty game object
        tempTarget = new GameObject();
        tempTarget.name = "Temp AI Target";
        pathfindingUnit.target = tempTarget.transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        maxEvaluationTimer = evaluationTimer;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetTarget(Vector3 newTarget)
    {
        pathfindingUnit.SetTargetVector(newTarget);
        pathfindingUnit.RequestNewPath();
    }
    public bool GetSightTarget()
    {
        //if there is an object in the view sensor
        if(viewSensor.currentObjects.Count > 0)
        { 
            //find the parent object and set that as the target object
            if(viewSensor.currentObjects[0].transform.parent == null)
            {
                aiTarget = viewSensor.currentObjects[0];
            }
            else
            {
                aiTarget = viewSensor.currentObjects[0].transform.parent.gameObject;
            }
            return true;
        }
        return false;
    }

    private void DamageTaken()
    {
        aiMesh.material = stateMaterials[2];
    }

}
