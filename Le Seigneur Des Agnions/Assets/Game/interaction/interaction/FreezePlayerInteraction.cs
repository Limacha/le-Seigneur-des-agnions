using interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FreezePlayerInteraction : InteractionObject
{
    [SerializeField] private bool freezMove = false;
    [SerializeField] private bool freezLookAround = false;
    [SerializeField] private bool freezInteract = false;
    [SerializeReference] private Player player;
    public override void InteractionPlayer()
    {
        player.CanMouve = freezMove;
        player.CanLookAround = freezLookAround;
        player.CanInteract = freezInteract;
    }
}
