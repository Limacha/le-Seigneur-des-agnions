using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using inventory;
public static class SaveSystem
{
    static readonly string defaultPath = Application.persistentDataPath + "/data/save"; //le chemin des sauvegarde par default
    static readonly BinaryFormatter formatter = new BinaryFormatter(); //le formateur en binaire
    static readonly GameObject gameManager = GameObject.Find("GameManager"); //gestionaire du jeux
    public static string DefaultPath { get { return defaultPath; } }

    /// <summary>
    /// verifier si les folder exists sinon les crees
    /// </summary>
    /// <param name="path">le chemin choisi</param>
    private static void DirectoryExistsOrCreate(string path)
    {
        string[] directorys = path.Split('/');
        string directory = "";
        //Debug.Log(directorys.Length);
        //parcour du chemin total
        for (int i = 0; i < directorys.Length - 1; i++)
        {
            directory += directorys[i] + "/";
            //Debug.Log(directory);
            //verifier cette partie de chemin
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }
    }
    /// <summary>
    /// delete une sauvegarde
    /// </summary>
    public static void DeleteSave()
    {
        string path = defaultPath + "/" + gameManager.GetComponent<GameManager>().Save;
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }
    }
    /// <summary>
    /// sauvegarde les donne du serveur
    /// </summary>
    /// <param name="player">le jouer dont il faus save les data</param>
    public static void SavePlayer(Player player)
    {
        string path = $"{defaultPath}/{gameManager.GetComponent<GameManager>().Save}/player/player.assa"; //le chemin
        //Debug.Log(path);

        DirectoryExistsOrCreate(path);

        FileStream stream = new FileStream(path, FileMode.Create); //creation du lien vers le fichier
        PlayerSaveData playerData = new PlayerSaveData(player); //donner a save
        formatter.Serialize(stream, playerData); //formatage
        stream.Close(); //fermeture du fichier
        //Debug.Log($"Save file set in: {path}");
    }
    /// <summary>
    /// charge les info du jouer
    /// </summary>
    /// <returns>les info du jouer</returns>
    public static PlayerSaveData LoadPlayer()
    {
        string path = $"{defaultPath}/{gameManager.GetComponent<GameManager>().Save}/player/player.assa";
        if (File.Exists(path))
        {
            FileStream stream = new FileStream(path, FileMode.Open);
            PlayerSaveData playerData =  formatter.Deserialize(stream) as PlayerSaveData; //lecteur des info
            stream.Close();

            return playerData;
        }
        else
        {
            //Debug.LogError($"Save file not found in: {path}");
            return null;
        }
    }

    /// <summary>
    /// sauvegarde les donne du serveur
    /// </summary>
    /// <param name="player">le jouer dont il faus save les data</param>
    public static void SaveInventory(Inventory inv)
    {
        string path = $"{defaultPath}/{gameManager.GetComponent<GameManager>().Save}/inventory/inv.assa";
        //Debug.Log(path);

        DirectoryExistsOrCreate(path);

        FileStream stream = new FileStream(path, FileMode.Create);
        InventorySaveData inventoryData = new InventorySaveData(inv); //creation des infos a saves
        formatter.Serialize(stream, inventoryData);
        stream.Close();
        //Debug.Log($"Save file set in: {path}");
    }
    /// <summary>
    /// charge les info du jouer
    /// </summary>
    /// <returns>les info du jouer</returns>
    public static InventorySaveData LoadInventory(Inventory inv)
    {
        string path = $"{defaultPath}/{gameManager.GetComponent<GameManager>().Save}/inventory/inv.assa";
        if (File.Exists(path))
        {
            FileStream stream = new FileStream(path, FileMode.Open);
            InventorySaveData inventoryData = formatter.Deserialize(stream) as InventorySaveData; //load les infos
            stream.Close();

            return inventoryData;
        }
        else
        {
            //Debug.LogError($"Save file not found in: {path}");
            return null;
        }
    }
}
