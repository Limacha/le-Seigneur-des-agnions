using AllFonction;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace inventory
{
    public class SlotDragDrop : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private Inventory inventory; //inventory

        private CanvasGroup canvasGroup; //canvas group pour gere les interaction
        private GameObject dragItemObj; //l'object pris par la souris
        private bool drag = false; //si il tien un object avec sa souris
        private bool dragItem = false; //si il tien un object item avec sa souris
        private ItemData itemData; //l'item du slot

        public GameObject DragItemObj { get { return dragItemObj; } }

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        }

        /// <summary>
        /// fait tourner l'obj pris avec la souris
        /// </summary>
        public void Rotate()
        {
            if (drag && dragItem)
            {
                //Debug.Log("rotate slot");
                //func.show2DSpriteContent(dragItemObj.GetComponent<ItemDragDrop>().ItemData.Patern);
                dragItemObj.GetComponent<ItemDragDrop>().ItemData.rotatePatern();
                dragItemObj.GetComponent<ItemDragDrop>().ItemData.Rotate = dragItemObj.GetComponent<ItemDragDrop>().ItemData.Rotate - 90;
                if (dragItemObj.GetComponent<ItemDragDrop>().ItemData.Rotate <= 0)
                {
                    dragItemObj.GetComponent<ItemDragDrop>().ItemData.Rotate = 360;
                }
                refreshDragDropObj();
                //func.show2DSpriteContent(patern);
            }
        }

        /// <summary>
        /// fait equipe l'item pris avec la souris si possible
        /// </summary>
        public void TryEquip()
        {
            if (drag && dragItem)
            {
                if(inventory.EquipItem(Instantiate(dragItemObj.GetComponent<ItemDragDrop>().ItemData), true))
                {
                    if (dragItemObj.GetComponent<ItemDragDrop>().ItemData.Stack > 1)
                    {
                        dragItemObj.GetComponent<ItemDragDrop>().ItemData.Stack--;
                    }
                    else
                    {
                        Destroy(dragItemObj);
                        drag = false;
                        dragItem = false;
                        canvasGroup.alpha = 1f; //ne plus mettre l'image en transparent
                        canvasGroup.blocksRaycasts = true; //reactive interaction avec item
                        dragItemObj = null;
                    }
                }
            }
        }

        public void Drop()
        {
            if (drag && dragItem)
            {
                Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
                itemData.Drop(playerPosition);
                Destroy(dragItemObj);
                drag = false;
                dragItem = false;
                canvasGroup.alpha = 1f; //ne plus mettre l'image en transparent
                canvasGroup.blocksRaycasts = true; //reactive interaction avec item
                dragItemObj = null;
            }
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            //Debug.Log("OnBeginDrag");
            itemData = GetComponent<Slot>().ItemData;
            drag = true;
            canvasGroup.alpha = .6f; //transparense de l'image
            canvasGroup.blocksRaycasts = false; //active OnDrop sur itemSlot

            if (itemData != null)
            {
                if (itemData.ID == inventory.ItemDataSprite.ID)
                {
                    itemData = inventory.Content[itemData.RefX, itemData.RefY];
                }
                //Debug.Log(itemData.refX + " " + itemData.refY);
                if (itemData != null)
                {
                    //Debug.Log(itemData);
                    dragItem = true;
                    dragItemObj = inventory.CreateItem(itemData);
                    dragItemObj.GetComponent<ItemDragDrop>().ItemData = itemData;

                    inventory.RemoveItemFrom(itemData, itemData.RefX, itemData.RefY, inventory.GetPosInPatern(itemData.Patern));
                    inventory.RefreshInventory();
                    refreshDragDropObj();
                    inventory.SlotDrag = this;
                }
            }
        }
        public void OnDrag(PointerEventData eventData)
        {
            //Debug.Log("drag slot");
            //Debug.Log(itemData);
            if (dragItem)
            {
                dragItemObj.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            }
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            //Debug.Log("OnEndDrag");
            drag = false;
            dragItem = false;
            canvasGroup.alpha = 1f; //ne plus mettre l'image en transparent
            canvasGroup.blocksRaycasts = true; //reactive interaction avec item
            dragItemObj = null;
            inventory.SlotDrag = null;
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            //Debug.Log("OnPointerDown");
        }

        public void refreshDragDropObj()
        {
            var num = 0;

            var GLG = dragItemObj.GetComponent<GridLayoutGroup>();
            dragItemObj.GetComponent<CanvasGroup>();

            dragItemObj.GetComponent<RectTransform>().sizeDelta = new Vector2(dragItemObj.GetComponent<ItemDragDrop>().ItemData.Patern.GetLength(0) * (inventory.SlotWidth + inventory.XSpacing), dragItemObj.GetComponent<ItemDragDrop>().ItemData.Patern.GetLength(1) * (inventory.SlotHeight + inventory.YSpacing));
            dragItemObj.GetComponent<ItemDragDrop>().ItemData = dragItemObj.GetComponent<ItemDragDrop>().ItemData;
            GLG.cellSize = new Vector2(inventory.SlotWidth, inventory.SlotHeight);
            GLG.spacing = new Vector2(inventory.XSpacing, inventory.YSpacing);
            for (int y = 0; y < dragItemObj.GetComponent<ItemDragDrop>().ItemData.Patern.GetLength(1); y++)
            {
                for (int x = 0; x < dragItemObj.GetComponent<ItemDragDrop>().ItemData.Patern.GetLength(0); x++)
                {
                    var img = dragItemObj.GetComponent<RectTransform>().GetChild(num).GetComponent<Image>();
                    img.name = $"image{x} {y}";
                    if (dragItemObj.GetComponent<ItemDragDrop>().ItemData.Patern[x, y] != null)
                    {
                        img.sprite = dragItemObj.GetComponent<ItemDragDrop>().ItemData.Patern[x, y];
                        dragItemObj.GetComponent<RectTransform>().GetChild(num).GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, dragItemObj.GetComponent<ItemDragDrop>().ItemData.Rotate);
                    }
                    else
                    {
                        img.raycastTarget = false;
                        img.sprite = inventory.TransImage;
                    }
                    num++;
                }
            }
        }
    }
}