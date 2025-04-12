using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using inventory;

[System.Serializable]
public class ItemSaveData
{
    public string _id; //l'id de l'object
    public int refX; //position en x
    public int refY; // position en y
    public int stack; //le nombre de stack
    public int rotation; //rotation de l'item

    /// <summary>
    /// genere les donnees de L'item
    /// </summary>
    /// <param name="item">l'item' a stocker les donnees</param>
    public ItemSaveData(ItemData item)
    {
        _id = item.ID;
        refX = item.RefX;
        refY = item.RefY;
        stack = item.Stack;
        rotation = item.Rotate;
    }
}
