using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace interaction
{
    public class FreezePlayerInteraction : InteractionObject
    {
        [SerializeField] private bool freezMove = false;
        [SerializeField] private bool freezLookAround = false;
        [SerializeField] private bool freezInteract = false;
        [SerializeReference, ReadOnly] private Player player;

        public void Start()
        {
            shine = false;
            player = GameObject.FindWithTag("Player").GetComponent<Player>();
            if (player == null)
            {
                Debug.Log($"{gameObject.name}: pas de joueur trouver");
                Destroy(gameObject);
            }

        }

        public override void InteractionPlayer()
        {
            player.CanMouve = !freezMove;
            player.CanLookAround = !freezLookAround;
            player.CanInteract = !freezInteract;
        }
    }
}