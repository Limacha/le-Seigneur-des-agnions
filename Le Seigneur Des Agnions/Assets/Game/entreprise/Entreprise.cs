using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace entreprise
{
    public class Entreprise : MonoBehaviour
    {
        [Header("")]
        private string nom;
        private int argent;
        private Employer[] employers;
        /*[Header("")]
        private int danger;
        private Infraction[] infractions;*/

        private int batiment;
        private Upgrade[] upgrades;
        private Recherche[] recherches;

        private int reputation;
    }
}