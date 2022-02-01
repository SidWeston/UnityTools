using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TerrainType
{
    public string name;
    public float height;
    public Color colour;
}

public class TileGeneration : MonoBehaviour
{
    [SerializeField]
    private TerrainType[] terrainTypes;
    [SerializeField]
    NoiseMapGeneration noiseMapGeneration;
    [SerializeField]
    private MeshRenderer tileRenderer;
    [SerializeField]
    private MeshFilter meshFilter;
    [SerializeField]
    private MeshCollider meshCollider;
    [SerializeField]
    private float mapScale;
    [SerializeField]
    private float heightMultiplier;
    [SerializeField]
    private AnimationCurve heightCurve;
    [SerializeField]
    private Wave[] waves;
    private void Start()
    {
        GenerateTile();
    }

    private void GenerateTile()
    {
        Vector3[] meshVertices = meshFilter.mesh.vertices;
        //calculate tile depth and width based on the mesh vertices
        int tileDepth = (int)Mathf.Sqrt(meshVertices.Length);
        int tileWidth = tileDepth;

        float offsetX = -gameObject.transform.position.x;
        float offsetZ = -gameObject.transform.position.z;

        float[,] heightMap = noiseMapGeneration.GenerateNoiseMap(tileDepth, tileWidth, mapScale, offsetX, offsetZ, waves);

        Texture2D tileTexture = BuildTexture(heightMap);
        tileRenderer.material.mainTexture = tileTexture;
        UpdateMeshVertices(heightMap);
    }

    private Texture2D BuildTexture(float[,] heightMap)
    {
        int tileDepth = heightMap.GetLength(0);
        int tileWidth = heightMap.GetLength(1);

        Color[] colourMap = new Color[tileDepth * tileWidth];
        for (int zIndex = 0; zIndex < tileDepth; zIndex++)
        {
            for (int xIndex = 0; xIndex < tileWidth; xIndex++)
            {
                int colourIndex = zIndex * tileWidth + xIndex;
                float height = heightMap[zIndex, xIndex];

                TerrainType terrainType = ChooseTerrainType(height);

                colourMap[colourIndex] = terrainType.colour;
            }
        }

        Texture2D tileTexture = new Texture2D(tileWidth, tileDepth);
        tileTexture.wrapMode = TextureWrapMode.Clamp;
        tileTexture.SetPixels(colourMap);
        tileTexture.Apply();

        return tileTexture;
    }

    TerrainType ChooseTerrainType(float height)
    {
        foreach (TerrainType terrainType in terrainTypes)
        {
            if (height < terrainType.height)
            {
                return terrainType;
            }
        }
        return terrainTypes[terrainTypes.Length - 1];
    }

    private void UpdateMeshVertices(float[,] heightMap)
    {
        int tileDepth = heightMap.GetLength(0);
        int tileWidth = heightMap.GetLength(1);

        Vector3[] meshVertices = meshFilter.mesh.vertices;

        int vertexIndex = 0;
        for (int zIndex = 0; zIndex < tileDepth; zIndex++)
        {
            for (int xIndex = 0; xIndex < tileWidth; xIndex++)
            {
                float height = heightMap[zIndex, xIndex];
                Vector3 vertex = meshVertices[vertexIndex];
                meshVertices[vertexIndex] = new Vector3(vertex.x, heightCurve.Evaluate(height) * heightMultiplier, vertex.z);
                vertexIndex++;
            }
        }

        meshFilter.mesh.vertices = meshVertices;
        meshFilter.mesh.RecalculateBounds();
        meshFilter.mesh.RecalculateNormals();
        meshCollider.sharedMesh = meshFilter.mesh;

    }
}
