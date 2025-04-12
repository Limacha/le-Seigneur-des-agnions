using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace entreprise
{
    public class Entreprise : MonoBehaviour
    {
        private class UpgradeRestriction
        {
            private int batimentLevel;
            private int argent;
            private int etoile; //0-5
            private int recherche;

            public int BatimentLevel { get { return batimentLevel; } }
            public int Argent { get { return argent; } }
            public int Etoile { get { return etoile; } }
            public int Recherche { get { return recherche; } }

            public UpgradeRestriction(int batimentLevel, int argent, int reputation, int recherche)
            {
                this.batimentLevel = batimentLevel;
                this.argent = argent;
                this.etoile = reputation;
                this.recherche = recherche;
            }
        }

        [Header("Principal")]
        [SerializeField] private string nom; //nom de l'entreprise
        [SerializeField] private float argent; //argent posseder
        [SerializeField] private int batiment; //niveau du batiment
        [SerializeField] private int batimentMax; //niveau max du batiement
        [SerializeField, Range(0, 100)] private int reputation; //0-100 -> /20 -> x/5 (n etoile)
        private readonly UpgradeRestriction[] batimentRestriction = new UpgradeRestriction[]
        {
            new UpgradeRestriction(0, 500, 1, 0),
            new UpgradeRestriction(1, 1000, 2, 1),
            new UpgradeRestriction(2, 50000, 2, 2),
            new UpgradeRestriction(3, 8954652, 5, 3),
            new UpgradeRestriction(4, 648634198, 8, 4)
        };//tableau de toute les restriction possible

        [SerializeField, ReadOnly] private Func<string, bool> functionInput; //function qui sera executer lors de l'entrer d'un input

        private List<Employer> employers; //liste des employer
        private List<Recherche> recherches; //liste des recherches
        private List<Dette> dettes = new() {
            new ("tuto", "une dette pour le tuto", 100, new DateTime(1, 1, 1), 365, 10, 365*5+1, 5),
            new ("test", "juste pour test", 5000, new DateTime(1, 2, 3), 958, 2, 958*5, 2)
        }; //liste des dettes

        private int danger; //a quel point l'entreprise semble illegal
        private string phraseAcueil = "Bonjour monsieur que voulez vous faire?"; //phrase afficher par default dans le menu


        public string Nom { get { return nom; } set { nom = value; } }
        public float Argent { get { return argent; } }
        public int Batiment { get { return batiment; } }
        public int Reputation { get { return reputation; } }
        public int Etoile { get { return reputation / 20; } }

        /// <summary>
        /// fait un modification du montant de l'argent de l'entreprise
        /// </summary>
        /// <param name="montant">le montent</param>
        public void Transaction(float montant)
        {
            argent += montant;
        }

        /// <summary>
        /// appele la fonction dans fonctionInput et lui donne l'input 
        /// </summary>
        /// <param name="input">l'entrer de l'utilisateur</param>
        public void GetMenuInput(string input)
        {
            //appeler la fonction si pas null
            if (functionInput != null)
            {
                functionInput.Invoke(input);
            }
            else
            {
                //Debug.Log("no function" + input);
            }
        }

        /// <summary>
        /// change la reputation par la reputation donner max 10
        /// </summary>
        /// <param name="reput">la nouvelle reputation</param>
        public void ChangerReput(int reput)
        {
            reputation = reput;
            if (reputation > 10)
            {
                reputation = 10;
            }
        }

        /// <summary>
        /// affiche les informations de l'entreprise
        /// </summary>
        /// <param name="label">l'endroit ou les affichers</param>
        public void DisplayInformation(TextMeshProUGUI label)
        {
            label.text = $"{nom}: \n";
            label.text += $"{argent}€\n";
            label.text += $"{batiment}/{batimentMax}\n";
            label.text += $"{Etoile} reputation\n";
        }

        #region amelioration du batiment

        /// <summary>
        /// renvoie un string contenant toute les restriction a afficher
        /// </summary>
        /// <param name="restriction">les restriction</param>
        /// <returns>le string cree</returns>
        private string AfficherRestriction(UpgradeRestriction restriction)
        {
            if (restriction != null)
            {
                return $"niveau du batiment {batiment} == {restriction.BatimentLevel}\n" +
                       $"argent requis: {argent}/{restriction.Argent}\n" +
                       $"reputation: {reputation}/{restriction.Etoile}\n" +
                       $"recherche: vraix/{restriction.Recherche}\n";
            }
            else
            {
                return $"aucune restriction trouver pour améliorer un batiment de niveau {batiment}\n";
            }
        }

        /// <summary>
        /// verifie les restrictions
        /// </summary>
        /// <param name="restriction">les restriction a verifier</param>
        /// <returns>si les restriction sont remplis</returns>
        private bool VerifierRestriction(UpgradeRestriction restriction)
        {
            if (restriction != null)
            {
                return (batiment == restriction.BatimentLevel && argent >= restriction.Argent && Etoile >= restriction.Etoile && true);
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// applique les restriction
        /// </summary>
        /// <param name="restriction">les restriction a appliquer</param>
        private void AppliquerRestriction(UpgradeRestriction restriction)
        {
            if (restriction != null)
            {
                Transaction(-restriction.Argent);
            }
        }

        /// <summary>
        /// lance tout le processus d'amelioration du batiment
        /// </summary>
        /// <param name="label">l'endroit </param>
        public void LaunchBatUpgrade(TextMeshProUGUI label)
        {
            UpgradeRestriction restriction = batimentRestriction.FirstOrDefault(restriction => restriction.BatimentLevel == batiment);
            label.text = AfficherRestriction(restriction);
            label.text += "\nVoullez-vous proceder a l'amelioration: oui/non\n";

            functionInput = (input) =>
            {
                functionInput = null;
                if (input.ToLower() == "oui")
                {

                    if (VerifierRestriction(restriction))
                    {
                        AppliquerRestriction(restriction);
                        batiment++;
                        //lancer animation pour le nouveau bat
                        if (batiment > batimentMax)
                        {
                            batiment = batimentMax;
                        }
                        label.text = "Amelioration faite";
                    }
                    else
                    {
                        label.text = "Amelioration impossible";
                    }
                    return true;
                }
                label.text = phraseAcueil;
                return false;
            };

        }

        #endregion

        #region dette/fisc

        /// <summary>
        /// afficher une liste de dette et defini une fonction pour l'input
        /// </summary>
        /// <param name="label">l'endroit ou afficher</param>
        public void DisplayDettes(TextMeshProUGUI label)
        {
            label.text = "Liste des dettes:";
            for (int i = 0; i < dettes.Count(); i++)
            {
                label.text += $"\n{i + 1}:\n";
                label.text += dettes[i].Info();
                label.text += "\n";
            }
            label.text += "\nEntrer le numero de l'amende que vous voulez payer.";
            functionInput = (input) =>
            {
                functionInput = null;
                if (ushort.TryParse(input, out ushort nDette))
                {
                    //si une dette est selectionner
                    nDette--;
                    if (nDette < dettes.Count())
                    {
                        label.text = dettes[nDette].Info();
                        label.text += "\n\nVoulez vous payer?\noui ou non.";
                        functionInput = (input) =>
                        {
                            functionInput = null;
                            if(input == "oui")
                            {
                                //payer la dette si il peut
                                if (dettes[nDette].Montant <= argent)
                                {
                                    Transaction(-dettes[nDette].Montant);
                                    dettes.Remove(dettes[nDette]);
                                    label.text = "Dette payez.";
                                    return true;
                                }
                                label.text = "Argent insufisant.";
                                return false;
                            }
                            label.text = phraseAcueil;
                            return false;
                        };
                    }
                    else
                    {
                        label.text = phraseAcueil;
                    }
                }
                else
                {
                    label.text = phraseAcueil;
                }
                return false;
            };
        }

        /// <summary>
        /// affiche une liste de tout les prets
        /// </summary>
        /// <param name="label">l'endroit ou afficher</param>
        public void DisplayPrets(TextMeshProUGUI label)
        {
            if(batiment >= 1)
            {
                if(Etoile >= 2)
                {
                    label.text = "fonction pas encore ajouter.";
                    return;
                }
                else
                {
                    label.text = "Vous avez une trop mauvaise reputation pour faire un pret.";
                    return;
                }
            }
            label.text = "Votre batiment n'est pas assez haut level pour pouvoir faire un pret.";
            return;
        }

        /// <summary>
        /// affiche un menu pour faire les dettes ou les prets
        /// </summary>
        /// <param name="label">la zone ou afficher</param>
        public void DisplayFisc(TextMeshProUGUI label)
        {
            label.text = "Voulez vous faire un pret ou voir vos dette.\n 1)pret \n 2)dette";
            functionInput = (input) => {
                functionInput = null;
                if (input == "1") { 
                    DisplayPrets(label); 
                } 
                else if (input == "2") { 
                    DisplayDettes(label); 
                } 
                else { label.text = phraseAcueil; 
                }
                return true; 
            };
        }

        #endregion

        #region name

        public void SetName(TextMeshProUGUI label)
        {
            if (GameObject.Find("GameManager"))
            {
                if (GameObject.Find("GameManager").TryGetComponent<GameManager>(out GameManager manager)) { 
                        label.text = "Entrez le nouveau nom\n" +
                                 "nom deja existant:\n";
                    for (int i = 0; i < SaveSystem.GetAllSaveName().Length; i++)
                    {
                        label.text += SaveSystem.GetAllSaveName()[i] + "\n";
                    }
                    functionInput = (input) =>
                    {
                        functionInput = null;
                        if (!SaveSystem.GetAllSaveName().Contains(input))
                        {
                            string save = input.ToLower();
                            label.text = $"nouveau nom: {input}\n";
                            label.text += "Merci de confirmer ce changement?\noui\nnon";

                            functionInput = (input) =>
                            {
                                functionInput = null;
                                if (input == "oui")
                                {
                                    if (manager.ChangeSaveName(save))
                                    {
                                        label.text = "nom changer";
                                        return true;
                                    }
                                    else
                                    {
                                        label.text = "probleme survenu";
                                        return false;
                                    }
                                }
                                else
                                {
                                    label.text = phraseAcueil;
                                }
                                return false;
                            };
                            return true;
                        }
                        else
                        {
                            label.text = "nom existe deja.";
                        }
                        return false;
                    };
                }
                else
                {
                    label.text = "pas de gameManager";
                }
            }
            else
            {
                label.text = "pas de gameManager";
            }
        }

        #endregion
    }
}