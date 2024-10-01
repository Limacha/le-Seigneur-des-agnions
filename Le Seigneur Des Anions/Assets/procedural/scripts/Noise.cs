using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    // fonction de génération de la grid de la noise map
    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, float scale) // scale est égal a  valeur du bruit (blanc, noir, gris,...)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight]; // la noisemap

        if(scale <= 0)
        {
            scale = 0.0001f; // Pour éviter de se tromper si scale est inférieur a 0
        }

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float sampleX = x / scale; // pour passer les variable int des for en float
                float sampleY = y / scale; // pareil

                // algorythme de perlin
                float perlinValue = Mathf.PerlinNoise(sampleX, sampleY);
                noiseMap[x,y] = perlinValue;
            }
        }

        return noiseMap;
    }
}
