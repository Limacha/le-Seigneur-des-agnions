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
                helpText += $"{command.Format}:\n{command.Description}\n";
            }
            console.Label = helpText;
        }
        /// <summary>
        /// juste pour test system input
        /// </summary>
        /// <param name="input">text taper par l'utilisateur</param>
        public void Test(string input)
        {
            ConsoleSystem console = GameObject.Find("GameManager").GetComponent<GameManager>().ConsoleSystem;
            console.Label = input;
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
    }
}