using UnityEngine;
using interaction;
using System.Linq;

namespace player
{
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] private float interactDistance; //distance pour interagir
        [SerializeField] private string[] interactTag; //tout les tags avec les quel interagir

        [SerializeReference, ReadOnly] private Player player; //la camera du joueur
        [SerializeReference] private Camera playerCamera;
        private Ray ray;

        public float InteractDistance { get { return interactDistance; } } //distance pour interagir

        void Start()
        {
            player = gameObject.GetComponent<Player>();
            if (player == null)
            {
                Destroy(gameObject);
            }
        }
        
        // Update is called once per frame
        void Update()
        {
            ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * interactDistance, Color.green);
        }

        public void LaunchInteract()
        {
            // ground check
            if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, interactDistance))
            {
                if (interactTag.Count((tag) => { return tag == hit.transform.tag; }) >= 0)
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
                                    //Debug.Log(hit.transform.gameObject.name);
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
/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}*/
