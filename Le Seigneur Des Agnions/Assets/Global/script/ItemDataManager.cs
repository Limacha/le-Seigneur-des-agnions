using System.Collections.Generic;
using UnityEngine;
using inventory;
using debugCommand;

public class ItemDataManager : MonoBehaviour
{
    [SerializeField] private ItemData[] items;
    public ItemData[] Items { get { return items; } }

    void Awake()
    {
        items = Resources.LoadAll<ItemData>("ItemDatas");
    }

    void Start()
    {        
        foreach (var item in items)
        {
            //Debug.Log(item);
            item.Init();
        }
    }
}
