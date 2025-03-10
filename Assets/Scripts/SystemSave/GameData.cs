using System;
using System.Collections.Generic;

[Serializable] // Permite la serialización
public class GameData
{
    public string NameGame;
    public float[] PlayerPosition = new float[3];
    public float[] PlayerRotation = new float[3];
    public List<Game> videoGamesData = new();
    public float playTime;
    public string lastPlayedDate;

    public GameData()
    {
        NameGame = "";
        PlayerPosition = new float[3];
        playTime = 0.0f;
        lastPlayedDate = "";
    }

    public GameData(string nameGame, float[] playerPosition, float[] playerRotation, float playTime, List<Game> videoGamesData)
    {
        this.NameGame = nameGame;
        this.PlayerPosition = playerPosition;
        this.PlayerRotation = playerRotation;
        this.playTime = playTime;
        lastPlayedDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
        this.videoGamesData = videoGamesData;
    }

}
