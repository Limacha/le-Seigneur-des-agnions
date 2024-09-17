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
        if (toolTip != null && GetComponent<Slot>().GetItem() != null)
        {
            toolTip.Show();
            if(GetComponent<Slot>().GetItem().id == inventory.GetItemDataSprite().id)
            {
                toolTip.SetInfo(inventory.GetContent()[GetComponent<Slot>().GetItem().refX, GetComponent<Slot>().GetItem().refY]);
            }
            else
            {
                toolTip.SetInfo(GetComponent<Slot>().GetItem());
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
