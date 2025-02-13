using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace interaction
{
    public abstract class InteractionEvent
    {
        /// <summary>
        /// commande qui affiche les autres commande
        /// </summary>
        public void Help()
        {
            Debug.Log("work");
        }
        /// <summary>
        /// commande qui affiche les autres commande
        /// </summary>
        public void Hlp()
        {
            Debug.Log("wk");
        }

        public void LaunchInterface(GameObject obj)
        {
            Debug.Log("name:" + obj.name);
        }
    }
}