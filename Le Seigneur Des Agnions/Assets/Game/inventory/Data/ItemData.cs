using System;
using System.Collections;
using UnityEngine;
using AllFonction;

namespace inventory
{
    [CreateAssetMenu(fileName = "scriptabelObject/items data", menuName = "Items/New item")]
    [Serializable]
    public class ItemData : ScriptableObject
    {
        [Header("global item variable")]
        [SerializeField, ReadOnly] private string _id = Guid.NewGuid().ToString(); //id unique
        [SerializeField] private string nom; //nom de l'item
        [SerializeField] private string description; //description de l'item
        [SerializeField] private int width; //larguer de la matrice
        [SerializeField] private int height; //hauteur de la matrice
        [SerializeField] private Sprite imgRef; //image de referance
        [SerializeField] private Sprite[] listSprite; //liste des sprites a mettre dans le patern 0 = x0y0 / 2 = x0y2 / 6 = x2y0
        [SerializeField] private Sprite[,] patern; //paterne des sprites dans l'inventaire
        [SerializeField] private float poids = 0; //poids en gramme (plus tard)
        [SerializeField] private bool stackable = false; //si il peut se stack
        [SerializeField] private int stackLimit = 1; //nombre maxe d'object
        [SerializeField] private int stack = 1; //le nombre d'item qu'il y a
        [SerializeField, ReadOnly] private int refX; //reference a l'item principal si sprite itemData
        [SerializeField, ReadOnly] private int refY; //reference a l'item principal si sprite itemData
        [SerializeField] private bool canRotate = true; //si il peut tourner
        [SerializeField] private int rotate = 360; //degrer de rotation
        [SerializeField] private Restrict[] restriction = new Restrict[1] {Restrict.inventory}; //restriction des placement de l'item
        [SerializeReference] private GameObject prefab; //object a faire spawn si drop
        [SerializeReference] private string personnalData; //variable perso

        public string ID { get { return _id; } }
        public string Nom { get { return nom; } }
        public string Description { get { return description; } }
        public int Width { get { return width; } }
        public float Height { get { return height; } }
        public Sprite ImgRef { get { return imgRef; } }
        public Sprite[] ListSprite { get { return listSprite; } }
        public Sprite[,] Patern { get { return patern; } set { patern = value; } }
        public float Poids { get { return poids; } }
        public bool Stackable { get { return stackable; } }
        public int StackLimit { get { return stackLimit; } }
        public int Stack { get { return stack; } set { stack = value; } }
        public int RefX { get { return refX; } set { refX = value; } }
        public int RefY { get { return refY; } set { refY = value; } }
        public bool CanRotate { get { return CanRotate; } }
        public int Rotate { get { return rotate; } set { rotate = value; } }
        public string PersonalData { get { return personnalData; } set { personnalData = value; } }
        public Restrict[] Restriction { get { return restriction; } }

        public enum Restrict
        {
            inventory, //si peut etre dans l'inventaire
            haveHand, //si il doit avoir des main
            leftHand, //main gauche
            rightHand, //main droite
            leftAndRightHand, //les deux main
            leftOrRightHand //nimporte la quel
        }

        /// <summary>
        /// instancie l'item
        /// </summary>
        public void Init()
        {
            InitPatern();
        }

        /// <summary>
        /// renvoie toute les infos de l'items
        /// </summary>
        /// <returns>les infos</returns>
        public override string ToString()
        {
            // Construction de la liste des restrictions sous forme de string
            string restrictionsStr = restriction.Length > 0 ? string.Join(", ", restriction) : "Aucune";

            // Construction des sprites du pattern sous forme de tableau texte
            string paternStr = "";
            if (patern != null)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        paternStr += patern[x, y] != null ? "[X] " : "[ ] ";
                    }
                    paternStr += "\n"; // Nouvelle ligne pour la prochaine rangée
                }
            }
            else
            {
                paternStr = "Non défini";
            }
            return $"=============== ITEM INFOS ===============\n" +
           $"ID: {_id}\n" +
           $"Nom: {nom}\n" +
           $"Description: {description}\n" +
           $"Dimensions: {width}x{height}\n" +
           $"Poids: {poids}g\n" +
           $"Stackable: {stackable} (Max: {stackLimit}, Actuel: {stack})\n" +
           $"Peut tourner: {canRotate} (Angle: {rotate}°)\n" +
           $"Restrictions: {restrictionsStr}\n" +
           $"Données personnalisées: {personnalData}\n" +
           $"Prefab associé: {(prefab != null ? prefab.name : "Aucun")}\n" +
           $"Sprite de référence: {(imgRef != null ? imgRef.name : "Aucun")}\n" +
           $"Liste des sprites: {(listSprite != null && listSprite.Length > 0 ? listSprite.Length + " sprites définis" : "Aucun")}\n" +
           $"Pattern dans l'inventaire:\n{paternStr}" +
           $"\n==========================================";
        }

        /// <summary>
        /// instancie le tableau de sprite
        /// </summary>
        public void InitPatern()
        {
            patern = new Sprite[width, height];
            int pos = 0;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (pos < listSprite.Length)
                    {
                        patern[x, y] = listSprite[pos++];
                        //Debug.Log($"{nom} {listSprite[pos - 1]} pos{pos} y{y} x{x}");
                    }
                }
            }
        }

        /// <summary>
        /// rotate le patern de 90° vers la droite
        /// </summary>
        /// <returns>2Dsprite de la rotation finel</returns>
        public Sprite[,] rotatePatern()
        {
            if (canRotate)
            {
                patern = Fonction.rotate2DSprite(patern);
            }
            return patern;
        }

        public bool Drop(Vector3 position)
        {
            if (prefab)
            {
                for (int i = 0; i < stack; i++)
                {
                    Instantiate(prefab, position, new Quaternion(0, 0, 0, 0));
                }
                return true;
            }
            return false;
        }
    }
}