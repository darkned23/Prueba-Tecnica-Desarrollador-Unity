using System.IO;
using UnityEngine;

public static class RawgSave
{
    private static string gameDataPath = Application.persistentDataPath + "/Files_Rawg/Save_Data/games.json";
    private static string imageFolderPath = Application.persistentDataPath + "/Files_Rawg/Images/";

    #region Game Management
    // Guardar juegos en JSON
    public static void SaveGames(pageGame[] data)
    {
        string json = JsonUtility.ToJson(new JsonPagesGames(data), true);
        File.WriteAllText(gameDataPath, json);
    }

    // Cargar juegos desde JSON
    public static pageGame[] LoadGames()
    {
        if (File.Exists(gameDataPath))
        {
            try
            {
                string json = File.ReadAllText(gameDataPath);
                JsonPagesGames wrapper = JsonUtility.FromJson<JsonPagesGames>(json);

                Debug.Log("La ruta del archivo es: " + gameDataPath);
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
            Debug.LogWarning("No se encontró el archivo en: " + gameDataPath);
            return null;
        }
    }

    // Eliminar archivo JSON de juegos
    public static void DeleteGames()
    {
        if (File.Exists(gameDataPath))
        {
            File.Delete(gameDataPath);
        }
    }
    #endregion

    #region Image Management
    // Verifica si la imagen ya está almacenada
    public static bool IsImageStored(string gameId)
    {
        string fullPath = GetImagePath(gameId);
        return File.Exists(fullPath);
    }

    // Retorna la ruta de la imagen basada en el ID del juego
    public static string GetImagePath(string gameId)
    {
        return Path.Combine(imageFolderPath, gameId + ".png");
    }

    // Guarda la imagen en almacenamiento persistente
    public static void SaveImage(string gameId, Texture2D texture)
    {
        if (!Directory.Exists(imageFolderPath))
        {
            Directory.CreateDirectory(imageFolderPath);
        }

        byte[] imageBytes = texture.EncodeToPNG();
        File.WriteAllBytes(GetImagePath(gameId), imageBytes);
        Debug.Log($"Imagen guardada en {GetImagePath(gameId)}");
    }

    // Carga la imagen desde almacenamiento persistente
    public static Texture2D LoadImage(string gameId)
    {
        string fullPath = GetImagePath(gameId);

        if (File.Exists(fullPath))
        {
            byte[] imageBytes = File.ReadAllBytes(fullPath);
            Texture2D texture = new Texture2D(2, 2);
            if (texture.LoadImage(imageBytes))
            {
                Debug.Log($"Imagen cargada desde almacenamiento: {fullPath}");
                return texture;
            }
        }

        return null;
    }
    #endregion
}
