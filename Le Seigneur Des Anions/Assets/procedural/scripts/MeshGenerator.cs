using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator
{
    // Génère un maillage 3D basé sur une heightMap, un multiplicateur de hauteur et une courbe de hauteur
    public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier, AnimationCurve heightCurve, int levelOfDetail)
    {
        // Récupère les dimensions de la heightMap
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        // Coordonnées du point en haut à gauche du maillage
        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;

        // Détermine le pas de simplification du maillage en fonction du niveau de détail (LOD)
        int meshSimplificationIncrement = (levelOfDetail == 0) ? 1 : levelOfDetail * 2;
        int verticesPerLine = (width - 1) / meshSimplificationIncrement + 1;

        // Crée un objet MeshData pour stocker les données du maillage (vertices, triangles, UV)
        MeshData meshData = new MeshData(verticesPerLine, verticesPerLine);
        int vertexIndex = 0;

        // Parcours des points de la heightMap avec un incrément selon le LOD pour créer les vertices
        for (int y = 0; y < height; y += meshSimplificationIncrement)
        {
            for (int x = 0; x < width; x += meshSimplificationIncrement)
            {
                // Crée un vertex avec les coordonnées (x, y) et applique la courbe et le multiplicateur de hauteur
                meshData.vertices[vertexIndex] = new Vector3(topLeftX + x, heightCurve.Evaluate(heightMap[x, y]) * heightMultiplier, topLeftZ - y);

                // Définit les coordonnées UV (2D) pour chaque vertex (utilisées pour l'application des textures)
                meshData.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);

                // Si on n'est pas au bord du maillage, ajoute deux triangles pour chaque quadrilatère formé par quatre vertices
                if (x < width - 1 && y < height - 1)
                {
                    meshData.AddTriangle(vertexIndex, vertexIndex + verticesPerLine + 1, vertexIndex + verticesPerLine);
                    meshData.AddTriangle(vertexIndex + verticesPerLine + 1, vertexIndex, vertexIndex + 1);
                }

                vertexIndex++;
            }
        }

        // Retourne les données du maillage
        return meshData;
    }
}

public class MeshData
{
    // Tableau des vertices du maillage (coordonnées 3D)
    public Vector3[] vertices;

    // Tableau des indices des triangles (chaque triangle est défini par 3 indices de vertices)
    public int[] triangles;

    // Tableau des coordonnées UV (coordonnées 2D pour le mappage des textures)
    public Vector2[] uvs;

    // Compteur pour l'index des triangles
    int triangleIndex;

    // Constructeur de la classe MeshData, initialise les tableaux de vertices, triangles et UVs
    public MeshData(int meshWidth, int meshHeight)
    {
        // Initialise les tableaux en fonction de la largeur et de la hauteur du maillage
        vertices = new Vector3[meshWidth * meshHeight];
        uvs = new Vector2[meshWidth * meshHeight];
        triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6]; // 6 indices pour chaque quadrilatère (2 triangles)
    }

    // Méthode pour ajouter un triangle à partir de trois indices de vertices
    public void AddTriangle(int a, int b, int c)
    {
        triangles[triangleIndex] = a;
        triangles[triangleIndex + 1] = b;
        triangles[triangleIndex + 2] = c;
        triangleIndex += 3; // Incrémente l'index de triangle
    }

    // Crée un Mesh Unity à partir des données stockées (vertices, triangles, UV)
    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        // Recalcule les normales du maillage pour améliorer l'éclairage et l'apparence des surfaces
        mesh.RecalculateNormals();

        return mesh;
    }
}