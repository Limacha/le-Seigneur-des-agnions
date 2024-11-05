using UnityEngine;
using UnityEngine.EventSystems;
using AllFonction;
using UnityEngine.UI;

namespace inventory
{
    public class ItemDragDrop : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private KeyBiding rotateKey; //key pour rotate les item
        [SerializeField] private ItemData itemData; //item drag
        [SerializeField] private Inventory inventory; //inventory

        private CanvasGroup canvasGroup; //canvas group pour gere les interaction
        private bool drag;

        public ItemData ItemData { get { return itemData; } set { itemData = value; } }
        public KeyBiding RotateKey { get { return rotateKey; } set { rotateKey = value; } }

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        }

        private void Update()
        {
            if (itemData != null && drag)
            {
                if (Input.GetKeyDown(rotateKey.key.ToLower()))
                {
                    //Debug.Log("rotate slot");
                    //func.show2DSpriteContent(itemData.patern);
                    itemData.rotatePatern();
                    itemData.Rotate = itemData.Rotate - 90;
                    if (itemData.Rotate <= 0)
                    {
                        itemData.Rotate = 360;
                    }
                    refreshDragDropObj();
                    //func.show2DSpriteContent(patern);
                }
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            //Debug.Log("OnBeginDrag");
            drag = true;
            canvasGroup.alpha = .6f; //transparense de l'image
            canvasGroup.blocksRaycasts = false; //active OnDrop sur itemSlot
        }
        public void OnDrag(PointerEventData eventData)
        {
            //Debug.Log("drag item");
            transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            //Debug.Log("OnEndDrag");
            drag = false;
            canvasGroup.alpha = 1f; //ne plus mettre l'image en transparent
            canvasGroup.blocksRaycasts = true; //reactive interaction avec item
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            //Debug.Log("OnPointerDown");
        }
        public void refreshDragDropObj()
        {
            var num = 0;

            var GLG = GetComponent<GridLayoutGroup>();
            GetComponent<CanvasGroup>();

            GetComponent<RectTransform>().sizeDelta = new Vector2(itemData.Patern.GetLength(0) * (inventory.SlotWidth + inventory.XSpacing), itemData.Patern.GetLength(1) * (inventory.SlotHeight + inventory.YSpacing));
            GetComponent<ItemDragDrop>().ItemData = itemData;
            GetComponent<ItemDragDrop>().RotateKey = rotateKey;
            GLG.cellSize = new Vector2(inventory.SlotWidth, inventory.SlotHeight);
            GLG.spacing = new Vector2(inventory.XSpacing, inventory.YSpacing);
            for (int y = 0; y < itemData.Patern.GetLength(1); y++)
            {
                for (int x = 0; x < itemData.Patern.GetLength(0); x++)
                {
                    var img = GetComponent<RectTransform>().GetChild(num).GetComponent<Image>();
                    img.name = $"image{x} {y}";
                    if (itemData.Patern[x, y] != null)
                    {
                        img.sprite = itemData.Patern[x, y];
                        GetComponent<RectTransform>().GetChild(num).GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, itemData.Rotate);
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