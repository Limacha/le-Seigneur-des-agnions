using entreprise;
using inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace craft
{
    public static class CraftSystem
    {
        /// <summary>
        /// verifie que la recherche requis est débloquer
        /// </summary>
        /// <param name="recipe">la recette qu'on veut craft</param>
        /// <param name="ent">l'entreprise qui contient les recherches</param>
        /// <returns>renvoie vrai si l'entreprise a la recheche ou qu'il n'y a pas de recheche requis</returns>
        private static bool VerifRecherche(CraftRecipe recipe, Entreprise ent = null)
        {
            if(recipe == null) return false; //si pas de recette
            if(recipe.rechercheRequire == null) return true; //si pas de recherche requis
            if (ent == null) return false; //si pas d'entreprise
            if (ent.Recherches.Contains(recipe.rechercheRequire)) return true; //si la recherche est déjà trouver
            return false;
        }

        /// <summary>
        /// verifier si on a tout les items
        /// </summary>
        /// <param name="recipe">la recette a craft</param>
        /// <param name="inv">l'inventaire du joueur</param>
        /// <returns>vrai si on a au minimum les items requis</returns>
        private static bool VerifItems(CraftRecipe recipe, Inventory inv = null)
        {
            if(recipe == null) return false; //si pas de recette
            if (recipe.itemsRequire == null) return true; //si pas d'item requis
            if (inv == null) return false; //si pas d'inventaire
            List<ItemData> itemsCheck = new List<ItemData>();//tous les items déjà vérifier
            //pour tout les items requis
            foreach (ItemData item in recipe.itemsRequire)
            {
                //si l'item n'a pas déjà été check
                if (itemsCheck.Count((verif) => { return verif.ID == item.ID; }) == 0)
                {
                    //si le nombre d'items requis est > au nombre d'items en possetion
                    if(recipe.itemsRequire.Count((verif) => { return verif.ID == item.ID; }) > inv.AllItemsInInv().Count((verif) => { return verif.ID == item.ID; }))
                    {
                        return false;
                    }
                    itemsCheck.Add(item);
                }
            }
            return true;
        }

        /// <summary>
        /// craft un items
        /// </summary>
        /// <param name="recipe">la recette du craft</param>
        /// <param name="inv">l'inventaire dans le quel l'ajouter</param>
        /// <param name="ent">l'entreprise avec les recherches</param>
        public static bool CraftItem(CraftRecipe recipe, Inventory inv, Entreprise ent)
        {
            //si pas de recette ou d'inventaire
            if (recipe == null) return false;
            if (inv == null) return false;

            //verifie la recherche
            if (VerifRecherche(recipe, ent))
            {
                //verifie si il y a tous les items requis
                if (VerifItems(recipe, inv))
                {
                    //supprime les items
                    foreach (ItemData item in recipe.itemsRequire)
                    {
                        if (!inv.RemoveItem(item)) { Debug.LogError("cheat not allow get out!!!"); return false; }
                    }

                    //ajoute tous les items a cree
                    foreach (ItemData result in recipe.result)
                    {
                        //essaye d'ajouter l'item
                        if (!inv.AddItem(result))
                        {
                            //creez une instance de l'item
                            ItemData item = GameObject.Instantiate(result);
                            //drop l'instance
                            inv.DropIfNotNull(ref item);
                        }
                    }
                    return true;
                }
            }
            return false;
        }
    }
}
