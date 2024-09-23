using AllFonction;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotDragDrop : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private KeyBiding rotateKey; //key pour rotate les item
    [SerializeField] private Inventory inventory; //inventory

    private CanvasGroup canvasGroup; //canvas group pour gere les interaction
    private readonly Fonction func = new Fonction(); //fonctionpour debuger et rotate
    private GameObject dragItemObj;
    private bool drag = false;
    private bool dragItem = false;
    private ItemData itemData;

    public GameObject DragItemObj { get { return dragItemObj; } }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
    }

    private void Update()
    {
        if (drag)
        {
            if (dragItem)
            {
                if (Input.GetKeyDown(rotateKey.key.ToLower()))
                {
                    //Debug.Log("rotate slot");
                    //func.show2DSpriteContent(dragItemObj.GetComponent<ItemDragDrop>().ItemData.patern);
                    dragItemObj.GetComponent<ItemDragDrop>().ItemData.patern = func.rotate2DSprite(dragItemObj.GetComponent<ItemDragDrop>().ItemData.patern);
                    dragItemObj.GetComponent<ItemDragDrop>().ItemData.rotate = dragItemObj.GetComponent<ItemDragDrop>().ItemData.rotate - 90;
                    if (dragItemObj.GetComponent<ItemDragDrop>().ItemData.rotate <= 0)
                    {
                        dragItemObj.GetComponent<ItemDragDrop>().ItemData.rotate = 360;
                    }
                    refreshDragDropObj();
                    //func.show2DSpriteContent(patern);
                }
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("OnBeginDrag");
        itemData = GetComponent<Slot>().ItemData;
        drag = true;
        canvasGroup.alpha = .6f; //transparense de l'image
        canvasGroup.blocksRaycasts = false; //active OnDrop sur itemSlot

        //Debug.Log(itemData);
        if (itemData != null)
        {
            if (itemData.id == inventory.ItemDataSprite.id)
            {
                itemData = inventory.Content[itemData.refX, itemData.refY];
            }
            //Debug.Log(itemData.refX + " " + itemData.refY);
            if (itemData != null)
            {
                //Debug.Log(itemData);
                dragItem = true;
                dragItemObj = inventory.CreateItem(itemData);
                dragItemObj.GetComponent<ItemDragDrop>().ItemData = itemData;

                inventory.RemoveItemFrom(itemData, itemData.refX, itemData.refY, inventory.GetPosInPatern(itemData.patern));
                inventory.RefreshInventory();
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

        dragItemObj.GetComponent<RectTransform>().sizeDelta = new Vector2(dragItemObj.GetComponent<ItemDragDrop>().ItemData.patern.GetLength(0) * (inventory.SlotWidth + inventory.XSpacing), dragItemObj.GetComponent<ItemDragDrop>().ItemData.patern.GetLength(1) * (inventory.SlotHeight + inventory.YSpacing));
        dragItemObj.GetComponent<ItemDragDrop>().ItemData = dragItemObj.GetComponent<ItemDragDrop>().ItemData;
        dragItemObj.GetComponent<ItemDragDrop>().RotateKey = rotateKey;
        GLG.cellSize = new Vector2(inventory.SlotWidth, inventory.SlotHeight);
        GLG.spacing = new Vector2(inventory.XSpacing, inventory.YSpacing);
        for (int y = 0; y < dragItemObj.GetComponent<ItemDragDrop>().ItemData.patern.GetLength(1); y++)
        {
            for (int x = 0; x < dragItemObj.GetComponent<ItemDragDrop>().ItemData.patern.GetLength(0); x++)
            {
                var img = dragItemObj.GetComponent<RectTransform>().GetChild(num).GetComponent<Image>();
                img.name = $"image{x} {y}";
                if (dragItemObj.GetComponent<ItemDragDrop>().ItemData.patern[x, y] != null)
                {
                    img.sprite = dragItemObj.GetComponent<ItemDragDrop>().ItemData.patern[x, y];
                    dragItemObj.GetComponent<RectTransform>().GetChild(num).GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, dragItemObj.GetComponent<ItemDragDrop>().ItemData.rotate);
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