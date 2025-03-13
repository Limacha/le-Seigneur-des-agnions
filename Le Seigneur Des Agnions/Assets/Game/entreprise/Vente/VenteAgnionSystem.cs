using mob;
using UnityEngine;

namespace entreprise.venteAgnion
{
    public class VenteAgnionSystem : MonoBehaviour
    {
        [SerializeReference] private SiteVente[] siteVentes; //les lieux ou on peut ventre des agnions
        [SerializeField] private int demandeGlobal; //le nombre total d'agnion demande sur tout les site reunis
        [SerializeField] private int marketGlobalPrice; //le prix global sur le marcher

        [SerializeField] private float buyMultiplier = 10f;


        public SiteVente[] SiteVentes { get { return siteVentes; } }
        public int DemandeGlobal { get { return demandeGlobal; } }
        public int MarketGlobalPrice { get { return marketGlobalPrice; } }


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

        public void Buy(Agnion[] agnions, SiteVente siteVente, Entreprise entreprise)
        {

        }

        public void Sell(Agnion[] agnions, SiteVente siteVente, Entreprise entreprise)
        {

        }
    }
}