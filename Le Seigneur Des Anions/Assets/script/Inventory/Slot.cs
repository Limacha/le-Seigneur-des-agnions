using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    [SerializeField] private ItemData itemData; //item drag

    /// <summary>
    /// obtenir l'item du slot
    /// </summary>
    /// <returns>l'item dans le slot</returns>
    public ItemData GetItem()
    {
        return itemData;
    }

    /// <summary>
    /// changer l'item du slot
    /// </summary>
    /// <param name="item">l'item a mettre dans le slot</param>
    public void SetItem(ItemData item)
    {
        itemData = item;
    }
}
