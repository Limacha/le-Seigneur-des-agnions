using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace inventory
{
    public class SlotToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        ToolTip toolTip;
        Inventory inventory;
        public void Start()
        {
            inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
            toolTip = inventory.ToolTip.GetComponent<ToolTip>();
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (toolTip != null && GetComponent<Slot>().ItemData != null)
            {
                toolTip.Show();
                if (GetComponent<Slot>().ItemData.ID == inventory.ItemDataSprite.ID)
                {
                    toolTip.SetInfo(inventory.Content[GetComponent<Slot>().ItemData.RefX, GetComponent<Slot>().ItemData.RefY]);
                }
                else
                {
                    toolTip.SetInfo(GetComponent<Slot>().ItemData);
                }
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (toolTip != null)
            {
                toolTip.Hide();
            }
        }
    }
}