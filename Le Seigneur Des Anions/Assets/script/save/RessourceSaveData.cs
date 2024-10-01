[System.Serializable]
public class RessourceSaveData : ItemSaveData
{
    public string source; //juste pour test

    /// <summary>
    /// genere les donnees de L'item
    /// </summary>
    /// <param name="item">l'item' a stocker les donnees</param>
    public RessourceSaveData(RessourceData item):base(item)
    {
        source = item.Source;
    }
}
