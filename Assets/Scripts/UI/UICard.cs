using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UICard : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI _titleTMP;
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private TextMeshProUGUI _descriptionTMP;
    [SerializeField] private TextMeshProUGUI _releasedTMP;
    [SerializeField] private TextMeshProUGUI _metacriticText;

    private int _cardId;
    private Game _videoGameData;
    public int CardId => _cardId;
    public Game VideoGameData => _videoGameData;

    public IEnumerator SetAllCardData(Game game)
    {
        if (game == null) yield break;

        _cardId = int.Parse(game.id);

        _videoGameData = game;
        _titleTMP.text = game.name;
        _descriptionTMP.text = !string.IsNullOrEmpty(game.description_short) ? game.description_short : "No description available.";
        _releasedTMP.text = game.released;
        _metacriticText.text = game.metacritic.ToString();
        StartCoroutine(AssignBackground(game));
    }

    private IEnumerator AssignBackground(Game game)
    {
        yield return ApiRawgManager.Instance.ValidateBackgroundTexture(_videoGameData);
        if (game.background_texture != null)
        {
            _backgroundImage.sprite = Sprite.Create(
                game.background_texture,
                new Rect(0, 0, game.background_texture.width, game.background_texture.height),
                new Vector2(0.5f, 0.5f)
            );
            _backgroundImage.preserveAspect = true;
            _backgroundImage.color = Color.white;
        }
        else
        {
            _backgroundImage.sprite = null;
        }
    }

    public IEnumerator SetShortCardData(Game game)
    {
        if (game == null) yield break;

        _cardId = int.Parse(game.id);
        _videoGameData = game;

        _titleTMP.text = game.name;
        _metacriticText.text = game.metacritic.ToString();

        // Obtener la imagen de fondo del juego
        yield return ApiRawgManager.Instance.ValidateBackgroundTexture(_videoGameData);
        if (game.background_texture != null)
        {
            _backgroundImage.sprite = Sprite.Create(
                game.background_texture,
                new Rect(0, 0, game.background_texture.width, game.background_texture.height),
                new Vector2(0.5f, 0.5f)
            );
            _backgroundImage.preserveAspect = true;
            _backgroundImage.color = Color.white;
        }
        else
        {
            _backgroundImage.sprite = null;
        }
    }
}
