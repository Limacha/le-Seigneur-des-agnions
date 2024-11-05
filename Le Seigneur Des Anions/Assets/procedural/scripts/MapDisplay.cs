using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    // Renderer pour afficher une texture sur un plane dans le jeu
    public Renderer textureRenderer;

    // Composants Mesh utilisés pour la création et l'affichage d'un maillage 3D
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;

    // Méthode pour afficher une texture 2D sur le plane
    public void DrawTexture(Texture2D texture)
    {
        // Assigne la texture donnée au matériau du Renderer, permettant de l'afficher sur l'objet
        textureRenderer.sharedMaterial.mainTexture = texture;

        // Redimensionne le plane pour correspondre à la taille de la texture
        textureRenderer.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }

    // Méthode pour afficher un maillage (Mesh) 3D avec une texture
    public void DrawMesh(MeshData meshData, Texture2D texture)
    {
        // Crée un maillage à partir des données fournies et l'assigne au MeshFilter
        meshFilter.sharedMesh = meshData.CreateMesh();

        // Applique la texture au maillage via le MeshRenderer
        meshRenderer.sharedMaterial.mainTexture = texture;
    }

}