using entreprise.venteAgnion;
using interaction;
using inventory;
using mob;
using System;
using UnityEngine;
using static UnityEditor.Progress;

namespace Assets.Game.interaction.interaction
{
    public class SellAgnionInteraction: InteractionObject
    {
        [SerializeReference] private SiteVente site;
        [SerializeReference] private ItemData agnData;
        [SerializeReference, ReadOnly] private Inventory inv;

        public void Start()
        {
            inv = GameObject.FindWithTag("inventory")?.GetComponent<Inventory>();
            if (site == null)
            {
                Debug.Log($"{gameObject.name}: pas de site");
                Destroy(gameObject);
            }
            if (inv == null)
            {
                Debug.Log($"{gameObject.name}: pas d'inventaire trouver");
                Destroy(gameObject);
            }
        }

        public override void InteractionPlayer()
        {
            (ItemData hand, Action clearHand)[] hands =
            {
                (inv.LeftHand, () => inv.LeftHand = null),
                (inv.RightHand, () => inv.RightHand = null),
                (inv.TwoHands, () => inv.TwoHands = null)
            };

            foreach (var (hand, clearHand) in hands)
            {
                if (hand?.ID == agnData.ID && byte.TryParse(hand.PersonalData, out byte quality))
                {
                    site.AddSellAgnion(quality);
                    clearHand();
                    break;
                }
            }
        }
    }
}
