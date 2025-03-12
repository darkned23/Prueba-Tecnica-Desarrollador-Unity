using System.IO;
using UnityEngine;

public static class RawgFormatter
{
    private static string baseFolderPath = Path.Combine(Application.persistentDataPath, "Files_Rawg");
    private static string saveDataFolderPath = Path.Combine(baseFolderPath, "Save_Data");
    private static string imagesFolderPath = Path.Combine(baseFolderPath, "Images");
    private static string gameDataPath = Path.Combine(saveDataFolderPath, "games.json");

    static RawgFormatter()
    {
        EnsureDirectoriesExist();
    }

    private static void EnsureDirectoriesExist()
    {
        if (!Directory.Exists(saveDataFolderPath))
        {
            Directory.CreateDirectory(saveDataFolderPath);
        }

        if (!Directory.Exists(imagesFolderPath))
        {
            Directory.CreateDirectory(imagesFolderPath);
        }
    }

    #region Game Management
    public static void SaveGames(_pageGame[] data)
    {
        string json = JsonUtility.ToJson(new JsonPagesGames(data), true);
        File.WriteAllText(gameDataPath, json);
    }

    public static _pageGame[] LoadGames()
    {
        if (File.Exists(gameDataPath))
        {
            try
            {
                string json = File.ReadAllText(gameDataPath);
                JsonPagesGames wrapper = JsonUtility.FromJson<JsonPagesGames>(json);
                return wrapper?.pageGames;
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error al cargar juegos: " + e.Message);
                File.Delete(gameDataPath);
                return null;
            }
        }
        else
        {
            return null;
        }
    }

    public static void DeleteGames()
    {
        if (File.Exists(gameDataPath))
        {
            File.Delete(gameDataPath);
        }
    }
    #endregion

    #region Image Management
    public static bool IsImageStored(string gameId)
    {
        string fullPath = GetImagePath(gameId);
        return File.Exists(fullPath);
    }

    public static string GetImagePath(string gameId)
    {
        return Path.Combine(imagesFolderPath, gameId + ".png");
    }

    public static void SaveImage(string gameId, Texture2D texture)
    {
        byte[] imageBytes = texture.EncodeToPNG();
        File.WriteAllBytes(GetImagePath(gameId), imageBytes);
    }

    public static Texture2D LoadImage(string gameId)
    {
        string fullPath = GetImagePath(gameId);

        if (File.Exists(fullPath))
        {
            byte[] imageBytes = File.ReadAllBytes(fullPath);
            Texture2D texture = new Texture2D(2, 2);
            if (texture.LoadImage(imageBytes))
            {
                return texture;
            }
        }

        return null;
    }
    #endregion
}
