using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ItemSaveData
{
    public string id; //l'id de l'object
    public int refX; //position en x
    public int refY; // position en y
    public int stack; //le nombre de stack

    /// <summary>
    /// genere les donnees de L'item
    /// </summary>
    /// <param name="item">l'item' a stocker les donnees</param>
    public ItemSaveData(ItemData item)
    {
        id = item.id;
        refX = item.refX;
        refY = item.refY;
        stack = item.stack;
    }
}
