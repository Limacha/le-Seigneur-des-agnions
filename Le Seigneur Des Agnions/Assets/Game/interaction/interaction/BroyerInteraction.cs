using interaction;
using inventory;
using mob;
using UnityEngine;

public class BroyerInteraction : InteractionObject
{
    [SerializeReference] private ItemData agnion;
    [SerializeReference] private GameObject agnPart;
    [SerializeReference] private Transform spawnPos;
    public override void InteractionPlayer()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Inventory inv = player.transform.GetChild(0).GetComponent<Inventory>();
            if (inv != null)
            {
                ItemData agn = null;
                if (inv.TwoHands?.ID == agnion.ID)
                {
                    agn = inv.TwoHands;
                    inv.TwoHands = null;
                }
                else if (inv.LeftHand?.ID == agnion.ID)
                {
                    agn = inv.LeftHand;
                    inv.LeftHand = null;
                }
                else if (inv.RightHand?.ID == agnion.ID)
                {
                    agn = inv.RightHand;
                    inv.RightHand = null;
                }

                if (agn && int.TryParse(agn?.PersonalData, out int quality))
                {
                    //Debug.Log(quality);
                    if (quality >= 0 && quality <= 10)
                    {
                        for (int i = 0; i < quality; i++)
                        {
                            Instantiate(agnPart, spawnPos);
                        }
                    }
                }
            }
        }
    }
}
