using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace inventory
{
    [CreateAssetMenu(menuName = "Items/New item/hands")]
    public class HandsData : ItemData
    {
        [Header("hands item variable")]
        [SerializeField] private GameObject leftHandPrefab;
        [SerializeField] private GameObject rightHandPrefab;
        
        public GameObject LeftHandPrefab { get { return leftHandPrefab; } }
        public GameObject RightHandPrefab { get { return rightHandPrefab; } }
    }
}