using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// Pour avoir un button de génération pour ne pas devoir lancer la game a chaque fois

[CustomEditor (typeof(MapGenerator))] // aller modifier le map generator pour pouvoir l'accédé
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapGenerator mapGen = (MapGenerator)target;


        // vérifier si auto update
        if (DrawDefaultInspector())
        {
            if (mapGen.autoUpdate)
            {
                // appeler la fonction de génération de map
                mapGen.GenerateMap();
            }
        }

        if (GUILayout.Button("Generate"))
        {
            // appeler la fonction de génération de map
            mapGen.GenerateMap();
        }
    }
}
