using mob;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using inventory;

namespace interaction
{
    public class CollectItemsInteraction : InteractionObject
    {
        //[SerializeReference] private Agnion fenetre;
        [SerializeReference] private bool canCollect;
        [SerializeReference] private ItemData item;
        [SerializeReference, ReadOnly] private Inventory inv;

        public void Start()
        {
            inv = GameObject.Find("Inventory").GetComponent<Inventory>();
            if(inv == null)
            {
                Debug.Log($"{gameObject.name}: pas d'inventaire");
                Destroy(gameObject);
            }
            if (item == null)
            {
                Debug.Log($"{gameObject.name}: pas d'item");
                Destroy(gameObject);
            }
        }

        public override void InteractionPlayer()
        {
            if(gameObject.TryGetComponent(out Agnion agnion))
            {
                item.PersonalData = agnion.Quality.ToString();
            }

            if (canCollect)
            {
                if (inv.EquipItem(item, false))
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}