using UnityEngine;
using System;

public class GameSaveManager : MonoBehaviour
{
    public static GameSaveManager Instance { get; private set; }

    public static event Action<int> OnGameDataDeleted; // Evento para notificar eliminación

    [SerializeField] private GameData initialGameData; // Datos iniciales de la partida
    [SerializeField] private float saveDelay = 60f; // Tiempo en segundos entre guardados automáticos

    public GameData currentGameData; // Datos de la partida actual(Esta variable solo es una comprobacion de que todo está funcionando correctamente, se borrará luego)
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
        GameSave.SaveGame(initialGameData, idSlot);

        currentGameData = initialGameData;
        Debug.Log("Partida creada.");
    }
    public void SaveGameData(PlayerData player)
    {
        GameData gameData = new("Edward", player.GetPlayerPosition(), player.GetPlayerRotation(), player.PlayTime, "");
        GameSave.SaveGame(gameData, currentSlot);

        currentGameData = gameData;
    }

    public GameData LoadGameData()
    {
        GameData gameData = GameSave.LoadGame(currentSlot);
        if (gameData == null)
        {
            Debug.LogWarning("No se encontró partida guardada o se produjo un error.");
        }

        currentGameData = gameData;
        Debug.Log("Partida cargada.");
        return gameData;
    }

    public void DeleteGameData()
    {
        GameSave.DeleteGame(currentSlot);
        OnGameDataDeleted?.Invoke(currentSlot); // Notificar suscriptores
    }

    public void InitialGame()
    {
        if (GameSave.LoadGame(currentSlot) == null)
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
