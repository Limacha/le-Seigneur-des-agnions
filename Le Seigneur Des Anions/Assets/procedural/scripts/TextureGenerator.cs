using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureGenerator
{
    // Génère une texture à partir d'une colorMap
    // colourMap : tableau de couleurs pour chaque pixel
    // width : largeur de la texture
    // height : hauteur de la texture
    public static Texture2D TextureFromColourMap(Color[] colourMap, int width, int height)
    {
        // Crée une nouvelle texture avec la largeur et la hauteur spécifiées
        Texture2D texture = new Texture2D(width, height);

        // Définit le mode de filtrage (ici "Point" pour un affichage sans interpolation)
        texture.filterMode = FilterMode.Point;

        // Définit le mode d'enroulement de la texture (ici "Clamp" pour éviter la répétition des bords)
        texture.wrapMode = TextureWrapMode.Clamp;

        // Applique le colorMap à la texture en définissant les couleurs des pixels
        texture.SetPixels(colourMap);

        // Applique les modifications à la texture pour la rendre visible
        texture.Apply();

        // Retourne la texture générée
        return texture;
    }

    // Génère une texture à partir d'une heightMap (carte de hauteur)
    // heightMap : tableau 2D de valeurs de hauteur (entre 0 et 1) représentant la topographie
    public static Texture2D TextureFromHeightMap(float[,] heightMap)
    {
        // Récupère la largeur et la hauteur de la heightMap
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        // Crée un tableau de couleurs correspondant à chaque pixel de la heightMap
        Color[] colourMap = new Color[width * height];

        // Remplit la colourMap en interpolant entre noir et blanc en fonction des valeurs de la heightMap
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // Interpolation linéaire entre noir (valeur basse) et blanc (valeur haute) selon la heightMap
                colourMap[y * width + x] = Color.Lerp(Color.black, Color.white, heightMap[x, y]);
            }
        }

        // Génère une texture à partir de la colourMap et la retourne
        return TextureFromColourMap(colourMap, width, height);
    }

}