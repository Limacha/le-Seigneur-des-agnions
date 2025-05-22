using inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace entreprise.recherche
{
    [CreateAssetMenu(menuName = "recherche/New recherche")]
    [Serializable]
    public class Recherche : ScriptableObject
    {
        [SerializeField, ReadOnly] public string ID = Guid.NewGuid().ToString(); //l'id unique de la recherche
        [SerializeField] public string nom; //nom de la recherche
        [SerializeField] public string description; //description de la recherche
        [SerializeReference] public Recherche[] recherchesRequire;  //les recherches qu'ils est requis d'avoir d'ébloquer
        [SerializeReference] public ItemData[] itemsRequire; //les items requit a la recherche
    }
}