using entreprise.venteAgnion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace interaction
{
    public class SetSiteInteraction : InteractionObject
    {
        [SerializeReference] private SiteVente site;
        [SerializeReference] private VenteUiSetContent venteUI;
        public void Start()
        {
            if (site == null)
            {
                Debug.Log($"{gameObject.name}: pas de site");
                Destroy(gameObject);
            }
            if (venteUI == null)
            {
                Debug.Log($"{gameObject.name}: pas de venteUi");
                Destroy(gameObject);
            }
        }

        public override void InteractionPlayer()
        {
            venteUI.Site = site;
        }
    }
}