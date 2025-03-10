using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadSceneByIndex(int index)
    {
        if (index >= 0 && index < SceneManager.sceneCountInBuildSettings)
        {
            StartCoroutine(LoadSceneAsync(index));
        }
        else
        {
            Debug.LogError($"Indice de escena {index} fuera de rango.");
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
            Debug.LogError($"La escena '{sceneName}' no esta en la lista de compilacion.");
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
            timer = Time.unscaledDeltaTime;
            // Agregar barra de carga luego aquí
            yield return null;
        }
    }

    public void ExitGame()
    {
        Debug.Log("Saliendo del juego");
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

}
