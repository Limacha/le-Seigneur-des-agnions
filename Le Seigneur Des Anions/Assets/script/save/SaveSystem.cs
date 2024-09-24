using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public static class SaveSystem
{
    static readonly string defaultPath = Application.persistentDataPath + "/data";
    static readonly BinaryFormatter formatter = new BinaryFormatter();
    static readonly GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

    private static void DirectoryExistsOrCreate(string path)
    {
        string[] directorys = path.Split('/');
        string directory = "";
        //Debug.Log(directorys.Length);
        for (int i = 0; i < directorys.Length - 1; i++)
        {
            directory += directorys[i] + "/";
            //Debug.Log(directory);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }
    }
    public static void SavePlayer(Player player)
    {
        string path = $"{defaultPath}/{gameManager.Save}/player.assa";
        //Debug.Log(path);

        DirectoryExistsOrCreate(path);

        FileStream stream = new FileStream(path, FileMode.Create);
        PlayerData playerData = new PlayerData(player);

        formatter.Serialize(stream, playerData);
        stream.Close();
        //Debug.Log($"Save file set in: {path}");
    }

    public static PlayerData LoadPlayer()
    {
        string path = $"{defaultPath}/{gameManager.Save}/player.assa";
        DirectoryExistsOrCreate(path);
        if (File.Exists(path))
        {
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData playerData =  formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return playerData;
        }
        else
        {
            //Debug.LogError($"Save file not found in: {path}");
            return null;
        }
    }
    /*
    public static void SaveInventory(Player player, string path)
    {
        FileStream stream = new FileStream(path, FileMode.Create);
        PlayerData playerData = new PlayerData(player);

        formatter.Serialize(stream, playerData);
        stream.Close();
        //Debug.Log($"Save file set in: {savePlayerPath}");
    }

    public static PlayerData LoadInventory(string path)
    {
        if (File.Exists(path))
        {
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData playerData = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return playerData;
        }
        else
        {
            Debug.LogError($"Save file not found in: {path}");
            return null;
        }
    }*/
}
