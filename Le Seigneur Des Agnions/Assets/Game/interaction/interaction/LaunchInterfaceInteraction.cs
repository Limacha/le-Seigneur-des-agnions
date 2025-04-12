using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace interaction
{
    public class launchInterfaceInteraction : InteractionObject
    {
        [SerializeReference] private Transform fenetre;

        public void Start()
        {
            if (fenetre == null)
            {
                Debug.Log($"{gameObject.name}: pas d'interface");
                Destroy(gameObject);
            }
        }

        public override void InteractionPlayer()
        {
            fenetre.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }
}