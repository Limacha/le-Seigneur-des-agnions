using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    ToolTip toolTip;
    Inventory inventory;
    public void Start()
    {
        toolTip = GameObject.Find("ToolTip").GetComponent<ToolTip>();
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (toolTip != null && GetComponent<Slot>().ItemData != null)
        {
            toolTip.Show();
            if(GetComponent<Slot>().ItemData.id == inventory.ItemDataSprite.id)
            {
                toolTip.SetInfo(inventory.Content[GetComponent<Slot>().ItemData.refX, GetComponent<Slot>().ItemData.refY]);
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
