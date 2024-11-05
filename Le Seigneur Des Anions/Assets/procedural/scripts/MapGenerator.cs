using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    // Enum pour choisir le mode d'affichage de la carte (NoiseMap = carte en noir et blanc, ColourMap = carte en couleurs, Mesh = modèle 3D)
    public enum DrawMode { NoiseMap, ColourMap, Mesh }
    public DrawMode drawMode;

    // Constante définissant la taille du chunk de la carte (optimisation LOD)
    const int mapChunkSize = 241;

    // Paramètre pour ajuster le niveau de détail (LOD)
    [Range(0, 6)]
    public int levelOfDetail;

    // Échelle de la noise map (détermine le zoom du bruit)
    public float noiseScale;

    // Nombre d'octaves pour le bruit fractal
    public int octaves;

    // Facteur de persistance (influence l'amplitude des octaves successives)
    [Range(0, 1)]
    public float persistance;

    // Facteur de lacunarité (influence la fréquence des octaves successives)
    public float lacunarity;

    // Graine utilisée pour générer des valeurs pseudo-aléatoires
    public int seed;

    // Offset pour déplacer la carte générée
    public Vector2 offset;

    // Multiplicateur pour ajuster la hauteur des vertices dans le modèle 3D
    public float meshHeightMultiplier;

    // Courbe d'animation utilisée pour moduler la hauteur des vertices du mesh
    public AnimationCurve meshHeightCurve;

    // Activer la mise à jour automatique lorsque les paramètres changent dans l'éditeur
    public bool autoUpdate;

    // Tableau définissant les régions du terrain et leurs couleurs en fonction de la hauteur
    public TerrainType[] regions;

    // Méthode pour générer la carte
    public void GenerateMap()
    {
        // Appel à la fonction statique GenerateNoiseMap pour créer une noise map
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance, lacunarity, offset);

        // Tableau de couleurs basé sur la noise map et les régions du terrain
        Color[] colourMap = new Color[mapChunkSize * mapChunkSize];

        // Parcours de chaque point de la noise map pour attribuer une couleur en fonction de la hauteur
        for (int y = 0; y < mapChunkSize; y++)
        {
            for (int x = 0; x < mapChunkSize; x++)
            {
                float currentHeight = noiseMap[x, y];

                // Sélection de la couleur selon la hauteur du point et les régions définies
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight <= regions[i].height)
                    {
                        colourMap[y * mapChunkSize + x] = regions[i].colour;
                        break;
                    }
                }
            }
        }

        // Récupération de la classe MapDisplay pour afficher la carte
        MapDisplay display = FindAnyObjectByType<MapDisplay>();

        // Selon le mode sélectionné, dessiner la noise map, la carte en couleurs, ou le maillage 3D
        if (drawMode == DrawMode.NoiseMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        }
        else if (drawMode == DrawMode.ColourMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromColourMap(colourMap, mapChunkSize, mapChunkSize));
        }
        else if (drawMode == DrawMode.Mesh)
        {
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail), TextureGenerator.TextureFromColourMap(colourMap, mapChunkSize, mapChunkSize));
        }
    }

    // Méthode appelée lorsque des valeurs sont modifiées dans l'éditeur pour valider et ajuster les paramètres
    private void OnValidate()
    {
        // Empêche la lacunarité d'être inférieure à 1
        if (lacunarity < 1)
        {
            lacunarity = 1;
        }

        // Empêche le nombre d'octaves d'être inférieur à 0
        if (octaves < 0)
        {
            octaves = 0;
        }
    }
}

// Struct pour définir les types de terrain (nom, hauteur maximale et couleur associée)
[System.Serializable]
public struct TerrainType
{
    public string name;   // Nom de la région du terrain (ex : "plaine", "montagne")
    public float height;  // Hauteur maximale pour cette région
    public Color colour;  // Couleur associée à cette région
}