using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTextureAutoHight : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        TerrainData terrainData = Terrain.activeTerrain.terrainData;
        float[,,] splatmapData = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];


        for (int y = 0; y < terrainData.alphamapHeight; y++)
{
    for (int x = 0; x < terrainData.alphamapWidth; x++)
    {

        // read the height at this location
        float height = terrainData.GetHeight(x,y);

        // determine the mix of textures 1, 2 & 3 to use 
        // (using a vector3, since it can be lerped & normalized)

        Vector3 splat = new Vector3(0,1,0);
        if (height > 0.5) {
            splat = Vector3.Lerp(splat, new Vector3(0,0,1), (height-0.5f)*2 );
        } else {
            splat = Vector3.Lerp(splat, new Vector3(1,0,0), height*2 );
        }

        // now assign the values to the correct location in the array
        splat.Normalize();
        splatmapData[x, y, 0] = splat.x;
        splatmapData[x, y, 1] = splat.y;
        splatmapData[x, y, 2] = splat.z;
    }
}

// and finally assign the new splatmap to the terrainData:
terrainData.SetAlphamaps(0, 0, splatmapData);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
