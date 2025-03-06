using UnityEngine;

public class Player : MonoBehaviour
{
    private float playTime = 0f;
    private float timeUntilNextSave = 0f;

    public float PlayTime => playTime;

    private void Awake()
    {
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
        timeUntilNextSave += Time.deltaTime;
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

        playTime = gameData.playTime;
        transform.position = new Vector3(gameData.playerPosition[0], gameData.playerPosition[1], gameData.playerPosition[2]);
        transform.rotation = Quaternion.Euler(gameData.playerRotation[0], gameData.playerRotation[1], gameData.playerRotation[2]);
    }

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
}