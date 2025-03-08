using System.IO;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class RawgApiClient : MonoBehaviour
{
    private string _apiKey;
    private string _apiUrl;
    private string _envPath = Application.dataPath + "/../config/.env";
    private string _response;

    void Awake()
    {
        LoadEnvVariables();
    }

    // Método para cargar las variables de entorno desde un archivo .env
    private void LoadEnvVariables()
    {
        if (!File.Exists(_envPath))
        {
            Debug.LogError(".env file not found at: " + _envPath);
            return;
        }

        string[] lines = File.ReadAllLines(_envPath);
        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line) || line.Trim().StartsWith("#"))
            {
                continue;
            }

            string[] parts = line.Split(new char[] { '=' }, 2);
            if (parts.Length != 2)
                continue;
            string key = parts[0].Trim();
            string value = parts[1].Trim();
            if (key == "RAWGAPI_KEY")
                _apiKey = value;
            else if (key == "RAWGAPI_URL")
                _apiUrl = value;
        }
    }

    // Método para obtener los datos de un juego de la API.
    public void GetResultsResponse(int pageId)
    {
        _response = null;
        string url = BuildPagesGamesUrl(pageId);
        StartCoroutine(MakeRequest(url));
    }

    // Método para obtener los datos de un juego específico de la API.
    public void GetGameResponse(string idGame)
    {
        _response = null;
        string url = BuildGameUrl(idGame);
        StartCoroutine(MakeRequest(url));
    }

    // Método para Obtener la imagen de fondo de un juego.
    public void GetBackgroundTexture(string url, Game game)
    {
        StartCoroutine(DownloadBackgroundTexture(url, game));
    }

    // Método para construir la URL de la API, modificado para obtener un juego aleatorio entre los 500 mejor puntuados.
    public string BuildPagesGamesUrl(int pageId)
    {
        if (string.IsNullOrEmpty(_apiUrl))
        {
            Debug.LogError("_apiUrl es nulo o vacío. Verifica el archivo .env.");
            return "";
        }

        string url = _apiUrl
                     + "?key=" + _apiKey
                     + "&exclude_additions=true"
                     + "&ordering=-metacritic"
                     + "&page_size=40"
                     + "&page=" + pageId;
        return url;
    }

    // Método para construir la URL de la API para obtener un juego específico.
    public string BuildGameUrl(string idGame)
    {
        string url = _apiUrl + "/" + idGame + "?key=" + _apiKey;
        return url;
    }

    // Coroutine para realizar la petición a la API y obtener los datos.
    private IEnumerator MakeRequest(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            _response = request.downloadHandler.text;
        }
    }

    // Coroutine para realizar la petición a la API y obtener la imagen de fondo del juego.
    private IEnumerator DownloadBackgroundTexture(string url, Game game)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error al descargar la imagen: " + request.error);
        }
        else
        {
            Texture2D _backgroundTexture = DownloadHandlerTexture.GetContent(request);
            game.background_texture = _backgroundTexture;
            Debug.Log("Se ha descargado la imagen de fondo del juego con el id: " + game.id);
        }
    }

    // Método para obtener la respuesta de la API.
    public string GetResponse()
    {
        return _response;
    }
}