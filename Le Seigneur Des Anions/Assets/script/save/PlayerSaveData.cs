[System.Serializable]
public class PlayerSaveData
{
    public float health; //vie du jouer
    public float faim; //faim du jouer
    public float endurance; //endurance du jouer
    public float[] position; //la position du jouer

    /// <summary>
    /// genere les donnees du jouer
    /// </summary>
    /// <param name="player">le jouer a stocker les donnees</param>
    public PlayerSaveData(Player player)
    {
        //transformation des variable en variable serelizable
        position = new float[3] {player.transform.position.x, player.transform.position.y, player.transform.position.z};
        health = player.Health;
        endurance = player.Endurance;
        faim = player.Faim;
    }
}
