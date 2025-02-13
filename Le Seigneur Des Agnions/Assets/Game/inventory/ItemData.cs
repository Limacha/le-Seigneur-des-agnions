using System;
using System.Collections;
using UnityEngine;

namespace inventory
{
    [CreateAssetMenu(fileName = "scriptabelObject/items data", menuName = "Items/New item")]
    public class ItemData : ScriptableObject
    {
        [Header("global item variable")]
        [SerializeField, ReadOnly] private string id = Guid.NewGuid().ToString(); //id unique
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
        [SerializeField] private int rotate = 360; //degrer de rotation
        [SerializeField] private Restrict[] restriction; //restriction des placement de l'item

        [SerializeReference] private GameObject prefab; //l'item en 3D

        public string ID { get { return id; } }
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
        public int Rotate { get { return rotate; } set { rotate = value; } }
        public GameObject Prefab { get { return prefab; } }

        private enum Restrict
        {

            inventory,
            haveHand,
            leftHand,
            rightHand,
            leftAndRightHand,
            leftOrRightHand
        }

        /// <summary>
        /// instancie l'item
        /// </summary>
        public void Init()
        {
            InitPatern();
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
            Sprite[,] newArray = new Sprite[patern.GetLength(1), patern.GetLength(0)]; //creation du nouveau tableau
            //parcour du vieux tableau
            for (var y = 0; y < patern.GetLength(1); y++)
            {
                for (var x = 0; x < patern.GetLength(0); x++)
                {
                    //Debug.Log($"x{x}y{y} => x{array.GetLength(1) - 1 - y}y{x}");
                    newArray[patern.GetLength(1) - 1 - y, x] = patern[x, y];
                }
            }
            patern = newArray;
            return patern;
        }
    }
}