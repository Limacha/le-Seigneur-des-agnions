using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float health; //santer du jouer
    [SerializeField] private float faim; //faim du jouer
    [SerializeField] private float endurance; //l'energie du jouer
    [SerializeField] private bool canMouve; //si le joueur peut bouger
    [SerializeField] private bool canLookAround; //si le joueur peut bouger la camera
    [SerializeField] private bool canInteract; //si le joueur peut interagir
    [SerializeField] private bool animationPlayed = false; //si une animation est jouer
    public float Health { get { return health; } set { health = value; } }
    public float Faim { get { return faim; } set { faim = value; } }
    public float Endurance { get { return endurance; } set { endurance = value; } }
    public bool CanMouve { get { return canMouve; } set { canMouve = value; } }
    public bool CanLookAround { get { return canLookAround; } set { canLookAround = value; } }
    public bool CanInteract { get { return canInteract; } set { canInteract = value; } }
    public bool AnimationPlayed { get { return animationPlayed; } set { animationPlayed = value; } }
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
    