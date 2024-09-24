using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private float level;
    [SerializeField] private float faim;
    [SerializeField] private float endurance;
    public float Health { get { return health; } set { health = value; } }
    public float Level { get { return level; } set { level = value; } }
    public float Faim { get { return faim; } set { faim = value; } }
    public float Endurance { get { return endurance; } set { endurance = value; } }
    
    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }
    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        health = data.health;
        level = data.level;
        faim = data.faim;
        endurance = data.endurance;
        transform.position = new Vector3(data.position[0], data.position[1], data.position[2]);
    }
}
    