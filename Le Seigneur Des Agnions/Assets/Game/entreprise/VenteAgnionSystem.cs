using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace entreprise
{
    public class VenteAgnionSystem : MonoBehaviour
    {
        private SiteVente[] siteVentes; //les lieux ou on peut ventre des agnions
        private int demandeGlobal; //le nombre total d'agnion demande sur tout les site reunis
        private int marketGlobalPrice; //le prix global sur le marcher


        public SiteVente[] SiteVentes {  get { return siteVentes; } }
        public int DemandeGlobal {  get { return demandeGlobal; } }
        public int MarketGlobalPrice { get { return marketGlobalPrice; } }


    }
}