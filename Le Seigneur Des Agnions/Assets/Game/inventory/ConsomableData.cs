using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace inventory
{
    [CreateAssetMenu(fileName = "scriptabelObject/items data/consomable", menuName = "Items/New item/consomable")]
    public class ConsomableData : ItemData
    {
        [Header("consomable item variable")]
        [SerializeField] private string source = "conso"; //juste pour test
        public string Source { get { return source; } set { source = value; } }
    }
}