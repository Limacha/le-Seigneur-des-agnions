using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "scriptabelObject/items data/consomable", menuName = "Items/New item/ressource")]
public class RessourceData : ItemData
{
    public string source = "bipbip";
    public string Source { get { return source; } set { source = value; } }
}

