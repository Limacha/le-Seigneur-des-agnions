using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

namespace debugCommand { 
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
            string[] properties = input.Split(' ');

            if (properties.Length == 3)
            {
                if (int.TryParse(properties[2], out int number))
                {
                    console.Label = $"{properties[1]}\n{number}";
                }
                else
                {
                    console.Label = "la valeur n°2 n'est pas un entier!";
                }
            } else
            {
                console.Label = "nombre de proprieter invalid";
            }
        }
    }
}