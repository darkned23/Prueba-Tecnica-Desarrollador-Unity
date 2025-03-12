using UnityEngine;
using System.Collections;

public class ApiRawgManager : MonoBehaviour
{
    public static ApiRawgManager Instance { get; private set; }

    [Header("API Configuration")]
    [SerializeField] private int _maxPage = 10;
    [SerializeField] private _pageGame[] pagesGames;
    [SerializeField] private Game _videoGameSelected;
    [SerializeField] private RawgApiClient _rawgApiClient;
    [SerializeField] private RawgParser _rawgparser;

    private bool isReadyAllGames = false;
    private bool isReadyDataGame = false;


    // Métodos Getter para obtener los valores de las propiedades.
    public Game GetGame() => _videoGameSelected;
    public bool IsReadyAllGames() => isReadyAllGames;
    public bool IsReadyDataGame() => isReadyDataGame;
    public int GetMaxPage() => _maxPage;

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
    private IEnumerator Start()
    {
        if (RawgFormatter.LoadGames() != null)
        {
            pagesGames = RawgFormatter.LoadGames();

            if (pagesGames.Length == _maxPage)
            {
                isReadyAllGames = true;
            }
            else
            {
                GetAllGames();
                yield return new WaitUntil(() => isReadyAllGames);
                RawgFormatter.SaveGames(pagesGames);
            }
        }
        else
        {
            GetAllGames();
            yield return new WaitUntil(() => isReadyAllGames);
            RawgFormatter.SaveGames(pagesGames);
        }
    }

    // Método para obtener todos los juegos de la API.
    public void GetAllGames()
    {
        pagesGames = new _pageGame[_maxPage];
        StartCoroutine(GenerateAllGames());
    }

    // Método para generar todos los juegos de la API bajo los parametros dados.
    private IEnumerator GenerateAllGames()
    {
        for (int i = 0; i < _maxPage; i++)
        {
            _rawgApiClient.GetResultsResponse(i + 1);
            yield return new WaitUntil(() => _rawgApiClient.GetResponse() != null);

            pagesGames[i] = _rawgparser.ParseResults(_rawgApiClient.GetResponse());
            yield return new WaitUntil(() => pagesGames[i] != null);
        }

        isReadyAllGames = true;
    }

    // Método para obtener un juego aleatorio de la pagesGames y su descripción.
    public IEnumerator GenerateDataGame()
    {
        // Seleccionar una página aleatoria y un juego aleatorio de esa página
        int randomPage = Random.Range(0, _maxPage);
        int randomIndex = Random.Range(0, pagesGames[randomPage].results.Length);

        // Seleccionar un juego aleatorio de la página aleatoria
        SelectGame(randomPage, randomIndex);

        // Obtener la descripción del juego
        yield return ValidateDescription();

        // Guardar los datos del juego
        SaveGameData(randomPage, randomIndex);
        isReadyDataGame = true;
    }

    // Método para seleccionar un juego aleatorio de la lista de juegos.
    private void SelectGame(int randomPage, int randomIndex)
    {
        isReadyDataGame = false;
        _videoGameSelected = GetRandomGame(pagesGames[randomPage], randomIndex);
    }

    // Método para asegurar que se asigna una descripción al juego seleccionado.
    private IEnumerator ValidateDescription()
    {
        // Si ya tiene una descripción, no hacer la petición
        if (!string.IsNullOrEmpty(_videoGameSelected.description_raw))
        {
            // Si la descripción corta es nula, generarla
            if (string.IsNullOrEmpty(_videoGameSelected.description_short))
            {
                _videoGameSelected.description_short = _rawgparser.GenerateShortDescription(_videoGameSelected);
            }
            yield break;
        }

        _rawgApiClient.GetGameResponse(_videoGameSelected.id);

        // Esperar la respuesta de la API con un timeout de 5 segundos (usando tiempo en tiempo real)
        float timeout = Time.realtimeSinceStartup + 5f;
        yield return new WaitUntil(() => _rawgApiClient.GetResponse() != null || Time.realtimeSinceStartup > timeout);

        // Verificar si la API respondió a tiempo
        if (_rawgApiClient.GetResponse() != null)
        {
            _videoGameSelected = _rawgparser.ParseGame(_rawgApiClient.GetResponse());
            _videoGameSelected.description_short = _rawgparser.GenerateShortDescription(_videoGameSelected);
        }

        // Si la descripción sigue siendo null o vacía, asignar una genérica
        if (string.IsNullOrEmpty(_videoGameSelected.description_raw))
        {
            _videoGameSelected.description_short = _rawgparser.GenerateGenericDescription(_videoGameSelected);
        }
    }

    // Método para asegurar que se asigna una imagen de fondo al juego seleccionado.
    public IEnumerator ValidateBackgroundTexture(Game videoGame)
    {
        // Verificar si ya tiene una imagen asignada en memoria
        if (videoGame.background_texture != null)
        {
            yield break;
        }

        // Verificar si la imagen ya está almacenada localmente
        if (RawgFormatter.IsImageStored(videoGame.id))
        {
            videoGame.background_texture = RawgFormatter.LoadImage(videoGame.id);
            yield break;
        }

        // Si no está almacenada, hacer la petición
        if (!string.IsNullOrEmpty(videoGame.background_image))
        {
            _rawgApiClient.GetBackgroundTexture(videoGame.background_image, videoGame);
        }
        else
        {
            Debug.LogWarning("No hay imagen disponible, Asignando imagen genérica.");
            videoGame.background_texture = Resources.Load<Texture2D>("Images/Backgrounds/DefaultBackground");
            yield break;
        }

        // Esperar hasta que la imagen se asigne y almacenarla
        yield return new WaitUntil(() => videoGame.background_texture != null);

        // Guardar la imagen descargada
        RawgFormatter.SaveImage(videoGame.id, videoGame.background_texture);
    }

    // Método para guardar los datos del juego seleccionado.
    private void SaveGameData(int randomPage, int randomIndex)
    {
        pagesGames[randomPage].results[randomIndex].UpdateGame(_videoGameSelected);
        RawgFormatter.SaveGames(pagesGames);
    }

    // Método para seleccionar un juego aleatorio de la lista y retornar un Game.
    public Game GetRandomGame(_pageGame results, int randomIndex)
    {
        Game game = results.results[randomIndex];

        return game;
    }
}