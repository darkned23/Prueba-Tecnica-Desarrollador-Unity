using TMPro;
using UnityEngine;

public class Slot : MonoBehaviour
{
    [Header("Slot Config")]
    [SerializeField] private int _idSlot;
    [SerializeField] private GameObject _buttonDelete;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _numberText;
    [SerializeField] private TMP_Text _textPlayTime;
    [SerializeField] private TMP_Text _textLastPlayedDate;

    [Header("Other Config")]
    [SerializeField] private GameObject _panelInputName;
    [SerializeField] private SceneLoader _sceneLoader;

    void Start()
    {
        AssignData();
    }
    public void AssingSelectSlot()
    {
        GameSaveManager.Instance.SelectedSlot = _idSlot;
    }

    public void SetNameGame(TMP_Text name)
    {
        GameSaveManager.Instance.SelectNameGame = name.text;
    }

    public void SearchSlot()
    {
        AssingSelectSlot();

        if (GameFormatter.LoadGame(_idSlot) != null)
        {
            GameSaveManager.Instance.SelectNameGame = GameFormatter.LoadGame(_idSlot).NameGame;
            GameSaveManager.Instance.LoadGameData();
            _sceneLoader.LoadNextScene();
        }
        else
        {
            GameSaveManager.Instance.IsNewSlot = true;
            _panelInputName.SetActive(true);
        }
    }

    private void AssignData()
    {
        if (GameFormatter.LoadGame(_idSlot) != null)
        {
            LoadData();
        }
        else
        {
            ClearData();
        }
    }

    public void DeleteData()
    {
        GameSaveManager.Instance.DeleteGameData();
    }
    private void LoadData()
    {
        _buttonDelete.SetActive(true);

        _numberText.text = $"{_idSlot + 1}.";
        _nameText.text = GameFormatter.LoadGame(_idSlot).NameGame;
        _textPlayTime.text = FormatPlayTime(GameFormatter.LoadGame(_idSlot).PlayTime);
        _textLastPlayedDate.text = GameFormatter.LoadGame(_idSlot).LastPlayedDate;
    }

    private void ClearData()
    {
        _buttonDelete.SetActive(false);

        _nameText.text = "New Game";
        _numberText.text = "";
        _textPlayTime.text = "";
        _textLastPlayedDate.text = "";
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
        if (deletedSlot == _idSlot)
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