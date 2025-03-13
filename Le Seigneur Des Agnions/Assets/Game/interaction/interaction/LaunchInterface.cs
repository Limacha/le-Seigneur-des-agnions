using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace interaction
{
    public class launchInterface : InteractionObject
    {
        [SerializeReference] private Transform fenetre;

        public override void InteractionPlayer()
        {
            fenetre.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}