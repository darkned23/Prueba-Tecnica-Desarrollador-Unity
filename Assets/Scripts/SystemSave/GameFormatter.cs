using System;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public static class GameFormatter
{
    private static string savePath;
    private static BinaryFormatter formatter = new();

    public static void SaveGame(GameData data, int slot)
    {
        savePath = GetSavePath(slot);
        using (FileStream stream = new(savePath, FileMode.Create))
        {
            formatter.Serialize(stream, data);
        }
    }

    public static GameData LoadGame(int slot)
    {
        savePath = GetSavePath(slot);
        if (File.Exists(savePath))
        {
            using (FileStream stream = new(savePath, FileMode.Open))
            {
                try
                {
                    return (GameData)formatter.Deserialize(stream);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error loading save file: {e.Message}");
                    return null;
                }
            }
        }
        return null;
    }

    public static void DeleteGame(int slot)
    {
        savePath = GetSavePath(slot);
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("Partida eliminada.");
        }
        else
        {
            Debug.LogWarning("No hay partida para eliminar.");
        }
    }

    private static string GetSavePath(int slot)
    {
        // Define la carpeta sin barra inicial
        string folderPath = Path.Combine(Application.persistentDataPath, "Files_GameData");
        // Asegura que la carpeta exista
        Directory.CreateDirectory(folderPath);

        return Path.Combine(folderPath, $"SaveSlot_{slot}.sav");
    }
}