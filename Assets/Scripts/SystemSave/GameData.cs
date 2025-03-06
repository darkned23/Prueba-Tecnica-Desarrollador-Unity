using System;

[Serializable] // Permite la serialización
public class GameData
{
    public String playerName;
    public float[] playerPosition = new float[3];
    public float[] playerRotation = new float[3];

    public float playTime; // Tiempo total de juego
    public string lastPlayedDate; // Última fecha de juego en formato string

    public GameData()
    {
        playerName = "";
        playerPosition = new float[3];
        playTime = 0.0f;
        lastPlayedDate = "";
    }

    public GameData(string playerName, float[] playerPosition, float[] playerRotation, float playTime, string lastPlayedDate)
    {
        this.playerName = playerName;
        this.playerPosition = playerPosition;
        this.playerRotation = playerRotation;
        this.playTime = playTime;
        this.lastPlayedDate = lastPlayedDate;
    }

}
