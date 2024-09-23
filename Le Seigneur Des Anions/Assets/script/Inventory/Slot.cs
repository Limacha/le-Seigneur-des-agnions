using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    [SerializeField] private ItemData itemData; //item drag
    public ItemData ItemData {  get { return itemData; } set { itemData = value; } }

}
