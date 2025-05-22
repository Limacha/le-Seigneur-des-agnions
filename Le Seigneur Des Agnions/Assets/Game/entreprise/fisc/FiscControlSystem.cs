using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace entreprise.fisc
{
    public static class FiscControlSystem
    {
        /// <summary>
        /// lance la demande de corruption
        /// </summary>
        public static void LaunchCorruption(Entreprise ent, GameManager manager)
        {
            uint danger = (uint)((ent.Argent) * ent.Reputation / 1000);
            ent.CreateDette("don apc", "un petit don pour l'apc.", danger, manager.ThisDate, 7, 10, 21);
        }
    }
}