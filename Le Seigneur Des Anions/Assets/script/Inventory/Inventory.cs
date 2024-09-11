using UnityEngine;
using UnityEngine.UI;
using AllFonction;

public class Inventory : MonoBehaviour
{
    [SerializeField] private float slotHeight; //hauteur des slots
    [SerializeField] private float ySpacing; //espace entre les slots
    [SerializeField] private ItemData[,] content = new ItemData[4, 5]; //contenu de l'inventaire
    [SerializeField] private ItemData itemDataSprite; //itemdata qui designe que l'espace est occuper
    [SerializeField] private KeyBiding rotateKey; //keyBiding pour rotate

    [Header("element list")]
    [SerializeField] private GameObject inventoryPanel; //panel de l'inventaire
    [SerializeField] private GameObject inventoryColumnContent; //le containeur des column
    [SerializeField] private Sprite transImage; //image transparente
    [SerializeField] private GameObject RowContentPrefab; //prefab du conteneur des slot
    [SerializeField] private GameObject slotPrefab; //prefab des slot

    private readonly Fonction func = new Fonction();
    void Start()
    {
        //func.show2DItemDataContent(content);
        InitInventory();
        RefreshInventory();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
    }
    #region get info

    public ItemData[,] GetContent()
    {
        return content;
    }

    public ItemData GetItemDataSprite()
    {
        return itemDataSprite;
    }
    public Sprite GetTransImage()
    {
        return transImage;
    }

    public float GetYSpacing()
    {
        return ySpacing;
    }

    public float GetSlotHeight()
    {
        return slotHeight;
    }

    public float GetXSpacing()
    {
        return inventoryColumnContent.GetComponent<GridLayoutGroup>().spacing.x;
    }

    public float GetSlotWidth()
    {
        return inventoryColumnContent.GetComponent<GridLayoutGroup>().cellSize.x;
    }

    #endregion

    /// <summary>
    /// ajoute un item a l'inventaire
    /// </summary>
    /// <param name="item">item a ajouter</param>
    public void AddItem(ItemData item)
    {
        //Debug.Log(item);
        bool find = false;

        //parcourt l'inventaire
        for (int y = 0; y < content.GetLength(1) && !find; y++)
        {
            for(int x = 0; x < content.GetLength(0) && !find; x++)
            {
                //Debug.Log($"no item {content[x, y] == null} {x} {y}");
                //verifier si la case contient qq chose
                if (content[x, y] == null)
                {
                    //si l'emplacement du patern est libre
                    if (VerifPlace(item, x, y))
                    {
                        //Debug.Log("goodPlace");
                        find = PlaceItemInInventory(item, x, y);
                    }
                }
            }
        }
        RefreshInventory();
    }

    /// <summary>
    /// creez un item sur le panel
    /// </summary>
    /// <param name="item">item a ajouter</param>
    public GameObject CreateItem(ItemData item)
    {
        var itemCells = item.patern;
        GameObject goItem = new GameObject();
        goItem.name = $"dragItem{item.name}";
        var GLG = goItem.AddComponent<GridLayoutGroup>();
        goItem.AddComponent<CanvasGroup>();

        goItem.GetComponent<RectTransform>().SetParent(inventoryPanel.GetComponent<RectTransform>());
        goItem.GetComponent<RectTransform>().sizeDelta = new Vector2(item.patern.GetLength(0) * (GetSlotWidth() + GetXSpacing()), item.patern.GetLength(1) * (GetSlotHeight() + GetYSpacing()));
        goItem.AddComponent<ItemDragDrop>().SetItem(item);
        goItem.GetComponent<ItemDragDrop>().SetRotateKey(rotateKey);
        GLG.cellSize = new Vector2(GetSlotWidth(), GetSlotHeight());
        GLG.spacing = new Vector2(GetXSpacing(), GetYSpacing());
        for (int y = 0; y < item.patern.GetLength(1); y++)
        {
            for (int x = 0; x < item.patern.GetLength(0); x++)
            {
                var img = new GameObject().AddComponent<Image>();
                img.name = $"image{x} {y}";
                if (itemCells[x, y] != null)
                {
                    img.sprite = itemCells[x, y];
                }
                else
                {
                    img.raycastTarget = false;
                    img.sprite = transImage;
                }
                img.GetComponent<RectTransform>().SetParent(goItem.GetComponent<RectTransform>());
            }
        }
        return goItem;
    }

    /// <summary>
    /// retire un item a l'inventaire
    /// </summary>
    /// <param name="item">item a retirer</param>
    public void RemoveItem(ItemData item)
    {
        //debugInvContent();
        bool delete = false;
        int[] pos = GetPosInPatern(item.patern);

        //parcourt l'inventaire
        for (int y = 0; y < content.GetLength(1) && !delete; y++)
        {
            for (int x = 0; x < content.GetLength(0) && !delete; x++)
            {
                if (content[x, y] != null)
                {
                    //Debug.Log($"verif item {contentCells[y, x]} {x} {y}");
                    //verifier si la case est l'item
                    if (content[x, y].id == item.id)
                    {
                        RemoveItemFrom(item, x, y, pos);
                        delete = true;
                    }
                }
            }
        }
        RefreshInventory();
    }

    public void RemoveItemFrom(ItemData item , int x, int y, int[] pos)
    {
        //Debug.Log(item);
        //Debug.Log(item.patern.GridSize.x);
        //si oui alors parcour le patern
        for (int i = 0; i < item.patern.GetLength(0); i++)
        {
            for (int j = 0; j < item.patern.GetLength(1); j++)
            {
                //Debug.Log($"patern item here {itemCells[j, i]} {i} {j}");
                if (item.patern[i, j] != null)
                {
                    //Debug.Log($"del item {content.GetCell(x + i - pos[0], y + j - pos[1])} {x + i - pos[0]} {y + j - pos[1]}");
                    content[x + i - pos[0], y + j - pos[1]] = null;
                    inventoryColumnContent.GetComponent<RectTransform>().GetChild(x + i - pos[0]).GetChild(y + j - pos[1]).GetComponent<SlotDragDrop>().SetItem(null);
                }
            }
        }
    }

    /// <summary>
    /// ferme l'inventaire
    /// </summary>
    public void CloseInventory()
    {
        inventoryPanel.SetActive(false);
    }

    /// <summary>
    /// ouvre l'inventaire
    /// </summary>
    public void OpenInventory()
    {
        inventoryPanel.SetActive(true);
    }

    /// <summary>
    /// refresh l'affichage de l'inventaire
    /// </summary>
    public void RefreshInventory()
    {

        for (var y = 0; y < content.GetLength(1); y++)
        {
            for (var x = 0; x < content.GetLength(0); x++)
            {
                // si l'element est null alors affiche un sprite transparent
                if (content[x, y] != null)
                {
                    // si itemDataSprite alors on fait rien car l'affichage se fait autre par
                    if (content[x, y].id != itemDataSprite.id)
                    {
                        //Debug.Log(content[x, y]);
                        var patCells = content[x, y].patern;
                        var pos = GetPosInPatern(patCells);
                        //Debug.Log(pos[0] + " " + pos[1]);
                        if (pos[0] >= 0 && pos[1] >= 0)
                        {
                            // si bon affichage du patern de l'item
                            for (var i = 0; i < patCells.GetLength(0); i++)
                            {
                                for (var j = 0; j < patCells.GetLength(1); j++)
                                {
                                    if (patCells[i, j] != null)
                                    {
                                        //Debug.Log($"cell: x{x + i - pos[0]}: {x} + {i} - {pos[0]} y{y + j - pos[1]}: {y} + {j} - {pos[1]} : {patCells[j, i]}");
                                        inventoryColumnContent.transform.GetChild(x + i - pos[0]).GetChild(y + j - pos[1]).GetChild(0).GetComponent<Image>().sprite = patCells[i, j];
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    inventoryColumnContent.transform.GetChild(x).GetChild(y).GetChild(0).GetComponent<Image>().sprite = transImage;
                }
            }
        }
    }

    /// <summary>
    /// regarde si son inventaire est plein
    /// </summary>
    /// <returns>true si full</returns>
    public bool IsFull()
    {

        for (var y = 0; y < content.GetLength(1); y++)
        {
            for (var x = 0; x < content.GetLength(0); x++)
            {
                if (content[x, y] == null)
                {

                    return false;
                }
            }
        }
        return true;
    }

    /// <summary>
    /// debug le contenu de l'inventaire avec "item in {cells[y, x]}({x}, {y})"
    /// </summary>
    public void DebugInvContent()
    {

        for (var y = 0; y < content.GetLength(1); y++)
        {
            for (var x = 0; x < content.GetLength(0); x++)
            {
                Debug.Log($"item in {content[x, y]}({x}, {y})");
            }
        }
    }

    /// <summary>
    /// obtenir la position de l'item
    /// </summary>
    /// <param name="patern">le patern dans le quel trouver l'objet</param>
    /// <returns>list posX posY</returns>
    public int[] GetPosInPatern(Sprite[,] patern)
    {
        int[] pos = new int[2];
        pos[0] = -1;
        pos[1] = -1;
        for (var x = 0; x < patern.GetLength(0); x++)
        {
            for (var y = 0; y < patern.GetLength(1); y++)
            {
                //Debug.Log($"p {patern[y, x]} {x} {y}");
                if (patern[y, x] != null && patern[y, x] != itemDataSprite)
                {
                    pos[0] = x;
                    pos[1] = y;
                    //Debug.Log($"p return g {x} {y}");
                    return pos;
                }
            }
        }
        //Debug.Log($"p return b {pos[0]} {pos[1]}");
        return pos;
    }

    /// <summary>
    /// charge les slots de l'inventaire
    /// </summary>
    public void InitInventory()
    {
        inventoryColumnContent.GetComponent<GridLayoutGroup>().cellSize = new Vector2(slotHeight, content.GetLength(1) * (slotHeight + ySpacing) - ySpacing);
        for (var x = 0; x < content.GetLength(0); x++)
        {
            var row = Instantiate(RowContentPrefab, inventoryColumnContent.transform);
            for (var y = 0; y < content.GetLength(1); y++)
            {
                Instantiate(slotPrefab, row.transform);
            }
        }
    }

    /// <summary>
    /// place un item dans l'inventaire avec les possition x et y haut gauche en fonction du patern
    /// </summary>
    /// <param name="item">l'item a placer dans l'inventaire</param>
    /// <param name="x">la position en x</param>
    /// <param name="y">la position en y</param>
    /// <returns>true si il a reusit a le posser</returns>
    public bool PlaceItemInInventory(ItemData item, int x, int y)
    {
        int[] firstPos = new int[2];
        firstPos[0] = -1;
        firstPos[1] = -1;

        int[] pos = GetPosInPatern(item.patern);
        //parcour le patern
        for (int i = 0; i < item.patern.GetLength(0); i++)
        {
            for (int j = 0; j < item.patern.GetLength(1); j++)
            {
                //si le patern n'est pas vide
                if (item.patern[i, j] != null)
                {
                    //Debug.Log($"place {item.patern[i, j]} {x + i - pos[0]} {y + j - pos[1]}");
                    //si premier element deja placer ou non
                    if (firstPos[0] == -1 && firstPos[1] == -1)
                    {
                        var instItem = Instantiate(item);
                        inventoryColumnContent.GetComponent<RectTransform>().GetChild(x + i - pos[0]).GetChild(y + j - pos[1]).GetComponent<SlotDragDrop>().SetItem(instItem);
                        content[x + i - pos[0], y + j - pos[1]] = instItem;
                        //Debug.Log(item.patern);
                        if (item.patern == null)
                        {
                            content[x + i - pos[0], y + j - pos[1]].SetPatern();
                        } else
                        {
                            content[x + i - pos[0], y + j - pos[1]].patern = item.patern;
                        }
                        //Debug.Log(content[x + i - pos[0], y + j - pos[1]]);
                        //Debug.Log(content[x + i - pos[0], y + j - pos[1]].patern);
                        //Debug.Log($"add item {item} {x + i - pos[0]} {y + j - pos[1]}");
                        firstPos[0] = x + i - pos[0];
                        firstPos[1] = y + j - pos[1];
                        instItem.refX = firstPos[0];
                        instItem.refY = firstPos[1];
                    }
                    else
                    {
                        var instItem = Instantiate(itemDataSprite);
                        instItem.refX = firstPos[0];
                        instItem.refY = firstPos[1];
                        inventoryColumnContent.GetComponent<RectTransform>().GetChild(x + i - pos[0]).GetChild(y + j - pos[1]).GetComponent<SlotDragDrop>().SetItem(instItem);
                        content[x + i - pos[0], y + j - pos[1]] = instItem;
                        //Debug.Log($"add item {itemDataSprite} {x + i} {y + j}");
                    }
                }
            }
        }
        //Debug.Log("return");
        return true;
    }

    /// <summary>
    /// verifie si il y a de la place dans l'inventaire
    /// </summary>
    /// <param name="item">l'item a verifier la place</param>
    /// <param name="x">la position en x</param>
    /// <param name="y">la position en y</param>
    /// <returns>true si libre</returns>
    public bool VerifPlace(ItemData item, int x, int y)
    {
        int[] pos = GetPosInPatern(item.patern);

        for (int i = 0; i < item.patern.GetLength(0); i++)
        {
            for (int j = 0; j < item.patern.GetLength(1); j++)
            {
                //Debug.Log($"item {itemCells[j, i]} {i} {j}");
                //verif si la case patern est occuper
                if (item.patern[i, j] != null)
                {
                    //Debug.Log($"space {x + i - pos[0] < content.GridSize.x && y + j - pos[1] < content.GridSize.y && 0 <= x + i - pos[0] && 0 <= y + j - pos[1]} {x + i - pos[0]} {y + j - pos[1]} x{x} y{y} i{i} j{j} pos[0]{pos[0]} pos[1]{pos[1]}");
                    //verif si les coordone sont encore situer dans l'inventaire encore dans l'espace de l'inventaire
                    if (x + i - pos[0] < content.GetLength(0) && y + j - pos[1] < content.GetLength(1) && 0 <= x + i - pos[0] &&  0 <= y + j - pos[1])
                    {
                        //verif si l'inv est occuper a cette position
                        if (content[x + i - pos[0], y + j - pos[1]] != null)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }
}
