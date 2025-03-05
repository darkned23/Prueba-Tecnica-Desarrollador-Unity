using System;
using UnityEngine;

[Serializable]
public class JsonPagesGames
{
    public pageGame[] pageGames;
    public JsonPagesGames(pageGame[] games) => pageGames = games;
}

[Serializable]
public class pageGame
{
    public Game[] results;

    public Game[] GetResults => results;
}

[Serializable]
public class Game
{
    public string id;
    public string name;
    public string released;
    public string description_raw;
    [NonSerialized]
    public string description_short;
    public string background_image;

    [NonSerialized]
    public Texture2D backgroundTexture;
    public int metacritic;
    public Platform[] platforms;
    public ApiItem[] genres;

    // Se agrega un método para actualizar las propiedades del juego.
    public void UpdateGame(Game updated)
    {
        // Actualiza las propiedades modificables
        this.name = updated.name;
        this.released = updated.released;
        this.description_raw = updated.description_raw;
        this.background_image = updated.background_image;
        this.metacritic = updated.metacritic;
        this.platforms = updated.platforms;
        this.genres = updated.genres;

        // Opcional: se puede actualizar backgroundTexture si es requerido
        this.backgroundTexture = updated.backgroundTexture;
    }

    // Metodos Getter y Setter para obtener y modificar los valores de las propiedades
    public string Id => id;
    public string Name => name;
    public string Released => released;
    public string Description { get { return description_raw; } set { description_raw = value; } }
    public string BackgroundImage => background_image;
    public Texture2D BackgroundTexture => backgroundTexture;

    public int Metacritic => metacritic;
    public Platform[] Platforms => platforms;
    public ApiItem[] Genres => genres;
}
[Serializable]
public class Platform
{
    public ApiItem platform;
    public ApiItem GetPlatform => platform;
}

[Serializable]
public class ApiItem
{
    public string id;
    public string name;
    public string slug;

    // Metodos Get para obtener los valores de las propiedades
    public string Id => id;
    public string Name => name;
    public string Slug => slug;


}
