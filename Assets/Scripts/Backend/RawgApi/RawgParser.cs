using System.Linq;
using UnityEngine;
public class RawgParser : MonoBehaviour
{
    // Método para parsear la respuesta y obtener la lista completa de juegos.
    public _pageGame ParseResults(string response)
    {
        _pageGame res = JsonUtility.FromJson<_pageGame>(response);
        if (res.results == null || res.results.Length == 0)
        {
            Debug.LogError("No se encontraron juegos en la respuesta.");
        }
        return res;
    }

    // Método para parsear un juego específico.
    public Game ParseGame(string response)
    {
        Game game = JsonUtility.FromJson<Game>(response);
        if (game == null)
        {
            Debug.LogError("No se encontraron datos en la respuesta.");
        }
        return game;
    }

    // Método para crear una descripción genérica si no hay datos de la API
    public string GenerateGenericDescription(Game game)
    {
        string genres = (game.genres != null && game.genres.Length > 0)
            ? string.Join(", ", game.genres.Select(g => g.Name))
            : "various genres";

        string platforms = (game.platforms != null && game.platforms.Length > 0)
            ? string.Join(", ", game.platforms.Select(p => p.platform.Name))
            : "unknown platforms";

        return $"This is a {genres} game, available on {platforms}.";
    }

    // Método para crear una descripción corta
    public string GenerateShortDescription(Game game, int maxLength = 200, int maxAttempts = 10)
    {
        if (string.IsNullOrEmpty(game.description_raw))
        {
            return string.Empty;
        }

        // Si el texto es más corto que el máximo, lo devolvemos completo
        if (game.description_raw.Length <= maxLength)
        {
            game.description_short = game.description_raw;
            return game.description_short;
        }

        // Empezamos buscando desde el límite establecido
        int currentPosition = maxLength;
        int attempts = 0;

        // Mientras no lleguemos al final del texto y no superemos los intentos máximos
        while (currentPosition < game.description_raw.Length && attempts < maxAttempts)
        {
            // Buscamos el punto más cercano antes de la posición actual
            int lastPeriod = game.description_raw.LastIndexOf('.', currentPosition);
            // Buscamos el espacio más cercano antes de la posición actual
            int lastSpace = game.description_raw.LastIndexOf(' ', currentPosition);

            // Si encontramos un punto y está después del último espacio
            if (lastPeriod > 0 && lastPeriod > lastSpace)
            {
                game.description_short = game.description_raw.Substring(0, lastPeriod + 1).Trim();
                return game.description_short;
            }
            // Si encontramos un espacio
            else if (lastSpace > 0)
            {
                game.description_short = game.description_raw.Substring(0, lastSpace).Trim() + "...";
                return game.description_short;
            }

            // Incrementamos la posición y el contador de intentos
            currentPosition++;
            attempts++;
        }

        // Si no encontramos ningún punto de corte adecuado o superamos los intentos, cortamos en el máximo
        game.description_short = game.description_raw.Substring(0, maxLength).Trim() + "...";
        return game.description_short;
    }
}