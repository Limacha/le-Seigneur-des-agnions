using inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace entreprise.recherche
{
    public static class RechercheSystem
    {

        /// <summary>
        /// verifie que tout les recherches requis sont possedez
        /// </summary>
        /// <param name="recherche">la recherche qu'on veut débloquer</param>
        /// <param name="entreprise">l'entreprise du joueur</param>
        /// <returns>si on a toute les recherche</returns>
        private static bool VerifRecherchesRestric(Recherche recherche, Entreprise entreprise)
        {
            if (entreprise != null)
            {
                if (entreprise.Recherches.Count(rechercheComplet => { return rechercheComplet.ID == recherche.ID; }) == 0)
                {
                    foreach (Recherche rechercheRequire in recherche.recherchesRequire)
                    {
                        if (entreprise.Recherches.Count(rechercheComplet => { return rechercheComplet.ID == rechercheRequire.ID; }) == 0)
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// verifie que tout les items sont posseder
        /// </summary>
        /// <param name="recherche">la recherche qu'on veut debloquer</param>
        /// <param name="inventory">l'inventaire du joueur</param>
        /// <returns>si on a tout les items</returns>
        private static bool VerifItemsRestric(Recherche recherche, Inventory inventory)
        {
            foreach (ItemData itemRequire in recherche.itemsRequire)
            {
                if (inventory.AllItemsInInv().Count(itemHave => { return itemHave.ID == itemRequire.ID; }) == 0)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// débloque une recherche si il peut
        /// </summary>
        /// <param name="recherche">la recherche a débloquer</param>
        public static bool UnlockRecherche(Recherche recherche, Inventory inv, Entreprise ent)
        {
            if (inv && ent)
            {
                if (VerifRecherchesRestric(recherche, ent))
                {
                    if (VerifItemsRestric(recherche, inv))
                    {
                        //supprime les items
                        foreach (ItemData item in recherche.itemsRequire)
                        {
                            if (!inv.RemoveItem(item)) { Debug.LogError("cheat not allow get out!!!"); return false; }
                        }

                        ent.Recherches.Add(recherche);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}