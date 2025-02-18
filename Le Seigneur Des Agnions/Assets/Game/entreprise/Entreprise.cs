using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace entreprise
{
    public class Entreprise : MonoBehaviour
    {
        [Header("Principal")]
        private string nom;
        private int argent;
        private int batiment;
        private int reputation;

        //private Employer[] employers;
        /*[Header("")]
        private int danger;
        private Infraction[] infractions;*/

        //private Upgrade[] upgrades;
        //private Recherche[] recherches;


        public string Nom { get { return nom; } }
        public int Argent { get { return argent; } }
        public int Batiment { get { return batiment; } }
        public int Reputation { get { return reputation; } }

        public void Transaction(int montant)
        {
            argent += montant;
        }

        public void Upgrade()
        {
            batiment++;
        }
        public void ChangerReput(int reput)
        {
            reputation = reput;
        }
    }
}