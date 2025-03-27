using mob;
using UnityEngine;

namespace entreprise.venteAgnion
{
    public class VenteAgnionSystem : MonoBehaviour
    {
        [SerializeReference, ReadOnly] private Entreprise entreprise; //l'entreprise du joueur
        [SerializeReference] private SiteVente[] siteVentes; //les lieux ou on peut ventre des agnions
        [SerializeField] private int demandeGlobal; //le nombre total d'agnion demande sur tout les site reunis
        [SerializeField] private int marketGlobalPrice; //le prix global sur le marcher

        [SerializeField] private float buyMultiplier = 10f; //l'inflation du prix lors de l'achat
        [SerializeField, ReadOnly] private int moyen = 0; //la moyen de la qualiter vendu
        [SerializeField, ReadOnly] private int nAgnionVendu = 0;//le nombre d'agnion qui est vendu

        [Header("temporaire")]
        [SerializeField, Range(0, 360)] private int rotation;//l'heure qu'il est


        public SiteVente[] SiteVentes { get { return siteVentes; } }
        public int DemandeGlobal { get { return demandeGlobal; } }
        public int MarketGlobalPrice { get { return marketGlobalPrice; } }

        public void Start()
        {
            entreprise = gameObject.GetComponent<Entreprise>();
            if (entreprise == null)
            {
                Debug.LogError("pas d'entreprise ici");
            }
        }

        private void FixedUpdate()
        {
            if (rotation <= 130 && rotation >= 125)
            {
                if (moyen / nAgnionVendu <= 5)
                {
                    entreprise.ChangerReput(entreprise.Reputation - 1);
                }
                else
                {
                    entreprise.ChangerReput(entreprise.Reputation + 1);
                }
            }
        }

        public float CalcSellPrice(Agnion[] agnions, SiteVente siteVente)
        {
            float prix = 0f;
            foreach (Agnion agnion in agnions)
            {
                prix += marketGlobalPrice * agnion.Quality;
            }
            return prix * siteVente.Ratio;
        }

        public float CalcBuyPrice(Agnion[] agnions, SiteVente siteVente)
        {
            float prix = 0f;
            foreach (Agnion agnion in agnions)
            {
                prix += marketGlobalPrice * agnion.Quality;
            }
            return prix * siteVente.Ratio * buyMultiplier;
        }

        public bool Buy(Agnion[] agnions, SiteVente siteVente)
        {
            float prix = -(CalcBuyPrice(agnions, siteVente));
            if (entreprise.Argent >= -prix)
            {
                entreprise.Transaction(prix);
                return true;
            }
            return false;
        }

        public bool Sell(Agnion[] agnions, SiteVente siteVente)
        {
            if (siteVente.Demand > 0)
            {
                float prix = CalcSellPrice(agnions, siteVente);
                entreprise.Transaction(prix);
                foreach (Agnion agnion in agnions)
                {
                    nAgnionVendu++;
                    moyen += agnion.Quality;
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}