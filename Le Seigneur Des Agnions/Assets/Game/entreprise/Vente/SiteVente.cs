using mob; //agnion
using System.Collections.Generic; //liste
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition; //monobehaviour

namespace entreprise.venteAgnion
{
    public class SiteVente : MonoBehaviour
    {
        [SerializeField] private string nom; //nom du site
        [SerializeField, Range(1, 9)] private int qualityMin; //borne des qualiter possible
        [SerializeField, Range(2, 10)] private int qualityMax; //borne des qualiter possible
        [SerializeField, Range(0, 100)] private int proprety; //la propreter du lieux
        [SerializeField, Range(0, 100)] private int celebrity; //la celebriter total
        [SerializeReference] private GameObject buyConteneur; //les agnions qu'ils est possible d'achetter sur place
        [SerializeReference] private GameObject sellConteneur; //les agnions qu'ils est possible de vendre

        [SerializeReference] private VenteAgnionSystem sellSystem; //durer avant de pouvoir recall Ini

        [SerializeField, ReadOnly] private int demand; //le nombre d'agnion demande dans le commerce
        [SerializeField, ReadOnly] private float ratio; //le ratio d'achat en fonctiion du prix global sur le marche
        [SerializeField, ReadOnly] private int reloadStock; //durer avant de pouvoir recall les fonction de stocks
        [SerializeField, ReadOnly] private int reloadInit; //durer avant de pouvoir recall Init
        [SerializeField] private bool change = true; //durer avant de pouvoir recall Init

        [SerializeReference] private Transform spawnPoint; //la position de spawn des agnions

        [Header("variable")]
        [SerializeField] private float reloadStockRatio;//combient on reload par seconde
        [SerializeField] private float reloadInitRatio;//combient on reload par seconde

        [Header("temporaire")]
        [SerializeField, Range(0, 360)] private int rotation;//l'heure qu'il est

        public string Nom { get { return nom; } }
        public int QualityMin { get { return qualityMin; } }
        public int QualityMax { get { return qualityMax; } }
        public int Proprety { get { return proprety; } }
        public int Celebrity { get { return celebrity; } }

        public int Demand { get { return demand; } }
        public float Ratio { get { return ratio; } }
        public GameObject BuyConteneur { get { return buyConteneur; } }
        public GameObject SellConteneur { get { return sellConteneur; } }
        public VenteAgnionSystem SellSystem { get { return sellSystem; } }
        public bool Change { get { return change; } set { change = value; } }
        public Transform SpawnPoint { get { return spawnPoint; } set { spawnPoint = value; } }


        public void Start()
        {
            //verifie les bornes
            if (qualityMin < 1)
            {
                qualityMin  = 1;
            }
            else if (qualityMin > 9)
            {
                qualityMin = 9;
            }

            if (qualityMax < 2)
            {
                qualityMax = 2;
            }
            else if (qualityMax > 10)
            {
                qualityMax = 10;
            }

            if (celebrity < 0)
            {
                celebrity = 0;
            }
            else if (celebrity > 100)
            {
                celebrity = 100;
            }

            if (proprety < 0)
            {
                proprety = 0;
            }
            else if (proprety > 100)
            {
                proprety = 100;
            }
            Init();
        }

        /// <summary>
        /// instancie tout les parametre du site quand le joueur rentre dedans
        /// </summary>
        /// <param name="venteAgnionSystem">le system de vente global</param>
        public void Init()
        {
            //verifier que le system global le connais
            if(sellSystem == null)
            {
                Debug.Log($"{gameObject.name}: pas de sell system");
                Destroy(gameObject);
            }

            if (!sellSystem.SiteVentes.Contains(this))
            {
                Debug.Log($"{gameObject.name}: site non conu du system");
                Destroy(gameObject);
            }

            int celebTotal = 0;
            foreach (SiteVente site in sellSystem.SiteVentes)
            {
                celebTotal += site.Celebrity;
            }
            if (celebTotal > 0 && celebrity > 0)
            {
                demand = (int)(sellSystem.DemandeGlobal * (float)((float)celebrity / (float)celebTotal) * Random.Range(0.9f, 1.1f));
                ratio = ((float)demand / (float)celebrity);
            }
            else
            {
                demand = 1;
                ratio = 0.5f;
            }
            reloadInit = (int)(50 * reloadInitRatio);
        }

        public void FixedUpdate()
        {
            if (change)
            {
                //gere les chronometre
                if (reloadInit > 0)
                {
                    reloadInit--;
                }
                if (reloadStock > 0)
                {
                    reloadStock--;
                }
                //reload tout les x secondes
                else if (reloadStock == 0)
                {
                    UpdateStock();
                    DegradeStock();
                    reloadStock = (int)(50 * reloadStockRatio);
                }
            }
        }

        /// <summary>
        /// quand qq rentre dans la boutique
        /// </summary>
        /// <param name="other">l'object qui est rentre</param>
        private void OnTriggerEnter(Collider other)
        {
            //verifi si s'est le player
            if (other.gameObject.tag == "Player")
            {
                //init si il peut
                if (reloadInit <= 0)
                {
                    Init();
                }
            }
        }

        /// <summary>
        /// degrade des agnions en fonction de la  propreter du lieux
        /// </summary>
        /// <returns>si il y a eux une degradation</returns>
        private bool DegradeStock()
        {
            if ((int)Random.Range(0, proprety) < 10)
            {
                //degrade un nombre au pif
                for (int i = 0; i < Random.Range(0, buyConteneur.GetComponents<Agnion>().Length/2); i++)
                {
                    //permet de degrader au pif
                    buyConteneur.GetComponents<Agnion>()[Random.Range(0, buyConteneur.GetComponents<Agnion>().Length)].Degrader();
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// met a jour le stock d'agnion
        /// </summary>
        /// <returns>si il y a un hcangement ou pas</returns>
        private bool UpdateStock()
        {
            if (celebrity > 10)
            {
                if ((int)Random.Range(0, celebrity) > 10)
                {
                    SupOrAdd();
                }
                return true;
            }
            else
            {
                //verifier l'heure
                if (rotation >= 120 && rotation <= 130)
                {
                    for (int i = 0; i < celebrity; i++)
                    {
                        SupOrAdd();
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// suprime ou ajoute un agnion
        /// </summary>
        /// <returns>0: sup, 1: ajout</returns>
        private int SupOrAdd()
        {
            //si on ajoute ou retire
            if ((int)Random.Range(0, 4) == 0)
            {
                if (buyConteneur.GetComponents<Agnion>().Length > 0)
                {
                    //agnions.GetComponentAtIndex(Random.Range(0, agnions.GetComponents<Agnion>().Length))
                    Destroy(buyConteneur.GetComponents<Agnion>()[Random.Range(0, buyConteneur.GetComponents<Agnion>().Length)]);
                }
                return 0;
            }
            else
            {
                Agnion agnion = buyConteneur.AddComponent<Agnion>();
                agnion.Quality = (byte)Random.Range(qualityMin, qualityMax + 1);

                return 1;
            }
        }
    
        public void AddSellAgnion(byte quality)
        {
            Agnion agnion = sellConteneur.AddComponent<Agnion>();
            agnion.Quality = quality;
        }
    }
}