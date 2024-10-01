using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "scriptabelObject/items data/consomable", menuName = "Items/New item/consomable")]
public class ConsomableData : ItemData
{
    public string source = "conso"; //juste pour test
    public string Source { get; set; }
}
