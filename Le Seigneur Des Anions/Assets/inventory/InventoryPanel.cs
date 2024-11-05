using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace inventory
{
    public class InventoryPanel : MonoBehaviour, IDropHandler
    {
        [SerializeReference] private Inventory inventory;
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
                    var elemRt = elem.GetComponent<RectTransform>(); //rect transform de l'element
                    var canvas = GameObject.Find("Canvas"); //le canvas
                    var canvasRt = canvas.GetComponent<RectTransform>(); //le rect du canvas
                    var rt = GetComponent<RectTransform>(); //le rt de inventory panel

                    if (elem.GetComponent<ItemDragDrop>() != null)
                    {
                        ItemData item = elem.GetComponent<ItemDragDrop>().ItemData; //item en ItemData
                                                                                    //Debug.Log(elem);
                                                                                    //Debug.Log(item);

                        float spacingWidth = inventory.XSpacing; //l'espacement des case en horizontal
                        float spacingHeight = inventory.YSpacing; //l'espacement des case en vertical
                        float slotWidth = inventory.SlotWidth; //la largeur de slot
                        float slotHeight = inventory.SlotHeight; //la hauteur des slot
                        int[] pos = inventory.GetPosInPatern(item.Patern);
                        ItemData[,] content = inventory.Content; //contenu de l'inventaire

                        float decalX = (item.Patern.GetLength(0) % 2 == 0) ? 50 : 25; //decalage de la grille en x
                        float decalY = (item.Patern.GetLength(1) % 2 == 0) ? 50 : 25; //decalage de la grille en y

                        float paddingLeft = rt.GetChild(0).GetComponent<RectTransform>().rect.xMin - rt.rect.xMin; //l'espace gauche entre l'inventory panel et le columnContent
                        float paddingTop = rt.GetChild(0).GetComponent<RectTransform>().rect.yMin - rt.rect.yMin; //l'espace en haut entre l'inventory panel et le columnContent

                        #region debug
                        //Debug.Log($"{paddingLeft}");
                        //Debug.Log($"{paddingTop}");

                        //Debug.Log((canvasRt.position.y - rt.position.y - paddingTop));
                        //Debug.Log((canvasRt.rect.height - rt.rect.height) / 2);
                        //Debug.Log(elemRt.position.y);

                        //Debug.Log($"x{elemRt.position.x - ((canvasRt.rect.width - rt.rect.width) / 2 - (canvasRt.position.x - rt.position.x - paddingLeft))}");
                        //Debug.Log($"y{elemRt.position.y - ((canvasRt.rect.height - rt.rect.height) / 2 - (canvasRt.position.y - rt.position.y - paddingTop))}");

                        //(elemRt.position.x - decalX - ((canvasRt.rect.width - rt.rect.width) / 2 - (canvasRt.position.x - rt.position.x - paddingLeft))) == position dans le column content
                        //(slotWidth + spacingWidth) = creez une grille de la taille des slot
                        //content.GridSize.y - = car y comence en bas
                        //Debug.Log($"xx{(int)Math.Round((elemRt.position.x - decalX - ((canvasRt.rect.width - rt.rect.width) / 2 - (canvasRt.position.x - rt.position.x - paddingLeft))) / (slotWidth + spacingWidth))}");
                        //Debug.Log($"yy{content.GridSize.y - (int)Math.Round((elemRt.position.y + decalY - ((canvasRt.rect.height - rt.rect.height) / 2 - (canvasRt.position.y - rt.position.y - paddingTop))) / (slotHeight + spacingHeight))}");
                        #endregion

                        int x = (int)Math.Round((elemRt.position.x - decalX - ((canvasRt.rect.width - rt.rect.width) / 2 - (canvasRt.position.x - rt.position.x - paddingLeft))) / (slotWidth + spacingWidth)); //position en x dans l'inventaire
                        int y = content.GetLength(1) - (int)Math.Round((elemRt.position.y + decalY - ((canvasRt.rect.height - rt.rect.height) / 2 - (canvasRt.position.y - rt.position.y - paddingTop))) / (slotHeight + spacingHeight)); //position en y dans l'inventaire

                        #region debug
                        //Debug.Log($"x{x} y{y}");
                        //Debug.Log(pos[0] + " " + pos[1]);
                        //Debug.Log(item.patern.GetLength(0) + " " + item.patern.GetLength(1));

                        //Debug.Log((int)Math.Ceiling((decimal)item.patern.GetLength(0) / 2));
                        //Debug.Log((int)Math.Ceiling((decimal)item.patern.GetLength(1) / 2));
                        #endregion

                        //decalage des position par raport a l'affichage
                        x -= (int)Math.Ceiling((decimal)item.Patern.GetLength(0) / 2) - 1 - pos[0]; //-1 car 1/1 != 0
                        y -= (int)Math.Ceiling((decimal)item.Patern.GetLength(1) / 2) - 1 - pos[1];
                        //Debug.Log($"x{x} y{y}");

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
    }
}