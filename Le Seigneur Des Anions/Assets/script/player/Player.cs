using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float health; //santer du jouer
    [SerializeField] private float faim; //faim du jouer
    [SerializeField] private float endurance; //l'energie du jouer
    [SerializeField] private float camSensibilityX; //sensibiliter de la camera
    [SerializeField] private float camSensibilityY; //sensibiliter de la camera
    public float Health { get { return health; } set { health = value; } }
    public float Faim { get { return faim; } set { faim = value; } }
    public float Endurance { get { return endurance; } set { endurance = value; } }
    public float CameraSensibilityX { get { return camSensibilityX; } set { camSensibilityX = value; } }
    public float CameraSensibilityY { get { return camSensibilityY; } set { camSensibilityY = value; } }
    /*
    /// <summary>
    /// sauvegarde du jouer
    /// </summary>
    public void SavePlayer()
    {
        SaveSystem.SavePlayer(, this);
    }
    /// <summary>
    /// chargemen du jouer
    /// </summary>
    public void LoadPlayer()
    {
        //definition des variable depuis la sauvegarde
        PlayerSaveData data = SaveSystem.LoadPlayer();
        if (data != null)
        {
            //definition des variable charger
            health = data.health;
            faim = data.faim;
            endurance = data.endurance;
            transform.position = new Vector3(data.position[0], data.position[1], data.position[2]);
        }
    }*/
}
    