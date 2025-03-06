using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

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

    public void LoadSceneByIndex(int index)
    {
        if (index >= 0 && index < SceneManager.sceneCountInBuildSettings)
        {
            StartCoroutine(LoadSceneAsync(index));
        }
        else
        {
            Debug.LogError($"Índice de escena {index} fuera de rango.");
        }
    }

    public void LoadSceneByName(string sceneName)
    {
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            StartCoroutine(LoadSceneAsync(sceneName));
        }
        else
        {
            Debug.LogError($"La escena '{sceneName}' no está en la lista de compilación.");
        }
    }

    public void LoadNextScene()
    {
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        LoadSceneByIndex(nextIndex);
    }

    public void LoadPreviousScene()
    {
        int previousIndex = SceneManager.GetActiveScene().buildIndex - 1;
        LoadSceneByIndex(previousIndex);
    }

    public void ReloadCurrentScene()
    {
        LoadSceneByIndex(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator LoadSceneAsync(object sceneIdentifier)
    {
        AsyncOperation operation;

        if (sceneIdentifier is int index)
        {
            operation = SceneManager.LoadSceneAsync(index);
        }
        else if (sceneIdentifier is string name)
        {
            operation = SceneManager.LoadSceneAsync(name);
        }
        else
        {
            yield break;
        }

        float timer;
        while (!operation.isDone)
        {
            timer = Time.deltaTime;
            Debug.Log(timer);
            // Agregar barra de carga luego aquí
            yield return null;
        }
    }
}
