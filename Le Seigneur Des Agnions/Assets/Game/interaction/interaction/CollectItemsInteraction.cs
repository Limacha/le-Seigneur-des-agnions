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
        }

        public override void InteractionPlayer()
        {
            if (canCollect)
            {
                bool add = false;
                int restric = 0;
                bool needHand = false;
                //parcour des restriction
                while (restric < item.Restriction.Length && !add)
                {
                    if (needHand)
                    {
                        if (inv.Hands != null)
                        {
                            if (item.Restriction[restric] == ItemData.Restrict.leftAndRightHand)
                            {
                                inv.EquipItem(item, false, Inventory.HandPosition.Both);
                                add = true;
                            }
                            else if (item.Restriction[restric] == ItemData.Restrict.leftHand)
                            {
                                inv.EquipItem(item, false, Inventory.HandPosition.Left);
                                add = true;
                            }
                            else if (item.Restriction[restric] == ItemData.Restrict.rightHand)
                            {
                                inv.EquipItem(item, false, Inventory.HandPosition.Right);
                                add = true;
                            }
                            else if (item.Restriction[restric] == ItemData.Restrict.leftOrRightHand)
                            {
                                if (inv.RightHand == null)
                                {
                                    inv.EquipItem(item, false, Inventory.HandPosition.Right);
                                    add = true;
                                }
                                else if (inv.LeftHand == null)
                                {
                                    inv.EquipItem(item, false, Inventory.HandPosition.Left);
                                    add = true;
                                }
                                else
                                {
                                    inv.EquipItem(item, false, Inventory.HandPosition.Right);
                                    add = true;
                                }
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        //si dans l'inventaire
                        if (item.Restriction[restric] == ItemData.Restrict.inventory)
                        {
                            if (inv.AddItem(item))
                            {
                                add = true;
                            }
                        }
                        else if (item.Restriction[restric] == ItemData.Restrict.haveHand)
                        {
                            needHand = true;
                        }
                        else if (item.Restriction[restric] == ItemData.Restrict.leftHand)
                        {
                            inv.EquipItem(item, false, Inventory.HandPosition.Left);
                            add = true;
                        }
                        else if (item.Restriction[restric] == ItemData.Restrict.rightHand)
                        {
                            inv.EquipItem(item, false, Inventory.HandPosition.Right);
                            add = true;
                        }
                        else if (item.Restriction[restric] == ItemData.Restrict.leftOrRightHand)
                        {
                            if (inv.RightHand == null)
                            {
                                inv.EquipItem(item, false, Inventory.HandPosition.Right);
                                add = true;
                            }
                            else if (inv.LeftHand == null)
                            {
                                inv.EquipItem(item, false, Inventory.HandPosition.Left);
                                add = true;
                            }
                            else
                            {
                                inv.EquipItem(item, false, Inventory.HandPosition.Right);
                                add = true;
                            }
                        }
                        else if (item.Restriction[restric] == ItemData.Restrict.leftAndRightHand)
                        {
                            inv.EquipItem(item, false, Inventory.HandPosition.Both);
                            add = true;
                        }
                    }
                }
                if (add)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}