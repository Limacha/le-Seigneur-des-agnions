using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    public Renderer textureRenderer;// plane ingame

    public void DrawNoiseMap(float[,] noiseMap)
    {
        int width = noiseMap.GetLength(0);// reprise de la largeur et hauteur
        int height = noiseMap.GetLength(1);

        Texture2D texture = new Texture2D(width, height); // création de la matrice texture pour crée la texture

        Color[] colourMap = new Color[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                colourMap[y * width + x] = Color.Lerp(Color.black, Color.white, noiseMap[x, y]);// trouver l'endroit ou mettre le pixel dans la colorMap
            }
        }

        texture.SetPixels(colourMap);// mettre les pixels
        texture.Apply();// appliquer la texture

        // rendre la texture et l'appliquer avec la taille du plane de 2d en 3d
        textureRenderer.sharedMaterial.mainTexture = texture; 
        textureRenderer.transform.localScale = new Vector3(width, 1, height);
    }
}
