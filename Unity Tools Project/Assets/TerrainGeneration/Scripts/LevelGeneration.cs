using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelGeneration : MonoBehaviour
{
    [SerializeField]
    private int mapWidthInTiles, mapDepthInTiles;
    [SerializeField]
    private GameObject tilePrefab;
    [SerializeField]
    private List<GameObject> worldTiles;


    private float buttonPress;
    private bool destroyedWorld = false;
    private void Start()
    {

        GenerateMap();
    }

    private void Update()
    {
        if (buttonPress > 0 && !destroyedWorld)
        {
            for (int i = 0; i < worldTiles.Count; i++)
            {
                Destroy(worldTiles[i].gameObject);
                worldTiles.RemoveAt(i);
            }
            destroyedWorld = true;
        }
        else if (buttonPress == 0 && destroyedWorld)
        {
            destroyedWorld = false;
            GenerateMap();
        }
    }

    private void GenerateMap()
    {
        Vector3 tileSize = tilePrefab.GetComponent<MeshRenderer>().bounds.size;
        int tileWidth = (int)tileSize.x;
        int tileDepth = (int)tileSize.z;

        for (int xTileIndex = 0; xTileIndex < mapWidthInTiles; xTileIndex++)
        {
            for (int zTileIndex = 0; zTileIndex < mapDepthInTiles; zTileIndex++)
            {
                Vector3 tilePosition = new Vector3(gameObject.transform.position.x + xTileIndex * tileWidth,
                    gameObject.transform.position.y, gameObject.transform.position.z + zTileIndex * tileDepth);

                GameObject currentTile = Instantiate(tilePrefab, tilePosition, Quaternion.identity);
                worldTiles.Add(currentTile);
            }
        }
    }



    public void OnGeneratePress(InputAction.CallbackContext context)
    {
        buttonPress = context.ReadValue<float>();
    }
}
