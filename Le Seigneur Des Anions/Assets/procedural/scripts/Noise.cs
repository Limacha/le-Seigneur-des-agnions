using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    // fonction de g�n�ration de la grid de la noise map
    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset) // scale est �gal a  valeur du bruit (blanc, noir, gris,...)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight]; // la noisemap

        // cr�e la seed (prng = pseudo random number generator)
        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        for(int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        if(scale <= 0)
        {
            scale = 0.0001f; // Pour �viter de se tromper si scale est inf�rieur a 0
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        // pour que la taille se change par le millieu
        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float amplitude = 1; // ajouter des d�tailles de moins en moins importants
                float frequency = 1; // + frequency �lev�e plus la hauteur de la map changeras rapidement
                float noiseHeight = 0;

                for(int i = 0; i < octaves; i++)
                {
                    float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x; // pour passer les variable int des for en float
                    float sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[i].y; // pareil

                    // algorythme de perlin
                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;
                    amplitude *= persistance; // augmenter l'amplitude
                    frequency *= lacunarity; // pareil mais pour frequency
                }

                // mettre toutes les valeurs entre 0 et 1
                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                } else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }

                noiseMap[x, y] = noiseHeight; // cr�e la noiseMap
            }
        }

        //envoie de r�ponses entre 0 & 1
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }
        return noiseMap;
    }
}
