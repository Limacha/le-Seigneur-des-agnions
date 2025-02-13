using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using interaction;

namespace player
{
    public class tryPlayerInteraction : MonoBehaviour
    {
        [SerializeField] private float interactDistance; //distance pour interagir
                                                         //[SerializeField] private LayerMask interactLayer; //layer pour interagir
        [SerializeReference] private KeyBiding interactKey; //la touche pour interagir
        [SerializeReference] private Camera playerCamera; //la camera du joueur
        [SerializeReference] private Player player; //la camera du joueur
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * interactDistance, Color.red);
            if (player.CanInteract)
            {
                if (Input.GetKeyDown(interactKey.key))
                {
                    // ground check
                    if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, interactDistance))
                    {
                        if (hit.transform.tag == "interact")
                        {
                            if (hit.transform.TryGetComponent<InteractionObject>(out InteractionObject interaction))
                            {
                                if (interaction != null)
                                {
                                    InteractionObject[] interactionObjects = hit.transform.GetComponents<InteractionObject>();
                                    if (interactionObjects.Length > 0)
                                    {
                                        foreach (InteractionObject interact in interactionObjects)
                                        {
                                            Debug.Log(hit.transform.gameObject.name);
                                            interact.InteractionPlayer();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}