using UnityEngine;
using UnityEngine.UI;
using AllFonction;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using static UnityEditor.Progress;
using UnityEngine.InputSystem;
using Unity.Jobs;


//optimiser avec chatGpt
namespace inventory
{
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
        [SerializeField] private ItemData leftHandItem; //mainGauche
        [SerializeField] private ItemData rightHandItem; //main droite
        [SerializeField] private ItemData twoHandsItem; //si il a quelque chose dans les deux mains
        [SerializeField] private HandsData handsItem; //les mains qu'il a

        [SerializeReference] private ItemData itemDataSprite; //itemdata qui designe que l'espace est occuper

        [Header("element list")]
        [SerializeReference] private GameObject canvas; //panel de l'inventaire
        [SerializeReference] private GameObject inventoryPanel; //panel de l'inventaire
        [SerializeReference] private GameObject inventoryColumnContent; //le containeur des column
        [SerializeReference] private Sprite transImage; //image transparente
        [SerializeReference] private GameObject rowContentPrefab; //prefab du conteneur des slot
        [SerializeReference] private GameObject slotPrefab; //prefab des slot
        [SerializeReference] private GameObject textPoids; //text qui affiche le poids
        [SerializeReference, ReadOnly] private GameObject gameManager; //game manager
        [SerializeReference] private GameObject toolTip; //toolTip
        [SerializeReference, ReadOnly] private SlotDragDrop slotDrag; //l'item drag
        [SerializeReference] private Transform leftHandTransform; //l'item drag
        [SerializeReference] private Transform rightHandTransform; //l'item drag
        [SerializeReference] private Transform twoHandTransform; //l'item drag
        [SerializeReference] private Image leftHandImage; //l'item drag
        [SerializeReference] private Image rightHandImage; //l'item drag
        [SerializeReference] private Image twoHandImage; //l'item drag
        [SerializeReference] private Image handsImage; //l'item drag

        private float poids = 0; //poid de l'inventaire
        private ItemData[,] content; //contenu de l'inventaire

        [Header("animations")]
        [SerializeField] private string openTriggerSac = "openTriggerSac"; // Animation ouverture du sac
        [SerializeField] private string closeTriggerSac = "closeTriggerSac"; // Animation ouverture du sac
        [SerializeReference] private Animator AnimatorPerso; // animator de l'animation du sac
        [SerializeReference] private GameObject personnage; // Référence au Panel du menu - doit être assigné dans l'éditeur
        [SerializeField] private bool animationSacEnCours = false;

        #region proprieter
        public float SlotHeight { get { return slotHeight; } }
        public float YSpacing { get { return ySpacing; } }
        public float SlotWidth { get { return slotWidth; } }
        public float XSpacing { get { return xSpacing; } }
        public int ContentWidth { get { return contentWidth; } }
        public int ContentHeight { get { return contentHeight; } }
        public ItemData LeftHandItem { get { return leftHandItem; } set { leftHandItem = value; } }
        public ItemData RightHandItem { get { return rightHandItem; } set { rightHandItem = value; } }
        public ItemData TwoHandsItem { get { return twoHandsItem; } set { twoHandsItem = value; } }
        public HandsData HandsItem { get { return handsItem; } }
        public ItemData[,] Content { get { return content; } }
        public ItemData ItemDataSprite { get { return itemDataSprite; } }
        public GameObject InventoryPanel { get { return inventoryPanel; } }
        public GameObject RowContentPrefab { get { return rowContentPrefab; } }
        public GameObject SlotContentPrefab { get { return SlotContentPrefab; } }
        public GameObject ItemContentPrefab { get { return ItemContentPrefab; } }
        public Sprite TransImage { get { return transImage; } }
        public float Poids { get { return poids; } }

        public GameObject ToolTip { get { return toolTip; } }
        public SlotDragDrop SlotDrag { get { return slotDrag; } set { slotDrag = value; } }

        public bool AnimationSacEnCours { get { return animationSacEnCours; } }
        #endregion

        public enum HandPosition
        {
            Left, //main gauche
            Right, //main droite
            Both, //les deux mains
            Hand //se sont des mains
        }

        public void Awake()
        {
            content = new ItemData[contentWidth, contentHeight];
            gameManager = GameObject.Find("GameManager");
        }

        void Start()
        {
            //init l'inventaire et l'affiche
            InitInventory();
            RefreshInventory();
        }

        /*
        /// <summary>
        /// savegarde cet inventaire
        /// </summary>
        public void SaveInventory()
        {
            SaveSystem.SaveInventory(this);
        }

        /// <summary>
        /// charge l'inventaire depuis le fichier de save
        /// </summary>
        public void LoadInventory()
        {
            InventorySaveData inventorySaveData = SaveSystem.LoadInventory(this);
            ResetInventory(); //reset de l'inventaire
            if (inventorySaveData != null) //si sa a charger qq chose
            {
                for (int i = 0; i < inventorySaveData.itemSaveDatas.Length; i++) //parcour le tableau
                {
                    if (inventorySaveData.itemSaveDatas[i] != null) //si l'item n'est pas null
                    {
                        ItemData defItem = null;
                        for (int j = 0; j < gameManager.GetComponent<ItemDataManager>().ItemList.Count; j++) //parcour les items charger
                        {
                            if (inventorySaveData.itemSaveDatas[i].id == gameManager.GetComponent<ItemDataManager>().ItemList[j].ID) //si les id son les meme
                            {
                                //creation d'un item par default
                                defItem = gameManager.GetComponent<ItemDataManager>().ItemList[j];
                                defItem.Stack = inventorySaveData.itemSaveDatas[i].stack;
                                //choix si un type particulier
                                if (inventorySaveData.itemSaveDatas[i].GetType() == typeof(RessourceSaveData))
                                {
                                    //ajout des variable special
                                    RessourceData item = defItem as RessourceData;
                                    item.Source = ((RessourceSaveData)inventorySaveData.itemSaveDatas[i]).Source;

                                    //ajout de l'item
                                    PlaceItemInInventory(item, inventorySaveData.itemSaveDatas[i].refX, inventorySaveData.itemSaveDatas[i].refY);
                                }
                                else if (inventorySaveData.itemSaveDatas[i].GetType() == typeof(RessourceSaveData))
                                {
                                    //ajout des variable special
                                    ConsomableData item = defItem as ConsomableData;
                                    item.Source = ((ConsomableSaveData)inventorySaveData.itemSaveDatas[i]).source;

                                    //ajout de l'item
                                    PlaceItemInInventory(item, inventorySaveData.itemSaveDatas[i].refX, inventorySaveData.itemSaveDatas[i].refY);
                                }
                                else
                                {
                                    //ajout de l'item
                                    PlaceItemInInventory(defItem, inventorySaveData.itemSaveDatas[i].refX, inventorySaveData.itemSaveDatas[i].refY);
                                }
                            }
                        }
                    }
                }
            }
            RefreshInventory(); //affiche l'inventaire
        }*/


        /// <summary>
        /// Initialise l'inventaire : setup UI, placement des slots, gestion taille/padding.
        /// </summary>
        public void InitInventory()
        {
            // Récupération des composants nécessaires
            CanvasScaler canvasScaler = canvas.GetComponent<CanvasScaler>();
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            RectTransform inventoryRect = inventoryPanel.GetComponent<RectTransform>();
            RectTransform poidsRect = textPoids.GetComponent<RectTransform>();
            TMP_Text poidsText = textPoids.GetComponent<TMP_Text>();
            RectTransform contentRect = inventoryColumnContent.GetComponent<RectTransform>();
            GridLayoutGroup columnLayout = inventoryColumnContent.GetComponent<GridLayoutGroup>();

            int width = content.GetLength(0);
            int height = content.GetLength(1);
            float ratioX = Screen.width / canvasScaler.referenceResolution.x;

            // --- Positionnement de l'inventaire ---
            Vector3 basePosition = invetoryPanelPosition + canvasRect.position;
            inventoryRect.position = basePosition;

            twoHandImage.transform.parent.position = new Vector3(slotWidth * ratioX, Screen.height - (slotHeight * ratioX + 5), 0);
            rightHandImage.transform.parent.position = new Vector3(slotWidth * ratioX, Screen.height - 2 * (slotHeight * ratioX + 5), 0);
            leftHandImage.transform.parent.position = new Vector3(slotWidth * ratioX, Screen.height- 3 * (slotHeight * ratioX + 5), 0);
            handsImage.transform.parent.position = new Vector3(slotWidth * ratioX, Screen.height- 4 * (slotHeight * ratioX + 5), 0);

            // --- Positionnement et style du texte poids ---
            float textOffsetY = (inventoryRect.rect.height / 2 + inventoryPanelPadding / 4) * ratioX;
            poidsRect.position = basePosition - new Vector3(0, textOffsetY, 0);
            poidsRect.sizeDelta = new Vector2(width * (slotWidth + xSpacing) - xSpacing, inventoryPanelPadding / 2);
            poidsText.margin = Vector4.zero;
            poidsText.fontSize = inventoryPanelPadding / 2;

            // --- Taille du panel ---
            float panelWidth = width * (slotWidth + xSpacing) - xSpacing + (inventoryPanelPadding * 2);
            float panelHeight = height * (slotHeight + ySpacing) - ySpacing + (inventoryPanelPadding * 2);
            inventoryRect.sizeDelta = new Vector2(panelWidth, panelHeight);

            // --- Padding autour du contenu ---
            Fonction.SetLeftRt(contentRect, inventoryPanelPadding);
            Fonction.SetRightRt(contentRect, inventoryPanelPadding);
            Fonction.SetTopRt(contentRect, inventoryPanelPadding);
            Fonction.SetBottomRt(contentRect, inventoryPanelPadding);

            // --- Layout vertical des colonnes ---
            columnLayout.spacing = new Vector2(xSpacing, 0);
            columnLayout.cellSize = new Vector2(slotHeight, height * (slotHeight + ySpacing) - ySpacing);

            // --- Création des slots (colonnes et lignes) ---
            for (int x = 0; x < width; x++)
            {
                GameObject row = Instantiate(RowContentPrefab, inventoryColumnContent.transform);
                GridLayoutGroup rowLayout = row.GetComponent<GridLayoutGroup>();
                rowLayout.spacing = new Vector2(0, ySpacing);
                rowLayout.cellSize = new Vector2(slotWidth, slotHeight);

                for (int y = 0; y < height; y++)
                {
                    Instantiate(slotPrefab, row.transform);
                }
            }
        }

        #region ajout un item

        /// <summary>
        /// ajoute un item a l'inventaire
        /// </summary>
        /// <param name="item">item a ajouter</param>
        public bool AddItem(ItemData item)
        {
            // Essayer de stacker l'item si c'est possible
            bool itemAdded = TryStackItem(item);

            // Si l'item n'a pas pu être stacké, essayer de le placer dans l'inventaire
            if (!itemAdded)
            {
                itemAdded = TryPlaceItem(item);
            }

            // Rafraîchir l'affichage de l'inventaire
            RefreshInventory();

            return itemAdded;
        }

        /// <summary>
        /// essaye de stacker l'item
        /// </summary>
        /// <param name="item">l'item a stacker</param>
        /// <returns>si on a put le stack</returns>
        private bool TryStackItem(ItemData item)
        {
            if (!item.Stackable) return false;
            // Parcourt toutes les cellules de l'inventaire pour essayer de stacker l'item
            for (int y = 0; y < content.GetLength(1); y++)
            {
                for (int x = 0; x < content.GetLength(0); x++)
                {
                    if (content[x, y] != null && content[x, y].ID == item.ID)
                    {
                        // Si la pile actuelle est inférieure à la limite de pile, empiler l'item
                        if (content[x, y].Stack < content[x, y].StackLimit)
                        {
                            content[x, y].Stack += item.Stack;
                            return true;  // L'item a été ajouté avec succès en stackant
                        }
                    }
                }
            }

            // Si l'item n'a pas été stacké, retourner false
            return false;
        }

        /// <summary>
        /// essaye d'jouter l'item si il y a de la place
        /// </summary>
        /// <param name="item">l'item a ajouter</param>
        /// <returns>si on a put l'ajouter</returns>
        private bool TryPlaceItem(ItemData item)
        {
            // Parcourt toutes les cellules de l'inventaire pour trouver un emplacement vide
            for (int y = 0; y < content.GetLength(1); y++)
            {
                for (int x = 0; x < content.GetLength(0); x++)
                {
                    // Si l'emplacement est vide et que l'item peut être placé ici
                    if (content[x, y] == null && VerifPlace(item, x, y))
                    {
                        // Si un emplacement valide est trouvé, placer l'item
                        return PlaceItemInInventory(item, x, y);
                    }
                }
            }

            // Si aucune place n'a été trouvée, retourner false
            return false;
        }

        /// <summary>
        /// place un item dans l'inventaire avec les possition x et y haut gauche + la position dans le patern
        /// </summary>
        /// <param name="item">l'item a placer dans l'inventaire</param>
        /// <param name="x">la position en x en fonction du patern</param>
        /// <param name="y">la position en y en fonction du patern</param>
        /// <returns>true si il a reusit a le posser</returns>
        public bool PlaceItemInInventory(ItemData item, int x, int y)
        {
            int[] firstPos = { -1, -1 };

            int[] pos = GetPosInPatern(item.Patern);
            //parcour le patern
            for (int i = 0; i < item.Patern.GetLength(0); i++)
            {
                for (int j = 0; j < item.Patern.GetLength(1); j++)
                {
                    if (item.Patern[i, j] == null) continue;

                    int gridX = x + i - pos[0];
                    int gridY = y + j - pos[1];

                    // Accès au slot UI
                    Transform slotTransform = inventoryColumnContent.GetComponent<RectTransform>().GetChild(gridX).GetChild(gridY);
                    Slot slot = slotTransform.GetComponent<Slot>();

                    ItemData instItem;
                    //si premier element deja placer ou non
                    if (firstPos[0] == -1 && firstPos[1] == -1)
                    {
                        instItem = Instantiate(item); //instancie l'objet

                        //si pas de patern alors on l'init
                        if (item.Patern == null)
                            item.InitPatern();
                        // Définir le pattern et la rotation
                        instItem.Patern = item.Patern;

                        instItem.Rotate = item.Rotate;

                        // Position de référence
                        firstPos[0] = gridX;
                        firstPos[1] = gridY;
                        instItem.RefX = gridX;
                        instItem.RefY = gridY;
                    }
                    else
                    {
                        // Autres cellules = clones visuels (liés à la ref)
                        instItem = Instantiate(itemDataSprite);
                        instItem.RefX = firstPos[0];
                        instItem.RefY = firstPos[1];
                    }

                    slot.ItemData = instItem;
                    content[gridX, gridY] = instItem;

                    //desactivation des bordure
                    //inventoryColumnContent.GetComponent<RectTransform>().GetChild(x + i - pos[0]).GetChild(y + j - pos[1]).GetComponent<Outline>().enabled = false;
                    SetBorder(true, slotTransform.gameObject, item, i, j);

                }
            }
            return true;
        }

        #endregion

        #region create drag item

        /// <summary>
        /// cree un gameObject pour pouvoir le deplacer dans l'inventaire
        /// </summary>
        /// <param name="item">l'item a cree le gameobject</param>
        /// <returns>le gameObject creer</returns>
        public GameObject CreateItem(ItemData item)
        {
            // Créer le GameObject principal pour l'item
            GameObject goItem = CreateItemObject(item);

            // Configure les propriétés du GridLayoutGroup
            ConfigureGridLayout(ref goItem, item);

            // Créer les cellules de l'item
            CreateItemCells(ref goItem, item);

            return goItem;
        }

        /// <summary>
        /// genere le gameObject sans aucune config
        /// </summary>
        /// <param name="item">l'item qui sert a genere</param>
        /// <returns>le gameobject cree</returns>
        private GameObject CreateItemObject(ItemData item)
        {
            // Crée un GameObject pour l'item avec un nom dynamique
            GameObject goItem = new GameObject($"dragItem{item.name}");

            goItem.AddComponent<RectTransform>();

            // Ajout des composants nécessaires au GameObject
            goItem.AddComponent<CanvasGroup>();
            goItem.AddComponent<ItemDragDrop>().ItemData = item;

            // Retourner l'objet nouvellement créé
            return goItem;
        }

        /// <summary>
        /// configure le game object (taille, layout)
        /// </summary>
        /// <param name="goItem">le gameobject a configurer</param>
        /// <param name="item">l'item originel</param>
        private void ConfigureGridLayout(ref GameObject goItem, ItemData item)
        {
            // Récupère le RectTransform du GameObject
            var rectTransform = goItem.GetComponent<RectTransform>();
            rectTransform.SetParent(inventoryPanel.GetComponent<RectTransform>());
            rectTransform.sizeDelta = new Vector2(item.Patern.GetLength(0) * (SlotWidth + XSpacing), item.Patern.GetLength(1) * (SlotHeight + YSpacing));
            rectTransform.localScale = Vector3.one;

            // Configuration du GridLayoutGroup
            var GLG = goItem.AddComponent<GridLayoutGroup>();
            GLG.cellSize = new Vector2(SlotWidth, SlotHeight);
            GLG.spacing = new Vector2(XSpacing, YSpacing);
        }

        /// <summary>
        /// ajoute toute les celule de l'item pour l'afficher
        /// </summary>
        /// <param name="goItem">le gameObject qui affiche l'item</param>
        /// <param name="item">l'item originel</param>
        private void CreateItemCells(ref GameObject goItem, ItemData item)
        {
            var itemCells = item.Patern;

            // Parcours du pattern pour créer chaque cellule
            for (int y = 0; y < item.Patern.GetLength(1); y++)
            {
                for (int x = 0; x < item.Patern.GetLength(0); x++)
                {
                    CreateCell(ref goItem, x, y, itemCells[x, y]);
                }
            }
        }

        /// <summary>
        /// cree une cellule qui sera afficher
        /// </summary>
        /// <param name="goItem">le gameobject ou ajouter la cellule</param>
        /// <param name="x">la position x de la cellule</param>
        /// <param name="y">la position y de la cellule</param>
        /// <param name="patternSprite">le sprite afficher</param>
        private void CreateCell(ref GameObject goItem, int x, int y, Sprite patternSprite)
        {
            // Créer un GameObject pour chaque cellule
            var img = new GameObject($"image{x} {y}").AddComponent<Image>();

            // Si le pattern a un sprite, on l'affecte, sinon on met une image transparente
            img.sprite = patternSprite ?? transImage;
            img.raycastTarget = patternSprite != null;  // Désactive le raycast si le pattern est vide

            // Configure le RectTransform de l'image
            var imgRectTransform = img.GetComponent<RectTransform>();
            imgRectTransform.SetParent(goItem.GetComponent<RectTransform>());
            imgRectTransform.localScale = Vector3.one;
        }

        #endregion

        #region remove item

        /// <summary>
        /// retire un item a l'inventaire
        /// </summary>
        /// <param name="item">item a retirer</param>
        public bool RemoveItem(ItemData item)
        {
            bool delete = false;

            // Si l'item est stackable, on essaie de réduire le stack au lieu de le supprimer.
            if (item.Stackable)
            {
                // Recherche un item stackable avec le même ID dans l'inventaire.
                delete = ReduceItemStack(item);
            }

            // Si l'item n'est pas stackable ou si le stack a été réduit, on le retire complètement.
            if (!delete)
            {
                delete = RemoveItemFromInventory(item);
            }

            // Rafraîchit l'inventaire après modification.
            RefreshInventory();

            return delete;  // Retourne si l'item a été supprimé ou non.
        }

        /// <summary>
        /// reduit l'item d'un stack si existe deja
        /// </summary>
        /// <param name="item">l'item a enlever</param>
        /// <returns>renvoie si l'item a ete enlever</returns>
        private bool ReduceItemStack(ItemData item)
        {
            for (int y = 0; y < content.GetLength(1); y++)
            {
                for (int x = 0; x < content.GetLength(0); x++)
                {
                    var currentItem = content[x, y];
                    if (currentItem != null && currentItem.ID == item.ID)
                    {
                        // Si le stack est supérieur à 1, on le réduit.
                        if (currentItem.Stack > 1)
                        {
                            currentItem.Stack -= item.Stack;
                            return true;  // Item réduit, on sort de la fonction.
                        }
                    }
                }
            }
            return false;  // Retourne false si l'item n'a pas été trouvé ou si le stack ne peut pas être réduit.
        }

        /// <summary>
        /// enleve l'item a une position
        /// </summary>
        /// <param name="item">l'item a enlever</param>
        /// <returns>si on a reussit</returns>
        private bool RemoveItemFromInventory(ItemData item)
        {
            int[] pos = GetPosInPatern(item.Patern);  // Récupère la position du pattern de l'item.
            for (int y = 0; y < content.GetLength(1); y++)
            {
                for (int x = 0; x < content.GetLength(0); x++)
                {
                    var currentItem = content[x, y];
                    if (currentItem != null && currentItem.ID == item.ID)
                    {
                        RemoveItemFrom(item, x, y, pos);  // Retire l'item de l'inventaire.
                        return true;  // Item supprimé, retourne true.
                    }
                }
            }
            return false;  // Retourne false si l'item n'a pas été trouvé dans l'inventaire.
        }

        /// <summary>
        /// remove un item depuis une position specific
        /// </summary>
        /// <param name="item">l'item a remove</param>
        /// <param name="x">la position de la case gauche de la matrice</param>
        /// <param name="y">la position de la case haute de la matrice</param>
        /// <param name="pos">la position de item dans la matrice</param>
        public void RemoveItemFrom(ItemData item, int x, int y, int[] pos)
        {
            item = content[x, y]; // On récupère l'item principal via ses coordonnées

            Sprite[,] pattern = item.Patern;
            if (pattern == null) return;

            int patternWidth = pattern.GetLength(0);
            int patternHeight = pattern.GetLength(1);

            for (int i = 0; i < patternWidth; i++)
            {
                for (int j = 0; j < patternHeight; j++)
                {
                    if (pattern[i, j] == null) continue;

                    int targetX = x + i - pos[0];
                    int targetY = y + j - pos[1];

                    content[targetX, targetY] = null;

                    var slot = inventoryColumnContent
                        .GetComponent<RectTransform>()
                        .GetChild(targetX)
                        .GetChild(targetY)
                        .GetComponent<Slot>();

                    slot.ItemData = null;

                    // Réactive les bordures
                    SetBorder(
                        show: false,
                        slot: slot.gameObject,
                        item: item,
                        i: i,
                        j: j
                    );
                }
            }
        }

        #endregion

        #region open/close inventory
        /// <summary>
        /// ouvre ou ferme l'inventaire
        /// </summary>
        public void OpenCloseInventory()
        {
            if (inventoryPanel.activeSelf)
            {
                CloseInventory();
            }
            else
            {
                OpenInventory();
            }
        }

        /// <summary>
        /// ferme l'inventaire
        /// </summary>
        public void CloseInventory()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            toolTip.SetActive(false);
            animationSacEnCours = true;
            StartCoroutine(ToggleCloseAnimeSac());
        }
        IEnumerator ToggleCloseAnimeSac()
        {
            RectTransform invPanelRT = inventoryPanel.GetComponent<RectTransform>();
            for (int i = 1; i >= 0 && i < invPanelRT.childCount; i++)
            {
                if (invPanelRT.GetChild(i).name.Substring(0, 8) == "dragItem")
                {
                    Destroy(invPanelRT.GetChild(i).gameObject);
                }
            }
            inventoryPanel.SetActive(false);
            AnimatorPerso.SetTrigger(closeTriggerSac);
            yield return new WaitForSeconds(3); // Attendre que l'animation se termine
            animationSacEnCours = false;
            Player player = transform.parent.gameObject.GetComponent<Player>();
            player.CanLookAround = true;
            player.CanMouve = true;
            player.CanInteract = true;
        }

        /// <summary>
        /// ouvre l'inventaire
        /// </summary>
        public void OpenInventory()
        {
            Player player = transform.parent.gameObject.GetComponent<Player>();
            player.CanLookAround = false;
            player.CanMouve = false;
            player.CanInteract = false;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            StartCoroutine(ToggleOpenAnimeSac());
        }
        IEnumerator ToggleOpenAnimeSac()
        {
            inventoryPanel.SetActive(false);
            AnimatorPerso.SetTrigger(openTriggerSac);
            yield return new WaitForSeconds(3); // Attendre que l'animation se termine
            inventoryPanel.SetActive(true);
            animationSacEnCours = false;
        }
        #endregion

        /// <summary>
        /// refresh l'affichage de l'inventaire
        /// </summary>
        public void RefreshInventory()
        {
            poids = 0;

            int width = content.GetLength(0);
            int height = content.GetLength(1);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var item = content[x, y];

                    // Si le slot est vide, on affiche une image transparente
                    if (item == null)
                    {
                        SetSlotSprite(x, y, transImage, 0);
                        continue;
                    }

                    // On ignore les sprites d'élément "fantôme"
                    if (item.ID == itemDataSprite.ID)
                        continue;

                    // Calcul du poids
                    poids += item.Poids * item.Stack;

                    var patern = item.Patern;
                    var refPos = GetPosInPatern(patern);

                    if (refPos[0] < 0 || refPos[1] < 0)
                        continue; // Patern invalide

                    for (int i = 0; i < patern.GetLength(0); i++)
                    {
                        for (int j = 0; j < patern.GetLength(1); j++)
                        {
                            Sprite sprite = patern[i, j];
                            if (sprite != null)
                            {
                                int px = x + i - refPos[0];
                                int py = y + j - refPos[1];
                                SetSlotSprite(px, py, sprite, item.Rotate);
                            }
                        }
                    }
                }
            }
            if (leftHandItem != null && leftHandImage)
            {
                leftHandImage.sprite = (leftHandItem.ImgRef) ? leftHandItem.ImgRef : transImage;
            }
            if (rightHandItem != null && rightHandItem)
            {
                rightHandImage.sprite = (rightHandItem.ImgRef) ? rightHandItem.ImgRef : transImage; 
            }
            if (twoHandsItem != null && twoHandImage)
            {
                twoHandImage.sprite = (twoHandsItem.ImgRef) ? twoHandsItem.ImgRef : transImage;
            }
            if (handsItem != null && handsImage)
            {
                handsImage.sprite = (handsItem.ImgRef) ? handsItem.ImgRef : transImage;
            }

            textPoids.GetComponent<TMP_Text>().SetText($"poids: {poids}");
        }

        /// <summary>
        /// defini le sprite d'un slot
        /// </summary>
        /// <param name="x">la position x</param>
        /// <param name="y">la position y</param>
        /// <param name="sprite">le sprite a afficher</param>
        /// <param name="rotation">la rotation du sprite</param>
        private void SetSlotSprite(int x, int y, Sprite sprite, float rotation)
        {
            var image = inventoryColumnContent
                .transform.GetChild(x)
                .GetChild(y)
                .GetChild(0)
                .GetComponent<Image>();

            var rt = image.GetComponent<RectTransform>();

            image.sprite = sprite;
            rt.eulerAngles = new Vector3(0, 0, rotation);
        }

        /// <summary>
        /// debug le contenu de l'inventaire avec "item in {cells[y, x]}({x}, {y})"
        /// </summary>
        public void DebugInvContent()
        {
            //parcour l'inventaire
            for (var y = 0; y < content.GetLength(1); y++)
            {
                for (var x = 0; x < content.GetLength(0); x++)
                {
                    Debug.Log($"item in {content[x, y]}({x}, {y})"); //affiche le contenu dans le debug
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
            if (patern == null)
            {
                Debug.LogWarning("Patern null passé à GetPosInPatern.");
                return new int[] { -1, -1 };
            }

            for (int x = 0; x < patern.GetLength(0); x++)
            {
                for (int y = 0; y < patern.GetLength(1); y++)
                {
                    Sprite sprite = patern[x, y];
                    if (sprite != null && sprite != itemDataSprite)
                    {
                        return new int[] { x, y };
                    }
                }
            }

            Debug.Log("Aucune case remplie trouvée dans le patern.");
            return new int[] { -1, -1 };
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

            int[] pivot = GetPosInPatern(item.Patern); // Position de référence dans le pattern

            int patternHeight = item.Patern.GetLength(0);
            int patternWidth = item.Patern.GetLength(1);
            int gridWidth = content.GetLength(0);
            int gridHeight = content.GetLength(1);
            // Parcours du pattern de l'item
            for (int i = 0; i < patternHeight; i++)
            {
                for (int j = 0; j < patternWidth; j++)
                {
                    // Si une cellule du pattern est occupée
                    if (item.Patern[i, j] != null)
                    {
                        int gridX = x + i - pivot[0];
                        int gridY = y + j - pivot[1];

                        // Vérifie que la cellule est dans les limites du tableau
                        if (gridX < 0 || gridX >= gridWidth || gridY < 0 || gridY >= gridHeight)
                        {
                            return false;
                        }

                        // Vérifie que la cellule est libre
                        if (content[gridX, gridY] != null)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// reset l'inventaire a 0
        /// </summary>
        public void ResetInventory()
        {
            //parcour l'inventaire
            for (int i = 0; i < contentWidth; i++)
            {
                for (int j = 0; j < contentHeight; j++)
                {
                    if (content[i, j] != null) //si plein
                    {
                        RemoveItemFrom(content[i, j], content[i, j].RefX, content[i, j].RefY, GetPosInPatern(content[i, j].Patern));//retire le contenu
                    }
                }
            }
        }

        /// <summary>
        /// gere les bordure des case
        /// </summary>
        /// <param name="show">l'etat des bordure actuel</param>
        /// <param name="slot">le slot concerner</param>
        /// <param name="item">l'item contenu</param>
        /// <param name="i">co x</param>
        /// <param name="j">co y</param>
        public void SetBorder(bool show, GameObject slot, ItemData item, int i, int j)
        {
            if (slot == null) return;

            // Accès plus direct au container des bordures
            Transform borders = slot.transform.GetChild(1);

            // Indexs des enfants : 0 = haut, 1 = gauche, 2 = bas, 3 = droite
            GameObject top = borders.GetChild(0).gameObject;
            GameObject left = borders.GetChild(1).gameObject;
            GameObject bottom = borders.GetChild(2).gameObject;
            GameObject right = borders.GetChild(3).gameObject;

            if (!show)
            {
                top.SetActive(true);
                left.SetActive(true);
                bottom.SetActive(true);
                right.SetActive(true);
                return;
            }

            if (item.Patern != null && item.Patern[i, j] != null)
            {
                // Bas (i + 1)
                if (i + 1 < item.Patern.GetLength(0) && item.Patern[i + 1, j] != null)
                    bottom.SetActive(false);

                // Haut (i - 1)
                if (i - 1 >= 0 && item.Patern[i - 1, j] != null)
                    top.SetActive(false);

                // Droite (j + 1)
                if (j + 1 < item.Patern.GetLength(1) && item.Patern[i, j + 1] != null)
                    right.SetActive(false);

                // Gauche (j - 1)
                if (j - 1 >= 0 && item.Patern[i, j - 1] != null)
                    left.SetActive(false);
            }
        }

        /// <summary>
        /// obtient un tableau de tout les items dans l'inventaire
        /// </summary>
        /// <returns>le tableau de tout les items</returns>
        public ItemData[] AllItemsInInv()
        {
            List<ItemData> itemslist = new List<ItemData>();
            for (int i = 0; i < content.GetLength(0); i++)
            {
                for (int j = 0; j < content.GetLength(1); j++)
                {
                    if(content[i, j] != null && content[i, j].ID != itemDataSprite.ID)
                    {
                        itemslist.Add(Instantiate(content[i, j]));
                    }
                }
            }
            return itemslist.ToArray();
        }

        #region find item
        /// <summary>
        /// permet de trouver un item avec son nom
        /// </summary>
        /// <param name="name">nom du game object</param>
        /// <returns>une instance de l'item trouver</returns>
        public ItemData FindItemWhitName(string name)
        {
            foreach (ItemData item in gameManager.GetComponent<ItemDataManager>().Items)
            {
                if (item.name == name)
                {
                    var instItem = Instantiate(item);
                    instItem.Init();
                    return instItem;
                }
            }
            return null;
        }

        /// <summary>
        /// permet de trouver un item avec son id
        /// </summary>
        /// <param name="id">id de l'item</param>
        /// <returns>une instance de l'item toruver</returns>
        public ItemData FindItemWhitId(string id)
        {
            foreach (ItemData item in gameManager.GetComponent<ItemDataManager>().Items)
            {
                if (item.ID == id)
                {
                    var instItem = Instantiate(item);
                    instItem.Init();
                    return instItem;
                }
            }
            return null;
        }

        #endregion

        #region equip item
        /// <summary>
        /// equip un item
        /// </summary>
        /// <param name="item">l'item a equiper</param>
        /// <param name="fromInv">si il viens de l'inventaire</param>
        /// <param name="handPosition">la main a equiper</param>
        /// <param name="forcer">si on force et drop l'object en main</param>
        /// <returns>si l'equipement a reussit</returns>
        public bool EquipItem(ItemData item, bool fromInv, HandPosition handPosition, bool forcer)
        {
            if (fromInv)
            {
                //equip en main
                bool result = EquipItem(item, false, handPosition, forcer);
                return result;
            }
            EquipableData equipItem = item as EquipableData;
            Transform newItemTrans;
            switch (handPosition)
            {
                case HandPosition.Left:
                    if (!forcer && (leftHandItem != null || twoHandsItem != null)) return false;
                    DropIfNotNull(ref leftHandItem);
                    DropIfNotNull(ref twoHandsItem);
                    leftHandItem = Instantiate(item);
                    if(equipItem && equipItem.OneHandPrefab)
                    {
                        newItemTrans = Instantiate(equipItem.OneHandPrefab).transform;
                        newItemTrans.parent = leftHandTransform;
                        newItemTrans.position = leftHandTransform.position;
                        newItemTrans.localScale = Vector3.one;
                    }

                    return true;

                case HandPosition.Right:
                    if (!forcer && (rightHandItem != null || twoHandsItem != null)) return false;
                    DropIfNotNull(ref rightHandItem);
                    DropIfNotNull(ref twoHandsItem);
                    rightHandItem = Instantiate(item);
                    if (equipItem && equipItem.OneHandPrefab)
                    {
                        newItemTrans = Instantiate(equipItem.OneHandPrefab).transform;
                        newItemTrans.parent = rightHandTransform;
                        newItemTrans.position = rightHandTransform.position;
                        newItemTrans.localScale = Vector3.one;
                    }
                    return true;

                case HandPosition.Both:
                    if (!forcer && (leftHandItem != null || rightHandItem != null || twoHandsItem != null)) return false;
                    DropIfNotNull(ref twoHandsItem);
                    DropIfNotNull(ref leftHandItem);
                    DropIfNotNull(ref rightHandItem);
                    twoHandsItem = Instantiate(item);
                    if (equipItem && equipItem.TwoHandPrefab)
                    {
                        newItemTrans = Instantiate(equipItem.TwoHandPrefab).transform;
                        newItemTrans.parent = twoHandTransform;
                        newItemTrans.position = twoHandTransform.position;
                        newItemTrans.localScale = Vector3.one;
                    }
                    return true;

                case HandPosition.Hand:
                    if (item is HandsData hands)
                    {
                        ItemData hand = this.handsItem;
                        DropIfNotNull(ref hand);
                        this.handsItem = null;

                        this.handsItem = Instantiate(hands);
                        if (hands.LeftHandPrefab)
                        {
                            newItemTrans = Instantiate(hands.LeftHandPrefab).transform;
                            newItemTrans.parent = leftHandTransform;
                            newItemTrans.position = leftHandTransform.position;
                            newItemTrans.localScale = Vector3.one;
                        }
                        if (hands.RightHandPrefab)
                        {
                            newItemTrans = Instantiate(hands.RightHandPrefab).transform;
                            newItemTrans.parent = rightHandTransform;
                            newItemTrans.position = rightHandTransform.position;
                            newItemTrans.localScale = Vector3.one;
                        }
                        RefreshInventory();
                        return true;
                    }
                    return false;
            }

            return false;
        }

        /// <summary>
        /// Équipe un objet automatiquement selon ses restrictions
        /// </summary>
        public bool EquipItem(ItemData item, bool fromInv)
        {
            item.Stack = 1;
            bool equipped = false;
            bool needHand = false;
            int restric = 0;

            // Première tentative : sans forcer
            while (restric < item.Restriction.Length && !equipped)
            {
                if (item.Restriction[restric] == ItemData.Restrict.haveHand) needHand = true;
                equipped = needHand && HandsItem == null ? false : ChooseHand(item, fromInv, restric, false);
                restric++;
            }

            // Deuxième tentative : en forçant (si pas encore équipé)
            restric = 0;
            while (!equipped && restric < item.Restriction.Length)
            {
                if (item.Restriction[restric] == ItemData.Restrict.haveHand) needHand = true;
                equipped = needHand && HandsItem == null ? false : ChooseHand(item, fromInv, restric, true);
                restric++;
            }

            return equipped;
        }

        /// <summary>
        /// choisit la main et essaye d'equipper
        /// </summary>
        /// <param name="item">l'item a equiper</param>
        /// <param name="fromInv">si il viens de l'inventaire</param>
        /// <param name="restric">le numero de la restriciton actuel</param>
        /// <param name="forcer">si on force ou pas</param>
        /// <returns>si on a put equiper</returns>
        public bool ChooseHand(ItemData item, bool fromInv, int restric, bool forcer)
        {
            var restriction = item.Restriction[restric];

            switch (restriction)
            {
                case ItemData.Restrict.leftAndRightHand:
                    return EquipItem(item, fromInv, HandPosition.Both, forcer);

                case ItemData.Restrict.leftHand:
                    return EquipItem(item, fromInv, HandPosition.Left, forcer);

                case ItemData.Restrict.rightHand:
                    return EquipItem(item, fromInv, HandPosition.Right, forcer);

                case ItemData.Restrict.leftOrRightHand:
                    if (RightHandItem == null)
                        return EquipItem(item, fromInv, HandPosition.Right, forcer);
                    else if (LeftHandItem == null)
                    {
                        return EquipItem(item, fromInv, HandPosition.Left, forcer);
                    }
                    else
                        return EquipItem(item, fromInv, HandPosition.Right, forcer);
            }
            //si pas equip autre par alors essaye de l'equip en main
            return EquipItem(item, fromInv, HandPosition.Hand, forcer);
        }

        #endregion

        #region drop

        /// <summary>
        /// drop tout les items en mains
        /// </summary>
        public void DropAll()
        {
            DropIfNotNull(ref leftHandItem);
            DropIfNotNull(ref rightHandItem);
            DropIfNotNull(ref twoHandsItem);
        }

        /// <summary>
        /// drop l'item si s'est pas null
        /// </summary>
        /// <param name="handItem">la variable avec l'item a drop et qui sera defini comme null</param>
        public void DropIfNotNull(ref ItemData handItem)
        {
            if (handItem != null)
            {
                handItem.Drop(transform.position);
                handItem = null;
            }
        }

        #endregion
    }
}