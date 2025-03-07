using System;
using System.Collections.Generic;

[Serializable] // Permite la serialización
public class GameData
{
    public String playerName;
    public float[] playerPosition = new float[3];
    public float[] playerRotation = new float[3];
    public List<Game> videoGamesData = new List<Game>();
    public float playTime;
    public string lastPlayedDate;

    public GameData()
    {
        playerName = "";
        playerPosition = new float[3];
        playTime = 0.0f;
        lastPlayedDate = "";
    }

    public GameData(string playerName, float[] playerPosition, float[] playerRotation, float playTime, List<Game> videoGamesData)
    {
        this.playerName = playerName;
        this.playerPosition = playerPosition;
        this.playerRotation = playerRotation;
        this.playTime = playTime;
        this.lastPlayedDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        this.videoGamesData = videoGamesData;
    }

}
