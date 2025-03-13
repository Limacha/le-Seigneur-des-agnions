using UnityEngine;

namespace entreprise
{
    public class Entreprise : MonoBehaviour
    {
        [Header("Principal")]
        [SerializeField] private string nom;
        [SerializeField] private float argent;
        [SerializeField] private int batiment;
        [SerializeField] private int reputation;

        //private Employer[] employers;
        /*[Header("")]
        private int danger;
        private Infraction[] infractions;*/

        //private Upgrade[] upgrades;
        //private Recherche[] recherches;


        public string Nom { get { return nom; } }
        public float Argent { get { return argent; } }
        public int Batiment { get { return batiment; } }
        public int Reputation { get { return reputation; } }

        public void Transaction(float montant)
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