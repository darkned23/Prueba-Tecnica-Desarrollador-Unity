using System;
using UnityEngine;

public class GameSaveManager : MonoBehaviour
{
    public static GameSaveManager Instance { get; private set; }
    public static event Action<int> OnGameDataDeleted;

    [SerializeField] private GameData _initialGameData = new();
    [SerializeField] private float saveDelay = 60f;
    public GameData CurrentGamgeData;
    private int _selectSlot;
    private string _selectNameGame;
    private bool _isNewSlot;
    public int SelectedSlot { get => _selectSlot; set => _selectSlot = value; }
    public string SelectNameGame { get => _selectNameGame; set => _selectNameGame = value; }
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

    public void CreateGameData()
    {
        _initialGameData.NameGame = _selectNameGame;

        GameFormatter.SaveGame(_initialGameData, _selectSlot);
        _isNewSlot = true;

        CurrentGamgeData = _initialGameData;
    }

    public void SaveGameData(PlayerData playerData)
    {
        _initialGameData.NameGame = _selectNameGame;

        GameData gameData = new(
            _initialGameData.NameGame,
            playerData.GetPlayerPosition(),
            playerData.GetPlayerRotation(),
            playerData.PlayTime,
            playerData.GetVideoGamesData());

        GameFormatter.SaveGame(gameData, _selectSlot);

        CurrentGamgeData = gameData;
    }

    public GameData LoadGameData()
    {
        GameData gameData = GameFormatter.LoadGame(_selectSlot);
        if (gameData == null)
        {
            Debug.LogWarning("No se encontró partida guardada o se produjo un error.");
        }
        Debug.Log("Partida cargada.");

        CurrentGamgeData = gameData;
        return gameData;
    }

    public void DeleteGameData()
    {
        GameFormatter.DeleteGame(_selectSlot);
        OnGameDataDeleted?.Invoke(_selectSlot); // Notificar suscriptores
    }
}
