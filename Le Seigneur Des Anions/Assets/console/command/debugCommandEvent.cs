using inventory;
using UnityEngine;

namespace debugCommand
{
    public class DebugCommandEvent : MonoBehaviour
    {
        /// <summary>
        /// commande qui affiche les autres commande
        /// </summary>
        public void Help()
        {
            ConsoleSystem console = GameObject.Find("GameManager").GetComponent<GameManager>().ConsoleSystem;
            string helpText = "";

            foreach (DebugCommand command in console.CommandList)
            {
                helpText += $"{command.Format}\n";
            }
            console.Label = helpText;
        }
        /// <summary>
        /// affiche le texte taper
        /// </summary>
        /// <param name="input">text taper par l'utilisateur</param>
        public void ShowText(string input)
        {
            ConsoleSystem console = GameObject.Find("GameManager").GetComponent<GameManager>().ConsoleSystem;
            if (input.Split('\"').Length > 2)
            {
                console.Label = input.Split('\"')[1];
                return;
            }
            console.Label = "Merci de respecter la syntax.";
        }
        /// <summary>
        /// permet de se donner un object
        /// </summary>
        /// <param name="input">text de l'utilisateur avec le nom[1] puis le nombre[2]</param>
        public void Give(string input)
        {
            ConsoleSystem console = GameObject.Find("GameManager").GetComponent<GameManager>().ConsoleSystem;
            Inventory inventaire = GameObject.Find("Inventory").GetComponent<Inventory>();
            string retour = "Une erreur est survenu.";
            string[] properties = input.Split(' ');

            if (properties.Length == 3)
            {
                ItemData item = inventaire.FindItemWhitName(properties[1]);
                if (item != null)
                {
                    if (int.TryParse(properties[2], out int number))
                    {
                        if (number != 0)
                        {
                            bool possible = true;
                            int i = 0;
                            if (number < 0)
                            {
                                retour = $"Supresion de {-number} {properties[1]}.\n";
                                for (i = 0; i < -number && possible; i++)
                                {
                                    possible = inventaire.RemoveItem(item);
                                }
                                retour += $"{i} {properties[1]} ont été suprimer.";
                            }
                            else
                            {
                                retour = $"Ajout de {number} {properties[1]}.";
                                for (i = 0; i < number && possible; i++)
                                {
                                    possible = inventaire.AddItem(item);
                                }
                                if (!possible)
                                {
                                    i--;
                                }
                                retour += $"{i} {properties[1]} ont été ajouter.";
                            }
                        }
                        else
                        {
                            retour = "Si tu veux rien tu n'aura rien";
                        }
                    }
                    else
                    {
                        retour = "La valeur n°2 n'est pas un entier!";
                    }
                }
                else
                {
                    retour = $"l'item avec comme nom {properties[1]} n'existe pas.";
                }
            }
            else
            {
                retour = "Nombre de proprieter invalid";
            }
            console.Label = retour;
        }
        /// <summary>
        /// sauvegarde une partie sur un nom donner
        /// </summary>
        /// <param name="input">l'entrer de lutilisateur avec le nom de la save[1]</param>
        public void Save(string input)
        {
            ConsoleSystem console = GameObject.Find("GameManager").GetComponent<GameManager>().ConsoleSystem;
            Inventory inventaire = GameObject.Find("Inventory").GetComponent<Inventory>();
            string save = input.Split(" ")[1];

            console.Label = $"sauvegarde de {save}:\n";

            console.Label += $"Tentative de SaveInventory dans {save}.\n";
            SaveSystem.SaveInventory(save, inventaire);
            console.Label += $"Inventaire sauver dans {save}.\n";

            console.Label += $"Fin de la sauvegarde de {save}.\n";
        }
        /// <summary>
        /// charge une partie sur un nom donner
        /// </summary>
        /// <param name="input">l'entrer de lutilisateur avec le nom de la save[1]</param>
        public void Load(string input)
        {
            ConsoleSystem console = GameObject.Find("GameManager").GetComponent<GameManager>().ConsoleSystem;
            Inventory inventaire = GameObject.Find("Inventory").GetComponent<Inventory>();
            string save = input.Split(" ")[1];

            console.Label = $"chargement de {save}:\n";

            console.Label += $"Tentative de LoadInventory de {save}.\n";
            SaveSystem.LoadInventory(save, inventaire);
            console.Label += $"Inventaire charge de {save}.\n";

            console.Label += $"Fin de du chargement de {save}.\n";
        }
    }
}