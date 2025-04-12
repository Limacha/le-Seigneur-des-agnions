using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace inventory
{
    [CreateAssetMenu(fileName = "scriptabelObject/items data/consomable", menuName = "Items/New item/consomable")]
    public class ConsomableData : ItemData
    {
        [Header("consomable item variable")]
        [SerializeField] private GameObject consoPref; //prefab pour quand on le consome
        public GameObject ConsoPref { get { return consoPref; } }
    }
}