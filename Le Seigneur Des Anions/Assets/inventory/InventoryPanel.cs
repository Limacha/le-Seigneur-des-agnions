using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

namespace inventory
{
    public class InventoryPanel : MonoBehaviour, IDropHandler
    {
        [SerializeReference] private Inventory inventory;
        public Texture texture;
        public GUISkin skin;
        [Header("style text area")]
        [SerializeField] private int labelWidth;
        [SerializeField] private int labelHeight;
        [SerializeField] private float labelMargX;
        [SerializeField] private float labelMargY;
        [SerializeField] private Color labelBorderColor;
        [SerializeField] private Color labelTextColor;
        [SerializeField] private Color labelBackColor;
        #region system1
        public void OnDrop(PointerEventData eventData)
        {
            //Debug.Log("onDrop");
            var elem = eventData.pointerDrag; //element drag

            //Debug.Log(elem);
            if (elem != null)
            {
                if (elem.TryGetComponent<SlotDragDrop>(out var component))
                {
                    elem = component.DragItemObj;
                }

                if (elem != null)
                {
                    RectTransform elemRt = elem.GetComponent<RectTransform>(); //rect transform de l'element
                    CanvasScaler canvasreso = GameObject.Find("Canvas").GetComponent<CanvasScaler>(); //le canvas
                    RectTransform rt = GetComponent<RectTransform>(); //le rt de inventory panel
                    RectTransform rtChild = rt.GetChild(1).GetComponent<RectTransform>(); //rt de l'enfant
                    float ratioX = Screen.width / canvasreso.referenceResolution.x; //proportion taille/reference

                    if (elem.GetComponent<ItemDragDrop>() != null)
                    {
                        ItemData item = elem.GetComponent<ItemDragDrop>().ItemData; //item en ItemData
                        float slotWidth = inventory.SlotWidth; //la largeur de slot
                        float slotHeight = inventory.SlotHeight; //la hauteur des slot
                        double prX = elemRt.position.x - (rtChild.position.x - (rtChild.rect.width / 2 * ratioX));
                        double prY = elemRt.position.y - (rtChild.position.y - (rtChild.rect.height / 2 * ratioX));

                        Debug.Log(prX);
                        Debug.Log(prY);

                        float decalX = (item.Patern.GetLength(0) % 2 == 0)? 25 : 0;
                        float decalY = (item.Patern.GetLength(1) % 2 == 0)? 25 : 0;

                        int x = (int)((prX + (decalX * ratioX)) / (slotWidth * ratioX));
                        int y = inventory.ContentHeight - 1 - (int)((prY - (decalY * ratioX)) / (slotHeight * ratioX));

                        Debug.Log(x);
                        Debug.Log(y);

                        x += inventory.GetPosInPatern(item.Patern)[0];
                        y += inventory.GetPosInPatern(item.Patern)[1];

                        Debug.Log(x);
                        Debug.Log(y);

                        x -= (int)(item.Patern.GetLength(0) / 2);
                        y -= (int)(item.Patern.GetLength(1) / 2);

                        Debug.Log(x);
                        Debug.Log(y);

                        if (inventory.VerifPlace(item, x, y))
                        {
                            inventory.PlaceItemInInventory(item, x, y);
                        }
                        else
                        {
                            inventory.PlaceItemInInventory(item, item.RefX, item.RefY);
                        }
                        Destroy(elem);
                        inventory.RefreshInventory();
                        elemRt.position = rt.position;
                    }
                }
            }
        }
        /*
        public void OnGUI()
        {
            RectTransform rt = GetComponent<RectTransform>(); //le rt de inventory panel
            RectTransform rtChild = rt.GetChild(1).GetComponent<RectTransform>();
            var canvas = GameObject.Find("Canvas"); //le canvas
            var canvasreso = canvas.GetComponent<CanvasScaler>(); //le canvas
            string label = "";
            var ratioX = Screen.width / canvasreso.referenceResolution.x;
            //var ratioY = Screen.height / canvasreso.referenceResolution.y;

            //label += $"panel x{rt.position.x} y{rt.position.y} \n";
            //label += $"panel W{rt.rect.width} H{rt.rect.height} \n";

            //label += $"panel 0 x{rt.position.x - rt.rect.width/2} y{rt.position.y - rt.rect.height/2} \n";
            //label += $"panel 0 + pad x{rt.position.x - rt.rect.width/2 + paddingLeft} y{rt.position.y - rt.rect.height/2 + paddingTop} \n";

            label += $"child x{rtChild.position.x} y{rtChild.position.y} \n";
            label += $"child W{rtChild.rect.width} H{rtChild.rect.height} \n";
            label += $"ecran W{Screen.width} H{Screen.height} \n";
            label += $"origi W{canvasreso.referenceResolution.x} H{canvasreso.referenceResolution.y} \n";
            label += $"ratio X{ratioX} YratioY \n";

            GUI.skin = skin;
            GUI.backgroundColor = labelBackColor;
            GUI.Box(new Rect(0, 0, labelWidth, labelHeight), "");
            GUI.backgroundColor = labelBorderColor;
            GUI.contentColor = labelTextColor;
            GUI.Label(new Rect(labelMargX, labelMargY, (labelWidth - (labelMargX * 2))*ratioX, (labelHeight - (labelMargY * 2))*ratioX), label);
            GUI.skin.label.fontSize = 10;


            GUI.color = Color.white;
            //GUI.DrawTexture(new Rect(rtChild.position, new Vector2(1, 1)), texture);
            //GUI.DrawTexture(new Rect(new Vector2(rt.position.x - rt.rect.width / 2, rt.position.y - rt.rect.height / 2), new Vector2(1, 1)), texture);
            GUI.DrawTexture(new Rect(rtChild.position, new Vector2(1, 1)), texture);
            GUI.DrawTexture(new Rect(new Vector2(rtChild.position.x - (rtChild.rect.width / 2), rtChild.position.y - (rtChild.rect.height / 2)), new Vector2(1, 1)), texture);
            GUI.DrawTexture(new Rect(new Vector2(rtChild.position.x - (rtChild.rect.width / 2) * ratioX, rtChild.position.y - (rtChild.rect.height / 2) * ratioX), new Vector2(1, 1)), texture);
            GUI.color = Color.red;
            GUI.backgroundColor = labelBackColor;
            GUI.Box(new Rect(new Vector2(rtChild.position.x - (rtChild.rect.width / 2) * ratioX, rtChild.position.y - (rtChild.rect.height / 2) * ratioX), new Vector2(rtChild.rect.width * ratioX, rtChild.rect.height * ratioX)), "");
            for (int i = 0; i < inventory.ContentWidth; i++)
            {
                GUI.color = Color.green;
                GUI.backgroundColor = labelBackColor;
                GUI.Box(new Rect(new Vector2(rtChild.position.x - (rtChild.rect.width / 2) * ratioX + (i * 50 * ratioX), rtChild.position.y - (rtChild.rect.height / 2) * ratioX), new Vector2(50*ratioX, 50*ratioX)), "");
            }
        }
        */
        #endregion
    }
}