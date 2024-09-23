using System;
using UnityEngine;

[CreateAssetMenu(fileName = "scriptabelObject/items data", menuName = "Items/New item")]
[System.Serializable]
public class ItemData : ScriptableObject
{
    public string id = Guid.NewGuid().ToString(); //id unique
    public string nom; //nom de l'item
    public string description; //description de l'item
    public int width; //larguer de la matrice
    public int height; //hauteur de la matrice
    public Sprite imgRef; //image de referance
    public Sprite[] listSprite; //liste des sprites a mettre dans le patern 0 = x0y0 / 2 = x0y2 / 6 = x2y0
    public Sprite[,] patern; //paterne des sprites dans l'inventaire
    public float poids = 0; //poids en gramme (plus tard)
    public bool stackable = false; //si il peut se stack
    public int stackLimit = 1; //nombre ou distance en m
    public int stack = 1;
    public int refX; //reference a l'item principal si sprite itemData
    public int refY; //reference a l'item principal si sprite itemData
    public int rotate = 360; //degrer de rotation

    public GameObject prefab; //l'item en 3D

    /// <summary>
    /// instancie l'item
    /// </summary>
    public void Init()
    {
        InitPatern();
    }

    /// <summary>
    /// instancie le tableau de sprite
    /// </summary>
    public void InitPatern()
    {
        patern = new Sprite[width, height];
        int pos = 0;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (pos < listSprite.Length)
                {
                    patern[x, y] = listSprite[pos++];
                    Debug.Log($"{nom} {listSprite[pos - 1]} pos{pos} y{y} x{x}");
                }
            }
        }
    }
}
