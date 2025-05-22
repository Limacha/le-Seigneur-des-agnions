using interaction;
using inventory;
using mob;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class BroyerInteraction : InteractionObject
{
    [SerializeReference] private ItemData agnion;
    [SerializeReference] private GameObject agnMeatHQ;
    [SerializeReference] private GameObject agnMeatLQ;
    [SerializeReference] private GameObject agnBone;
    [SerializeReference] private Transform spawnPos;
    [SerializeReference] private Animator animator;
    [SerializeReference] private Animator animatorPoussoire;
    [SerializeField, Range(11, 100)] private int chanceForBone = 15;
    [SerializeField, Range(11, 100)] private int chanceForHighMeat = 20;

    public override void InteractionPlayer()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Inventory inv = player.transform.GetChild(0).GetComponent<Inventory>();
            if (inv != null)
            {
                ItemData agn = null;
                if (inv.TwoHandsItem?.ID == agnion.ID)
                {
                    agn = inv.TwoHandsItem;
                    inv.TwoHandsItem = null;
                }
                else if (inv.LeftHandItem?.ID == agnion.ID)
                {
                    agn = inv.LeftHandItem;
                    inv.LeftHandItem = null;
                }
                else if (inv.RightHandItem?.ID == agnion.ID)
                {
                    agn = inv.RightHandItem;
                    inv.RightHandItem = null;
                }

                if (agn && int.TryParse(agn?.PersonalData, out int quality))
                {

                    //Debug.Log(quality);
                    if (quality >= 0 && quality <= 10)
                    {
                        StartCoroutine(SpawnViande(quality));
                    }
                }
            }
        }
    }
    IEnumerator SpawnViande(int quality)
    {
        if (animator != null) { animator.SetBool("Ouvert", false); }

        for (int i = 0; i < 4; i++)
        {
            if (Random.Range(quality, chanceForBone) == 11)
            {
                Instantiate(agnBone, spawnPos);
                if (animatorPoussoire != null) { animatorPoussoire.SetTrigger("poussoire"); }
                yield return new WaitForSeconds(5);

            }
        }


        for (int i = 0; i < quality; i++)
        {
            if (Random.Range(quality, chanceForHighMeat) == 11)
            {
                Instantiate(agnMeatHQ, spawnPos);
            }
            else
            {
                Instantiate(agnMeatLQ, spawnPos);
            }

            if (animatorPoussoire != null) { animatorPoussoire.SetTrigger("poussoire"); }
            yield return new WaitForSeconds(2);
        }

        if (animator != null) { animator.SetBool("Ouvert", true); }
    }
}
