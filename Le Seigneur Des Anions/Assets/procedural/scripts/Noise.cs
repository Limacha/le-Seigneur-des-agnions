using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    // Fonction pour générer une noise map
    // mapWidth : largeur de la carte
    // mapHeight : hauteur de la carte
    // seed : graine pour générer des valeurs pseudo-aléatoires
    // scale : échelle de la noise map (détermine la granularité du bruit)
    // octaves : nombre d'octaves pour la génération de bruit
    // persistance : influence de l'amplitude des octaves successives
    // lacunarity : influence de la fréquence des octaves successives
    // offset : déplacement appliqué aux coordonnées (x, y)
    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset)
    {
        // Crée un tableau 2D pour stocker les valeurs de bruit
        float[,] noiseMap = new float[mapWidth, mapHeight];

        // Initialise le générateur de nombres pseudo-aléatoires avec la graine donnée
        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];

        // Génére des offsets aléatoires pour chaque octave
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        // Empêche l'échelle d'être trop petite (ce qui pourrait provoquer des erreurs)
        if (scale <= 0)
        {
            scale = 0.0001f;
        }

        // Variables pour suivre les valeurs de bruit maximales et minimales
        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        // Calcul des moitiés de la largeur et de la hauteur pour centrer le bruit
        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;

        // Boucle sur chaque point de la noise map
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                // Initialise les valeurs d'amplitude et de fréquence pour les octaves
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                // Génère le bruit en utilisant plusieurs octaves
                for (int i = 0; i < octaves; i++)
                {
                    // Calcule les coordonnées d'échantillonnage en fonction de la fréquence et de l'offset
                    float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x;
                    float sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[i].y;

                    // Génère une valeur de bruit de Perlin pour ces coordonnées
                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1; // Multiplie par 2 et soustrait 1 pour obtenir des valeurs entre -1 et 1
                    noiseHeight += perlinValue * amplitude;

                    // Réduit l'amplitude et augmente la fréquence pour les octaves suivantes
                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                // Met à jour les valeurs maximales et minimales de bruit
                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }

                // Affecte la hauteur de bruit à la noise map
                noiseMap[x, y] = noiseHeight;
            }
        }

        // Normalise les valeurs de la noise map entre 0 et 1
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }

        // Retourne la noise map générée
        return noiseMap;
    }
}
