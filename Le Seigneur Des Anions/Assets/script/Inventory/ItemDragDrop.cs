using UnityEngine;
using UnityEngine.EventSystems;
using AllFonction;
using UnityEngine.UI;

public class ItemDragDrop : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private KeyBiding rotateKey; //key pour rotate les item
    [SerializeField] private ItemData itemData; //item drag
    [SerializeField] private Inventory inventory; //inventory

    private CanvasGroup canvasGroup; //canvas group pour gere les interaction
    private readonly Fonction func = new Fonction(); //fonctionpour debuger et rotate
    private bool drag;

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
                itemData.patern = func.rotate2DSprite(itemData.patern);
                refreshDragDropObj();
                //func.show2DSpriteContent(patern);
            }
        }
    }

    #region get info
    public ItemData GetItem()
    {
        return itemData;
    }
    #endregion

    #region set info
    public void SetItem(ItemData item)
    {
        itemData = item;
    }
    public void SetRotateKey(KeyBiding key)
    {
        rotateKey =  key;
    }
    #endregion

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

        GetComponent<RectTransform>().sizeDelta = new Vector2(itemData.patern.GetLength(0) * (inventory.GetSlotWidth() + inventory.GetXSpacing()), itemData.patern.GetLength(1) * (inventory.GetSlotHeight() + inventory.GetYSpacing()));
        GetComponent<ItemDragDrop>().SetItem(itemData);
        GetComponent<ItemDragDrop>().SetRotateKey(rotateKey);
        GLG.cellSize = new Vector2(inventory.GetSlotWidth(), inventory.GetSlotHeight());
        GLG.spacing = new Vector2(inventory.GetXSpacing(), inventory.GetYSpacing());
        for (int y = 0; y < itemData.patern.GetLength(1); y++)
        {
            for (int x = 0; x < itemData.patern.GetLength(0); x++)
            {
                var img = GetComponent<RectTransform>().GetChild(num).GetComponent<Image>();
                img.name = $"image{x} {y}";
                if (itemData.patern[x, y] != null)
                {
                    img.sprite = itemData.patern[x, y];
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
