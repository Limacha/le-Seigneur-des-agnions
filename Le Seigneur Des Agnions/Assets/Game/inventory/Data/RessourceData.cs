using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace inventory
{
    [CreateAssetMenu(fileName = "scriptabelObject/items data/consomable", menuName = "Items/New item/ressource")]
    public class RessourceData : ItemData
    {
        [Header("ressource item variable")]
        [SerializeField] private string source = "bipbip";
        public string Source { get { return source; } set { source = value; } }
    }
}