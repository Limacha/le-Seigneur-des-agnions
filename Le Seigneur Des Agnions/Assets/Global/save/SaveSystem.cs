using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using inventory;
public static class SaveSystem
{
    //C:\Users\Nico\AppData\LocalLow\Handdor\Le Seigneur Des Agnions\data\save
    private static readonly string defaultPath = Application.persistentDataPath + "/data/save"; //le chemin des sauvegarde par default
    private static readonly BinaryFormatter formatter = new BinaryFormatter(); //le formateur en binaire
    public static string DefaultPath { get { return defaultPath; } }

    /// <summary>
    /// verifier si les folders exists sinon les crees
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
    /// obtient le nom de toute les saves
    /// </summary>
    /// <returns>un tableau avec tout les noms</returns>
    public static string[] GetAllSaveName()
    {
        //Debug.Log(defaultPath);
        string[] directories = Directory.Exists(defaultPath) ? Directory.GetDirectories(defaultPath) : new string[0];
        string[] saveName = new string[directories.Length];

        for (int i = 0; i < directories.Length; i++)
        {
            saveName[i] = Path.GetFileName(directories[i]).ToLower();
        }
        return saveName;
    }

    public static bool RenameSave(string oldSave, string newSave)
    {
        newSave = defaultPath + "/" + newSave;
        oldSave = defaultPath + "/" + oldSave;
        if (!Directory.Exists(newSave))
        {
            if (Directory.Exists(oldSave))
            {
                Directory.Move(oldSave, newSave);
            }
            else
            {
                Directory.CreateDirectory(newSave);
            }
            return true;
        }
        return false;
    }

    public static bool DuplicateSave(string oldSave, string newSave)
    {
        if(oldSave == null || newSave == null || oldSave == newSave)
        {
            return false; 
        }
        oldSave = defaultPath + "/" + oldSave;
        newSave = defaultPath + "/" + newSave;// V�rifie si le dossier source existe
        if (!Directory.Exists(oldSave))
        {
            return false;
        }
        // Cr�e le dossier de destination s'il n'existe pas
        if (Directory.Exists(newSave))
        {
            Directory.Delete(newSave, true);
        }
        Directory.CreateDirectory(newSave);

        CopyDirectory(oldSave, newSave);
        return true;
    }

    private static void CopyDirectory(string oldDir, string newDir)
    {
        // Cr�e le dossier de destination s'il n'existe pas
        if (Directory.Exists(newDir))
        {
            Directory.Delete(newDir, true);
        }
        Directory.CreateDirectory(newDir);

        // Copier les fichiers du dossier source
        foreach (string file in Directory.GetFiles(oldDir))
        {
            string fileName = Path.GetFileName(file);
            string destFile = Path.Combine(newDir, fileName);
            File.Copy(file, destFile, true); // true = overwrite si existe
        }
        // Copier les sous-dossiers r�cursivement
        foreach (string subDir in Directory.GetDirectories(oldDir))
        {
            string subDirName = Path.GetFileName(subDir);
            string destSubDir = Path.Combine(newDir, subDirName);
            CopyDirectory(subDir, destSubDir);
        }
    }

    /// <summary>
    /// suppression d'une sauvegarde
    /// </summary>
    /// <param name="save">nom de la save</param>
    public static void DeleteSave(string save)
    {
        string path = defaultPath + "/" + save;
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }
    }
    /*
    /// <summary>
    /// sauvegarde les donne du serveur
    /// </summary>
    /// <param name="player">le jouer dont il faus save les data</param>
    public static void SavePlayer(string save, Player player)
    {
        string path = $"{defaultPath}/{save}/player/player.assa"; //le chemin
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
    public static PlayerSaveData LoadPlayer(string save)
    {
        string path = $"{defaultPath}/{save}/player/player.assa";
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
    */
    /// <summary>
    /// sauvegarde les donne du serveur
    /// </summary>
    /// <param name="player">le jouer dont il faus save les data</param>
    public static void SaveInventory(string save, Inventory inv)
    {
        string path = $"{defaultPath}/{save}/inventory/inv.assa";
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
    public static InventorySaveData LoadInventory(string save)
    {
        string path = $"{defaultPath}/{save}/inventory/inv.assa";
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
