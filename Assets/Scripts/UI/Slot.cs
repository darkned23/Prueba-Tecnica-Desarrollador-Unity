using TMPro;
using UnityEngine;

public class Slot : MonoBehaviour
{
    [SerializeField] private int idSlot;
    [SerializeField] private GameObject buttonDelete;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text numberText;
    [SerializeField] private TMP_Text progressText;

    void Start()
    {
        AssignData();
    }

    private void AssignData()
    {
        if (GameSave.LoadGame(idSlot) != null)
        {
            LoadData();
        }
        else
        {
            ClearData();
        }
    }

    private void LoadData()
    {
        buttonDelete.SetActive(true);

        nameText.text = GameSave.LoadGame(idSlot).playerName;
        numberText.text = $"{idSlot + 1}.";
        progressText.text = FormatPlayTime(GameSave.LoadGame(idSlot).playTime);
    }

    private void ClearData()
    {
        buttonDelete.SetActive(false);

        nameText.text = "New Game";
        numberText.text = $"";
        progressText.text = "";
    }

    private void OnEnable()
    {
        GameSaveManager.OnGameDataDeleted += OnGameDataDeleted;
    }

    private void OnDisable()
    {
        GameSaveManager.OnGameDataDeleted -= OnGameDataDeleted;
    }

    private void OnGameDataDeleted(int deletedSlot)
    {
        if (deletedSlot == idSlot)
        {
            ClearData();
        }
    }

    private string FormatPlayTime(float playTimeInSeconds)
    {
        int hours = Mathf.FloorToInt(playTimeInSeconds / 3600);
        int minutes = Mathf.FloorToInt((playTimeInSeconds % 3600) / 60);

        if (hours > 0)
        {
            return $"{hours}H {minutes}M";
        }
        return $"{minutes}M";
    }
}