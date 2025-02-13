using System.Collections.Generic;
using UnityEngine;
using inventory;

public class ItemDataManager : MonoBehaviour
{
    [SerializeField] private List<ItemData> itemList = new List<ItemData>();
    public List<ItemData> ItemList { get { return itemList; } }
    // Start is called before the first frame update
    void Start()
    {        
        /*string[] assetNames = AssetDatabase.FindAssets("", new[] { "Assets/scriptabelObject/items data/sc", "Assets/scriptabelObject/items data/ressource", "Assets/scriptabelObject/items data/consomable" });
        itemList.Clear();
        foreach (string SOName in assetNames)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOName);
            var item = AssetDatabase.LoadAssetAtPath<ItemData>(SOpath);
            itemList.Add(item);
        }*/ 

        foreach (var item in itemList)
        {
            //Debug.Log(item);
            item.Init();
        }
    }
}
