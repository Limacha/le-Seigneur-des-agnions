using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace inventory
{
    [CreateAssetMenu(menuName = "Items/New item/equipable")]
    public class EquipableData : ItemData
    {
        [Header("equip item variable")]
        [SerializeField] private GameObject oneHandPrefab;
        [SerializeField] private GameObject twoHandPrefab;
        
        public GameObject OneHandPrefab { get { return oneHandPrefab; } }
        public GameObject TwoHandPrefab { get { return twoHandPrefab; } }
    }
}