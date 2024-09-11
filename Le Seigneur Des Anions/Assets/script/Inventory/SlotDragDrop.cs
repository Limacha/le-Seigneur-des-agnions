using AllFonction;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotDragDrop : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private KeyBiding rotateKey; //key pour rotate les item
    [SerializeField] private ItemData itemData; //item drag
    [SerializeField] private Inventory inventory; //inventory

    private CanvasGroup canvasGroup; //canvas group pour gere les interaction
    private readonly Fonction func = new Fonction(); //fonctionpour debuger et rotate
    private GameObject dragItemObj;
    private bool drag = false;
    private bool dragItem = false;

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
                    //func.show2DSpriteContent(dragItemObj.GetComponent<ItemDragDrop>().GetItem().patern);
                    dragItemObj.GetComponent<ItemDragDrop>().GetItem().patern = func.rotate2DSprite(dragItemObj.GetComponent<ItemDragDrop>().GetItem().patern);
                    refreshDragDropObj();
                    //func.show2DSpriteContent(patern);
                }
            }
        }
    }

    #region get info
    public ItemData GetItem()
    {
        return itemData;
    }
    public GameObject GetGameObject()
    {
        return dragItemObj;
    }
    #endregion

    #region set info
    public void SetItem(ItemData item)
    {
        itemData = item;
    }
    public void SetRotateKey(KeyBiding key)
    {
        rotateKey = key;
    }
    #endregion

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("OnBeginDrag");
        drag = true;
        canvasGroup.alpha = .6f; //transparense de l'image
        canvasGroup.blocksRaycasts = false; //active OnDrop sur itemSlot

        //Debug.Log(itemData);
        if (itemData != null)
        {
            if (itemData.id == inventory.GetItemDataSprite().id)
            {
                itemData = inventory.GetContent()[itemData.refX, itemData.refY];
            }
            //Debug.Log(itemData.refX + " " + itemData.refY);
            if (itemData != null)
            {
                //Debug.Log(itemData);
                dragItem = true;
                dragItemObj = inventory.CreateItem(itemData);
                dragItemObj.GetComponent<ItemDragDrop>().SetItem(itemData);

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

        dragItemObj.GetComponent<RectTransform>().sizeDelta = new Vector2(dragItemObj.GetComponent<ItemDragDrop>().GetItem().patern.GetLength(0) * (inventory.GetSlotWidth() + inventory.GetXSpacing()), dragItemObj.GetComponent<ItemDragDrop>().GetItem().patern.GetLength(1) * (inventory.GetSlotHeight() + inventory.GetYSpacing()));
        dragItemObj.GetComponent<ItemDragDrop>().SetItem(dragItemObj.GetComponent<ItemDragDrop>().GetItem());
        dragItemObj.GetComponent<ItemDragDrop>().SetRotateKey(rotateKey);
        GLG.cellSize = new Vector2(inventory.GetSlotWidth(), inventory.GetSlotHeight());
        GLG.spacing = new Vector2(inventory.GetXSpacing(), inventory.GetYSpacing());
        for (int y = 0; y < dragItemObj.GetComponent<ItemDragDrop>().GetItem().patern.GetLength(1); y++)
        {
            for (int x = 0; x < dragItemObj.GetComponent<ItemDragDrop>().GetItem().patern.GetLength(0); x++)
            {
                var img = this.dragItemObj.GetComponent<RectTransform>().GetChild(num).GetComponent<Image>();
                img.name = $"image{x} {y}";
                if (dragItemObj.GetComponent<ItemDragDrop>().GetItem().patern[x, y] != null)
                {
                    img.sprite = dragItemObj.GetComponent<ItemDragDrop>().GetItem().patern[x, y];
                }
                else
                {
                    img.raycastTarget = false;
                    img.sprite = inventory.GetTransImage();
                }
                num++;
            }
        }
    }
}