using UnityEngine;
using System.Collections.Generic;
using System;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance { get; private set; }
    private float playTime = 0f;
    private float timeUntilNextSave = 0f;
    public List<Game> videoGamesData;

    public event Action<Game> OnVideoGameAdded;
    public event Action<Game> OnVideoGameRemoved;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (GameSaveManager.Instance.IsNewSlot)
        {
            SaveGame();
            GameSaveManager.Instance.IsNewSlot = false;
        }
        else
        {
            LoadGame();
        }
    }

    private void Update()
    {
        playTime += Time.deltaTime;
        AutoSaveGame();
    }

    private void AutoSaveGame()
    {
        timeUntilNextSave += Time.unscaledDeltaTime;
        if (timeUntilNextSave >= GameSaveManager.Instance.SaveDelay)
        {
            SaveGame();
            timeUntilNextSave = 0f;
            Debug.Log($"Autoguardado realizado a los {GameSaveManager.Instance.SaveDelay} segundos");
        }
    }

    public void SaveGame()
    {
        GameSaveManager.Instance.SaveGameData(this);
    }

    public void LoadGame()
    {
        GameData gameData = GameSaveManager.Instance.LoadGameData();
        if (gameData == null)
        {
            Debug.LogWarning("GameData is null in PlayerData.LoadGame. Loading default values.");
            return; // Evita errores por referencia nula
        }

        playTime = gameData.PlayTime;
        videoGamesData = gameData.VideoGamesData;
        transform.position = new Vector3(gameData.PlayerPosition[0], gameData.PlayerPosition[1], gameData.PlayerPosition[2]);
        transform.rotation = Quaternion.Euler(gameData.PlayerRotation[0], gameData.PlayerRotation[1], gameData.PlayerRotation[2]);
    }

    public void AddVideoGame(Game videoGameData)
    {
        if (videoGamesData == null)
        {
            videoGamesData = new List<Game>();
        }
        videoGamesData.Add(videoGameData);
        SaveGame();

        OnVideoGameAdded?.Invoke(videoGameData);
    }

    public void RemoveVideoGame(Game videoGameData)
    {
        if (videoGamesData == null) return;

        videoGamesData.Remove(videoGameData);
        SaveGame();

        OnVideoGameRemoved?.Invoke(videoGameData);
    }

    #region Getters and Setters
    public float PlayTime => playTime;
    public List<Game> VideoGameData => videoGamesData;

    public float[] GetPlayerPosition()
    {
        float[] position = new float[3];

        position[0] = transform.position.x;
        position[1] = transform.position.y;
        position[2] = transform.position.z;

        return position;
    }
    public float[] GetPlayerRotation()
    {
        float[] rotation = new float[3];

        rotation[0] = transform.rotation.eulerAngles.x;
        rotation[1] = transform.rotation.eulerAngles.y;
        rotation[2] = transform.rotation.eulerAngles.z;

        return rotation;
    }

    public List<Game> GetVideoGamesData()
    {
        return videoGamesData;
    }
    #endregion

}