using inventory;

[System.Serializable]
public class ConsomableSaveData : ItemSaveData
{ 
    /// <summary>
    /// genere les donnees de L'item
    /// </summary>
    /// <param name="item">l'item' a stocker les donnees</param>
    public ConsomableSaveData(ConsomableData item) : base(item)
    {
    }
}
