using System;
using System.Collections.Generic;

[Serializable] // Permite la serialización
public class GameData
{
    public string NameGame;
    public float[] PlayerPosition = new float[3];
    public float[] PlayerRotation = new float[3];
    public List<Game> VideoGamesData = new();
    public float PlayTime;
    public string LastPlayedDate;

    public GameData()
    {
        NameGame = "";
        PlayerPosition = new float[3];
        PlayTime = 0.0f;
        LastPlayedDate = "";
    }

    public GameData(string nameGame, float[] playerPosition, float[] playerRotation, float playTime, List<Game> videoGamesData)
    {
        NameGame = nameGame;
        PlayerPosition = playerPosition;
        PlayerRotation = playerRotation;
        PlayTime = playTime;
        LastPlayedDate = DateTime.Now.ToString("dd/MM/yyyy");
        VideoGamesData = videoGamesData;
    }

}
