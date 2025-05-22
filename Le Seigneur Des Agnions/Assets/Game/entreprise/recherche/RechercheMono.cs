using entreprise;
using inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace entreprise.recherche
{
    public class RechercheMono : MonoBehaviour
    {
        [SerializeReference] Recherche recherche;
        [SerializeReference, ReadOnly] Inventory inv;
        [SerializeReference, ReadOnly] Entreprise ent;
        [SerializeReference, ReadOnly] Button button;

        public void Start()
        {
            if(!TryGetComponent(out button))
            {
                Destroy(gameObject);
            }
            GameObject.FindWithTag("inventory").TryGetComponent(out inv);
            GameObject.FindWithTag("Entreprise").TryGetComponent(out ent);
            button.onClick.AddListener(CraftAction);
        }

        void CraftAction()
        {
            RechercheSystem.UnlockRecherche(recherche, inv, ent);
        }
    }
}