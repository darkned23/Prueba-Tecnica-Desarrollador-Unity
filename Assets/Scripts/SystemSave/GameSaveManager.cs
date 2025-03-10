using System;
using TMPro;
using UnityEngine;

public class GameSaveManager : MonoBehaviour
{
    public static GameSaveManager Instance { get; private set; }
    public static event Action<int> OnGameDataDeleted;

    [SerializeField] private GameData _initialGameData;
    [SerializeField] private float saveDelay = 60f;

    private int _selectSlot;
    private bool _isNewSlot;
    public int SelectedSlot { get => _selectSlot; set => _selectSlot = value; }
    public bool IsNewSlot { get => _isNewSlot; set => _isNewSlot = value; }
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
        GameFormatter.SaveGame(_initialGameData, idSlot);
        _isNewSlot = true;
        Debug.Log("Partida creada.");
    }
    public void SaveGameData(PlayerData playerData)
    {
        GameData gameData = new(_initialGameData.NameGame, playerData.GetPlayerPosition(), playerData.GetPlayerRotation(), playerData.PlayTime, playerData.GetGames());
        GameFormatter.SaveGame(gameData, _selectSlot);
    }

    public GameData LoadGameData()
    {
        GameData gameData = GameFormatter.LoadGame(_selectSlot);
        if (gameData == null)
        {
            Debug.LogWarning("No se encontró partida guardada o se produjo un error.");
        }
        Debug.Log("Partida cargada.");
        return gameData;
    }

    public void DeleteGameData()
    {
        GameFormatter.DeleteGame(_selectSlot);
        OnGameDataDeleted?.Invoke(_selectSlot); // Notificar suscriptores
    }

    public void InitialGame()
    {
        if (GameFormatter.LoadGame(_selectSlot) == null)
        {
            CreateGameData(_selectSlot);
        }
        else
        {
            LoadGameData();
        }
    }

    public void SetNameGame(TMP_Text name)
    {
        _initialGameData.NameGame = name.text;
    }
}
