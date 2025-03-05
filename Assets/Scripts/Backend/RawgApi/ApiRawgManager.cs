using UnityEngine;
using System.Collections;

public class ApiRawgManager : MonoBehaviour
{
    [SerializeField] private int maxPage = 10;
    [SerializeField] private pageGame[] pagesGames;
    [SerializeField] private Game _gameSelected;
    [SerializeField] private RawgApiClient rawgApiClient;
    [SerializeField] private RawgParser rawgparser;

    private bool isReadyAllGames = false;
    private bool isReadyDataGame = false;
    public static ApiRawgManager instance;


    // Métodos Getter para obtener los valores de las propiedades.
    public Game GetGame() => _gameSelected;
    public bool IsReadyAllGames() => isReadyAllGames;
    public bool IsReadyDataGame() => isReadyDataGame;
    public int GetMaxPage() => maxPage;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private IEnumerator Start()
    {
        if (SaveManager.LoadGames() != null)
        {
            pagesGames = SaveManager.LoadGames();

            if (pagesGames.Length == maxPage)
            {
                isReadyAllGames = true;
                Debug.Log("Todos los juegos han sido cargados y no se ha realizado ninguna petición.");
            }
            else
            {
                GetAllGames();
                yield return new WaitUntil(() => isReadyAllGames);
                SaveManager.SaveGames(pagesGames);
                Debug.Log("Se ha actualizado la lista de juegos.");
            }
        }
        else
        {
            GetAllGames();
            yield return new WaitUntil(() => isReadyAllGames);
            SaveManager.SaveGames(pagesGames);
            Debug.Log("Todos los juegos han sido guardados.");
        }
    }

    // Método para obtener todos los juegos de la API.
    public void GetAllGames()
    {
        pagesGames = new pageGame[maxPage];
        StartCoroutine(GenerateAllGames());
    }

    // Método para generar todos los juegos de la API bajo los parametros dados.
    private IEnumerator GenerateAllGames()
    {
        // Corregir el índice: usar de 0 a maxPage - 1
        for (int i = 0; i < maxPage; i++)
        {
            // Se solicita la página (asumiendo que la API utiliza 1-based para el parámetro)
            rawgApiClient.GetResultsResponse(i + 1);
            yield return new WaitUntil(() => rawgApiClient.GetResponse() != null);

            // Parsear y asignar el resultado al array
            pagesGames[i] = rawgparser.ParseResults(rawgApiClient.GetResponse());
            yield return new WaitUntil(() => pagesGames[i] != null);
        }

        isReadyAllGames = true;
        Debug.Log("Todos los juegos han sido generados.");
    }

    // Método para obtener un juego aleatorio de la pagesGames y su descripción.
    public IEnumerator GenerateDataGame()
    {

        // Seleccionar una página aleatoria y un juego aleatorio de esa página
        int randomPage = Random.Range(0, maxPage);
        int randomIndex = Random.Range(0, pagesGames[randomPage].results.Length);

        // Seleccionar un juego aleatorio de la página aleatoria
        SelectGame(randomPage, randomIndex);

        // Obtener la descripción del juego
        yield return ValidateDescription();
        // Obtener la imagen de fondo del juego
        yield return ValidateBackgroundTexture(_gameSelected.id);

        // Guardar los datos del juego
        SaveGameData(randomPage, randomIndex);
        isReadyDataGame = true;
    }

    // Método para seleccionar un juego aleatorio de la lista de juegos.
    private void SelectGame(int randomPage, int randomIndex)
    {
        isReadyDataGame = false;
        _gameSelected = GetRandomGame(pagesGames[randomPage], randomIndex);
        Debug.Log("Se ha seleccionado un juego de la página " + randomPage + " con id: " + _gameSelected.id);
    }

    // Método para asegurar que se asigna una descripción al juego seleccionado.
    private IEnumerator ValidateDescription()
    {
        // Si ya tiene una descripción, no hacer la petición
        if (!string.IsNullOrEmpty(_gameSelected.description_raw))
        {
            Debug.Log("El juego ya tiene una descripción asignada.");

            // Si la descripción corta es nula, generarla
            if (string.IsNullOrEmpty(_gameSelected.description_short))
            {
                _gameSelected.description_short = rawgparser.GenerateShortDescription(_gameSelected);
            }
            yield break;
        }

        Debug.Log("Obteniendo la descripción desde la API...");
        rawgApiClient.GetGameResponse(_gameSelected.id);

        // Esperar la respuesta de la API con un timeout de 5 segundos
        float timeout = Time.time + 5f;
        yield return new WaitUntil(() => rawgApiClient.GetResponse() != null || Time.time > timeout);

        // Verificar si la API respondió a tiempo
        if (rawgApiClient.GetResponse() != null)
        {
            _gameSelected = rawgparser.ParseGame(rawgApiClient.GetResponse());
            _gameSelected.description_short = rawgparser.GenerateShortDescription(_gameSelected);
        }

        // Si la descripción sigue siendo null o vacía, asignar una genérica
        if (string.IsNullOrEmpty(_gameSelected.description_raw))
        {
            _gameSelected.description_short = rawgparser.GenerateGenericDescription(_gameSelected);
            Debug.LogWarning("Se asignó una descripción genérica.");
        }
    }

    // Método para asegurar que se asigna una imagen de fondo al juego seleccionado.
    private IEnumerator ValidateBackgroundTexture(string gameId)
    {
        // 1. Si ya tiene una imagen en memoria, salir
        if (_gameSelected.backgroundTexture != null)
        {
            Debug.Log("El juego ya tiene una imagen de fondo asignada.");
            yield break;
        }

        // 2. Verificar si la imagen ya está almacenada localmente
        if (SaveManager.IsImageStored(gameId))
        {
            Debug.Log("Cargando imagen almacenada localmente.");
            _gameSelected.backgroundTexture = SaveManager.LoadImage(gameId);
            yield break;
        }

        // 3. Si no está almacenada, hacer la petición
        if (!string.IsNullOrEmpty(_gameSelected.background_image))
        {
            Debug.Log("Seleccionar la background_image si está disponible.");
            rawgApiClient.GetBackgroundTexture(_gameSelected.background_image, _gameSelected);
        }
        else
        {
            Debug.LogWarning("No hay imagen disponible, asignando imagen genérica.");
            _gameSelected.backgroundTexture = Resources.Load<Texture2D>("Images/Backgrounds/DefaultBackground");
            yield break;
        }

        // 4. Esperar hasta que la imagen se asigne y almacenarla
        yield return new WaitUntil(() => _gameSelected.backgroundTexture != null);

        // 5. Guardar la imagen descargada
        SaveManager.SaveImage(gameId, _gameSelected.backgroundTexture);
    }

    // Método para guardar los datos del juego seleccionado.
    private void SaveGameData(int randomPage, int randomIndex)
    {
        pagesGames[randomPage].results[randomIndex].UpdateGame(_gameSelected);
        SaveManager.SaveGames(pagesGames);

        Debug.Log("Se ha actualizado la pagesGames con la descripción");
    }

    // Método para seleccionar un juego aleatorio de la lista y retornar un Game.
    public Game GetRandomGame(pageGame results, int randomIndex)
    {
        Game game = results.results[randomIndex];

        return game;
    }
}