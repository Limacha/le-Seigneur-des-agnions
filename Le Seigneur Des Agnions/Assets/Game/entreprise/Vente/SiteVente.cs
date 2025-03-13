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
        [SerializeField, Range(1, 9)] private int qualityMin; //borne des qualiter possible
        [SerializeField, Range(2, 10)] private int qualityMax; //borne des qualiter possible
        [SerializeField, Range(0, 100)] private int proprety; //la propreter du lieux
        [SerializeField, Range(0, 100)] private int celebrity; //la celebriter total
        [SerializeReference] private GameObject agnions; //les agnions qu'ils est possible d'achetter sur place

        [SerializeReference] private VenteAgnionSystem sellSystem; //durer avant de pouvoir recall Ini

        [SerializeField, ReadOnly] private int demand; //le nombre d'agnion demande dans le commerce
        [SerializeField, ReadOnly] private float ratio; //le ratio d'achat en fonctiion du prix global sur le marche
        [SerializeField, ReadOnly] private int reloadStock; //durer avant de pouvoir recall les fonction de stocks
        [SerializeField, ReadOnly] private int reloadInit; //durer avant de pouvoir recall Init

        [Header("variable")]
        [SerializeField] private float reloadStockRatio;//combient on reload par seconde
        [SerializeField] private float reloadInitRatio;//combient on reload par seconde

        [Header("temporaire")]
        [SerializeField, Range(0, 360)] private int rotation;//l'heure qu'il est

        public int QualityMin { get { return qualityMin; } }
        public int QualityMax { get { return qualityMax; } }
        public int Proprety { get { return proprety; } }
        public int Celebrity { get { return celebrity; } }

        public int Demand { get { return demand; } }
        public float Ratio { get { return Ratio; } }
        public GameObject Agnion { get { return agnions; } }
        public VenteAgnionSystem SellSystem { get { return sellSystem; } }


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
        }

        /// <summary>
        /// instancie tout les parametre du site quand le joueur rentre dedans
        /// </summary>
        /// <param name="venteAgnionSystem">le system de vente global</param>
        private void Init(VenteAgnionSystem venteAgnionSystem)
        {
            //verifier que le system global le connais
            if (!venteAgnionSystem.SiteVentes.Contains(this))
            {
                Destroy(gameObject);
            }

            int celebTotal = 0;
            foreach (SiteVente site in venteAgnionSystem.SiteVentes)
            {
                celebTotal += site.Celebrity;
            }
            if (celebTotal > 0 && celebrity > 0)
            {
                demand = (int)(venteAgnionSystem.DemandeGlobal * (celebrity / celebTotal) * Random.Range(0.9f, 1.1f));
                ratio = ((float)demand / (float)celebrity);
                reloadInit = (int)(50 * reloadInitRatio);
            }
        }

        public void FixedUpdate()
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
                    Init(sellSystem);
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
                for (int i = 0; i < Random.Range(0, agnions.GetComponents<Agnion>().Length/2); i++)
                {
                    //permet de degrader au pif
                    agnions.GetComponents<Agnion>()[Random.Range(0, agnions.GetComponents<Agnion>().Length)].Degrader();
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
                //si on modifier la demande d'agnions
                if ((int)Random.Range(0, celebrity) > 10)
                {
                    SupOrAdd();
                }
                return true;
            }
            else
            {
                /*
                //verifier si il y a un soleil
                GameObject sun = GameObject.Find("Sun");
                if (sun)
                {
                    Debug.Log(sun.transform);*/
                //verifier l'heure
                if (rotation >= 120 && rotation <= 130)
                {
                    for (int i = 0; i < celebrity; i++)
                    {
                        SupOrAdd();
                    }
                    return true;
                }
                //}
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
                if (agnions.GetComponents<Agnion>().Length > 0)
                {
                    //agnions.GetComponentAtIndex(Random.Range(0, agnions.GetComponents<Agnion>().Length))
                    Destroy(agnions.GetComponents<Agnion>()[Random.Range(0, agnions.GetComponents<Agnion>().Length)]);
                }
                return 0;
            }
            else
            {
                Agnion agnion = agnions.AddComponent<Agnion>();
                agnion.Quality = Random.Range(qualityMin, qualityMax + 1);

                return 1;
            }
        }
    }
}