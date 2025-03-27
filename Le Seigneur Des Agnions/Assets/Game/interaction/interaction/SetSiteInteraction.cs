using entreprise.venteAgnion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace interaction
{
    public class SetSiteInteraction : InteractionObject
    {
        [SerializeReference] private VenteUiSetContent venteUI;
        [SerializeReference] private SiteVente site;

        public override void InteractionPlayer()
        {
            venteUI.Site = site;
        }
    }
}