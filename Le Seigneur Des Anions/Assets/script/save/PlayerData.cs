[System.Serializable]
public class PlayerData
{
    public float health;
    public float faim;
    public float endurance;
    public float level;
    public float[] position;

    public PlayerData(Player player)
    {
        position = new float[3] {player.transform.position.x, player.transform.position.y, player.transform.position.z};
        health = player.Health;
        level = player.Level;
        endurance = player.Endurance;
        faim = player.Faim;
    }
}
