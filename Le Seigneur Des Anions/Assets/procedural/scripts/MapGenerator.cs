using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int mapWidth;
    public int mapHeight;
    public float noiseScale;

    // refaire directement la noisemap quand on change une donnée
    public bool autoUpdate;

    public void GenerateMap()
    {
        // appel de public static float[,] GenerateNoiseMap
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, noiseScale);

        // appeler le map display tout en indiquant qui il est
        MapDisplay display = FindObjectOfType<MapDisplay>();
        display.DrawNoiseMap(noiseMap);
    }
}
