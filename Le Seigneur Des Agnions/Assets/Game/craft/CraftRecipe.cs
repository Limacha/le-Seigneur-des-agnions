using entreprise.recherche;
using inventory;
using System;
using UnityEngine;

namespace craft
{
    [CreateAssetMenu(menuName = "craft/New craft")]
    [Serializable]
    public class CraftRecipe : ScriptableObject
    {
        [SerializeField, ReadOnly] public string ID = Guid.NewGuid().ToString(); //l'id unique de la recherche
        [SerializeReference] public ItemData[] result; //le resulta
        [SerializeReference] public Recherche rechercheRequire;  //les recherches qu'ils est requis d'avoir d'ébloquer
        [SerializeReference] public ItemData[] itemsRequire; //les items requit au craft
    }
}
