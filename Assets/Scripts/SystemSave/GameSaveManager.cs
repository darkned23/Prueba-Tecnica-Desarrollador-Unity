using System;
using UnityEngine;

public class GameSaveManager : MonoBehaviour
{
    public static GameSaveManager Instance { get; private set; }
    public static event Action<int> OnGameDataDeleted; // Evento para notificar eliminación

    [SerializeField] private GameData initialGameData; // Datos iniciales de la partida
    [SerializeField] private float saveDelay = 60f; // Tiempo en segundos entre guardados automáticos

    private int currentSlot;
    private bool isNewSlot;
    public int CurrentSlot { get => currentSlot; set => currentSlot = value; }
    public bool IsNewSlot { get => isNewSlot; set => isNewSlot = value; }
    public float SaveDelay { get => saveDelay; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CreateGameData(int idSlot)
    {
        GameFormatter.SaveGame(initialGameData, idSlot);
        Debug.Log("Partida creada.");
    }
    public void SaveGameData(PlayerData playerData)
    {
        GameData gameData = new("Edward", playerData.GetPlayerPosition(), playerData.GetPlayerRotation(), playerData.PlayTime, playerData.GetGames());
        GameFormatter.SaveGame(gameData, currentSlot);
    }

    public GameData LoadGameData()
    {
        GameData gameData = GameFormatter.LoadGame(currentSlot);
        if (gameData == null)
        {
            Debug.LogWarning("No se encontró partida guardada o se produjo un error.");
        }
        Debug.Log("Partida cargada.");
        return gameData;
    }

    public void DeleteGameData()
    {
        GameFormatter.DeleteGame(currentSlot);
        OnGameDataDeleted?.Invoke(currentSlot); // Notificar suscriptores
    }

    public void InitialGame()
    {
        if (GameFormatter.LoadGame(currentSlot) == null)
        {
            CreateGameData(currentSlot);
            isNewSlot = true;
        }
        else
        {
            LoadGameData();
        }
    }
}
