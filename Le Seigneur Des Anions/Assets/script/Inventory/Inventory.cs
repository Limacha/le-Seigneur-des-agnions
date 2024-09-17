using UnityEngine;
using UnityEngine.UI;
using AllFonction;

public class Inventory : MonoBehaviour
{
    [Header("inventory panel atribut")]
    [SerializeField] private float slotHeight; //hauteur des slots
    [SerializeField] private float ySpacing; //espace entre les slots en hauteur
    [SerializeField] private float slotWidth; //largeur des slots
    [SerializeField] private float xSpacing; //espace entre les slots en largeur
    [SerializeField] private Vector3 invetoryPanelPosition; //position du panel de l'inventaire
    [SerializeField] private float inventoryPanelPadding; //espace entre le bort du panel et le contenu
    [SerializeField] private int contentWidth; //largeur du contenu
    [SerializeField] private int contentHeight; //hauteur du contenu
    [SerializeReference] private ItemData itemDataSprite; //itemdata qui designe que l'espace est occuper

    [Header("element list")]
    [SerializeReference] private GameObject inventoryPanel; //panel de l'inventaire
    [SerializeReference] private GameObject inventoryColumnContent; //le containeur des column
    [SerializeReference] private Sprite transImage; //image transparente
    [SerializeReference] private GameObject RowContentPrefab; //prefab du conteneur des slot
    [SerializeReference] private GameObject slotPrefab; //prefab des slot

    [Header("input")]
    [SerializeReference] private KeyBiding rotateKey; //keyBiding pour rotate


    private ItemData[,] content; //contenu de l'inventaire
    private readonly Fonction func = new Fonction(); //object fonction


    public void Awake()
    {
        content = new ItemData[contentWidth, contentHeight];
    }

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
        return xSpacing;
    }

    public float GetSlotWidth()
    {
        return slotWidth;
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
                //img.GetComponent<RectTransform>().transform.rotation = Quaternion.Euler(0, 0, content[x, y].rotate);
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

    /// <summary>
    /// remove un item depuis une position specific
    /// </summary>
    /// <param name="item">l'item a remove</param>
    /// <param name="x">la position de la case gauche de la matrice</param>
    /// <param name="y">la position de la case haute de la matrice</param>
    /// <param name="pos">la position de item dans la matrice</param>
    public void RemoveItemFrom(ItemData item , int x, int y, int[] pos)
    {
        //Debug.Log(item);
        //Debug.Log(item.patern.GridSize.x);
        //si oui alors parcour le patern
        item = content[x, y];
        for (int i = 0; i < item.patern.GetLength(0); i++)
        {
            for (int j = 0; j < item.patern.GetLength(1); j++)
            {
                //Debug.Log($"patern item here {itemCells[j, i]} {i} {j}");
                if (item.patern[i, j] != null)
                {
                    //Debug.Log($"del item {content.GetCell(x + i - pos[0], y + j - pos[1])} {x + i - pos[0]} {y + j - pos[1]}");
                    content[x + i - pos[0], y + j - pos[1]] = null;
                    inventoryColumnContent.GetComponent<RectTransform>().GetChild(x + i - pos[0]).GetChild(y + j - pos[1]).GetComponent<Slot>().SetItem(null);

                    //inventoryColumnContent.GetComponent<RectTransform>().GetChild(x + i - pos[0]).GetChild(y + j - pos[1]).GetComponent<Outline>().enabled = true;
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
                                        //Debug.Log($"{x} {y} {content[x, y].rotate}");
                                        
                                        inventoryColumnContent.transform.GetChild(x + i - pos[0]).GetChild(y + j - pos[1]).GetChild(0).GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, content[x, y].rotate);
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
        //Debug.Log(content.GetLength(0));
        //Debug.Log(slotWidth);
        //Debug.Log(inventoryPanelPadding * 2);
        inventoryPanel.GetComponent<RectTransform>().position = invetoryPanelPosition + GameObject.Find("Canvas").GetComponent<RectTransform>().position;
        inventoryPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(content.GetLength(0) * (slotWidth + xSpacing) - xSpacing + (inventoryPanelPadding * 2), content.GetLength(1) * (slotHeight + ySpacing) - ySpacing + (inventoryPanelPadding * 2));
        
        func.SetLeftRt(inventoryColumnContent.GetComponent<RectTransform>(), inventoryPanelPadding);
        func.SetRightRt(inventoryColumnContent.GetComponent<RectTransform>(), inventoryPanelPadding);
        func.SetTopRt(inventoryColumnContent.GetComponent<RectTransform>(), inventoryPanelPadding);
        func.SetBottomRt(inventoryColumnContent.GetComponent<RectTransform>(), inventoryPanelPadding);

        inventoryColumnContent.GetComponent<GridLayoutGroup>().spacing = new Vector2(xSpacing, 0);
        inventoryColumnContent.GetComponent<GridLayoutGroup>().cellSize = new Vector2(slotHeight, content.GetLength(1) * (slotHeight + ySpacing) - ySpacing);
        for (var x = 0; x < content.GetLength(0); x++)
        {
            var row = Instantiate(RowContentPrefab, inventoryColumnContent.transform);
            row.GetComponent<GridLayoutGroup>().spacing = new Vector2(0, ySpacing);
            row.GetComponent <GridLayoutGroup>().cellSize = new Vector2(slotWidth, slotHeight);
            for (var y = 0; y < content.GetLength(1); y++)
            {
                var slot = Instantiate(slotPrefab, row.transform);
                slot.GetComponent<RectTransform>().GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(slotWidth , slotHeight);
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
                        inventoryColumnContent.GetComponent<RectTransform>().GetChild(x + i - pos[0]).GetChild(y + j - pos[1]).GetComponent<Slot>().SetItem(instItem);
                        content[x + i - pos[0], y + j - pos[1]] = instItem;
                        //Debug.Log(item.patern);
                        if (item.patern == null)
                        {
                            content[x + i - pos[0], y + j - pos[1]].InitPatern();
                        } else
                        {
                            content[x + i - pos[0], y + j - pos[1]].patern = item.patern;
                        }
                        content[x + i - pos[0], y + j - pos[1]].SetRotate(item.rotate);
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
                        inventoryColumnContent.GetComponent<RectTransform>().GetChild(x + i - pos[0]).GetChild(y + j - pos[1]).GetComponent<Slot>().SetItem(instItem);
                        content[x + i - pos[0], y + j - pos[1]] = instItem;
                        //Debug.Log($"add item {itemDataSprite} {x + i} {y + j}");
                    }
                    //inventoryColumnContent.GetComponent<RectTransform>().GetChild(x + i - pos[0]).GetChild(y + j - pos[1]).GetComponent<Outline>().enabled = false;
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
