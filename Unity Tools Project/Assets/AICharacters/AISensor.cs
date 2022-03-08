using System.Collections.Generic;
using UnityEngine;


public class AISensor : MonoBehaviour
{
    //variables for the size and colour of the sensor
    public float sensorDistance = 10;
    public float sensorAngle = 30;
    public float sensorHeight = 1;
    public Color meshColour = Color.red;

    //offsets the height of where the field of view mesh starts
    public float sensorHeightOffset = 0;

    public int scanFrequency = 30;
    private float scanInterval;
    private float scanTimer;
    public LayerMask layers;
    public LayerMask occlusionLayers;
    public List<GameObject> currentObjects = new List<GameObject>();
    public Collider[] colliders = new Collider[50];
    private int colliderCount;


    Mesh wedgeMesh;
    // Start is called before the first frame update
    void Start()
    {
        scanInterval = 1.0f / scanFrequency;
    }

    // Update is called once per frame
    void Update()
    {

        scanTimer -= Time.deltaTime;
        if (scanTimer <= 0)
        {
            scanTimer += scanInterval;
            ScanForObjects();
        }
    }

    private void OnValidate()
    {
        wedgeMesh = CreateWedgeMesh();
        scanInterval = 1.0f / scanFrequency;
        meshColour.a = 0.5f;
    }

    private Mesh CreateWedgeMesh()
    {
        Mesh mesh = new Mesh();

        int segments = 10;
        int numTriangles = (segments * 4) + 2 + 2;
        int numVertices = numTriangles * 3;

        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numVertices];

        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0, -sensorAngle, 0) * Vector3.forward * sensorDistance;
        Vector3 bottomRight = Quaternion.Euler(0, sensorAngle, 0) * Vector3.forward * sensorDistance;

        Vector3 topCenter = bottomCenter + Vector3.up * sensorHeight;
        Vector3 topLeft = bottomLeft + Vector3.up * sensorHeight;
        Vector3 topRight = bottomRight + Vector3.up * sensorHeight;

        bottomCenter.y += sensorHeightOffset;
        bottomLeft.y += sensorHeightOffset;
        bottomRight.y += sensorHeightOffset;
        topCenter.y += sensorHeightOffset;
        topLeft.y += sensorHeightOffset;
        topRight.y += sensorHeightOffset;

        int vert = 0;

        //left side made up of two triangles to make a rectangle
        vertices[vert++] = bottomCenter;
        vertices[vert++] = bottomLeft;
        vertices[vert++] = topLeft;

        vertices[vert++] = topLeft;
        vertices[vert++] = topCenter;
        vertices[vert++] = bottomCenter;

        //right side made up of two triangles to make a rectangle
        vertices[vert++] = bottomCenter;
        vertices[vert++] = topCenter;
        vertices[vert++] = topRight;

        vertices[vert++] = topRight;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomCenter;

        float currentAngle = -sensorAngle;
        float deltaAngle = (sensorAngle * 2) / segments;

        for (int i = 0; i < segments; ++i)
        {
            bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * sensorDistance;
            bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * sensorDistance;

            topLeft = bottomLeft + Vector3.up * sensorHeight;
            topRight = bottomRight + Vector3.up * sensorHeight;

            bottomLeft.y += sensorHeightOffset;
            bottomRight.y += sensorHeightOffset;
            topLeft.y += sensorHeightOffset;
            topRight.y += sensorHeightOffset;

            //far side

            vertices[vert++] = bottomLeft;
            vertices[vert++] = bottomRight;
            vertices[vert++] = topRight;

            vertices[vert++] = topRight;
            vertices[vert++] = topLeft;
            vertices[vert++] = bottomLeft;

            //top

            vertices[vert++] = topCenter;
            vertices[vert++] = topLeft;
            vertices[vert++] = topRight;

            //bottom

            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomLeft;

            currentAngle += deltaAngle;
        }

        for (int i = 0; i < numVertices; ++i)
        {
            triangles[i] = i;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }

    private void ScanForObjects()
    {
        //keeps track of all objects within a certain radius of the sensor
        colliderCount = Physics.OverlapSphereNonAlloc(transform.position, sensorDistance, colliders, layers, QueryTriggerInteraction.Collide);

        //clears the current objects in the field of view
        currentObjects.Clear();
        //loops through the objects currently in range of the sensor 
        for (int i = 0; i < colliderCount; ++i)
        {
            GameObject obj = colliders[i].gameObject;
            //tests whether the object is within the line of sight cone 

            //perform line of sight test
            if (ObjectInSight(obj))
            {
                //if successful, add it to the array of the current objects in line of sight
                currentObjects.Add(obj);
            }
        }
    }

    public bool ObjectInSight(GameObject obj)
    {

        Vector3 origin = transform.position;
        origin.y += sensorHeightOffset;
        Vector3 dest = obj.transform.position;
        Vector3 direction = dest - origin;
        //check if the obj is neither higher than the sensor or lower
        if (direction.y < 0 || direction.y > sensorHeight)
        {
            Debug.Log("Object is too high or LOS cone is too low");
            return false;
        }
        //set the y axis to 0 to ensure that only the x and z axis are taken into account
        direction.y = 0;
        //get the angle of the target object relevant to the forward vector of the ai unit
        float deltaAngle = Vector3.Angle(direction, transform.forward);
        //if the delta angle is greater than the angle of the sensor, the target object is not in line of sight
        if (deltaAngle > sensorAngle)
        {
            return false;
        }
        //set the origin to be in the center of the sensor cone 
        origin.y += sensorHeight / 2;
        dest.y = origin.y;
        //perform a raycast to test if there is any geometry in the way
        if (Physics.Linecast(origin, dest, occlusionLayers))
        {
            //if the raycast hits anything, it means there is no line of sight
            return false;
        }
        //otherwise, the ai should have clear line of sight to the target
        return true;
    }

    private void OnDrawGizmos()
    {
        //check wedge mesh has been created successfully
        if (wedgeMesh)
        {
            Gizmos.color = meshColour;
            Gizmos.DrawMesh(wedgeMesh, transform.position, transform.rotation);
        }

        Gizmos.DrawWireSphere(transform.position, sensorDistance);
        for (int i = 0; i < colliderCount; ++i)
        {
            Gizmos.DrawSphere(colliders[i].transform.position, 0.2f);
        }

        Gizmos.color = Color.green;
        foreach (var obj in currentObjects)
        {
            Gizmos.DrawSphere(obj.transform.position, 0.2f);
        }
    }
}
