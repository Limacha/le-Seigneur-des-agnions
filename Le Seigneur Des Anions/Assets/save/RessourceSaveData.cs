using inventory;

[System.Serializable]
public class RessourceSaveData : ItemSaveData
{
    private string source; //juste pour test
    public string Source { get { return source; } set { source = value; } }

    /// <summary>
    /// genere les donnees de L'item
    /// </summary>
    /// <param name="item">l'item' a stocker les donnees</param>
    public RessourceSaveData(RessourceData item):base(item)
    {
        source = item.Source;
    }
}
