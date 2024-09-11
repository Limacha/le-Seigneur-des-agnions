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
    /// <summary>
    /// liste des sprites a mettre dans le patern dabord les y puis les x (0 = x0y0 / 2 = x0y2 / 6 = x2y0)
    /// </summary>
    public Sprite[] listSprite; //liste des sprites a mettre dans le patern 0 = x0y0 / 2 = x0y2 / 6 = x2y0
    public Sprite[,] patern; //paterne des sprites dans l'inventaire
    public float poids = 0; //poids en gramme (plus tard)
    public bool stackable = false; //si il peut se stack
    public int stackLimit = 1; //nombre ou distance en m
    public int refX; //reference a l'item principal si sprite itemData
    public int refY; //reference a l'item principal si sprite itemData
    public int rotate; //degrer de rotation

    public GameObject prefab; //l'item en 3D

    public ItemType type; //type de l'item
    /// <summary>
    /// instancie le tableau de sprite
    /// </summary>
    public void SetPatern()
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
                    //Debug.Log($"{listSprite[pos - 1]} pos{pos} y{y} x{x}");
                }
            }
        }
    }
}
/// <summary>
/// enumeration de tout les type possible pour un item
/// </summary>
public enum ItemType
{
    /// <summary> item sans effet </summary>
    ressource,
    /// <summary> item comsomable par l'utilisateur </summary>
    consomable,
    /// <summary> block un case de l'inventaire destiner a un item qui prend de la place </summary>
    sprite
}
