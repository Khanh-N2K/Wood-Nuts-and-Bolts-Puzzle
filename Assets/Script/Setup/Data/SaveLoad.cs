using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using System;

public static class SaveLoad
{
    public static void SaveData(object data, DataTypes dataType)
    {
        BinaryFormatter formatter = new();
        string path = Application.persistentDataPath + "/" + dataType.ToString() + ".khanh"; 
        FileStream stream = new(path, FileMode.Create);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static object LoadData(DataTypes dataType)
    {
        string path = Application.persistentDataPath + "/" + dataType.ToString() + ".khanh";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new();
            FileStream stream = new(path, FileMode.Open);
            Type type = Type.GetType(dataType.ToString());
            object data = formatter.Deserialize(stream);
            stream.Close();
            if (type.IsInstanceOfType(data))
                return data;

            Debug.LogError("Can't parse type of saved data");
            return null;
        }
        else
        {
            Debug.LogWarning("Error: Save file not found in " + path);
            return null;
        }
    }
}

public enum DataTypes
{
    CurrentLevel,
    AvailableLevel,
    Gold,
    BackGround,
    LevelRank,
    BoltSkinData,
    BoardSkinData,

    // Minigame
    CurrentImage_MinigameData
}