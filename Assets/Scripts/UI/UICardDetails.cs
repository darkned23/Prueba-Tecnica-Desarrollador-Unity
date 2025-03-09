using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class UICardDetails : MonoBehaviour
{
    public static UICardDetails Instance { get; private set; }

    [Header("UI Components")]
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private TextMeshProUGUI _titleTMP;
    [SerializeField] private TextMeshProUGUI _released;
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private TextMeshProUGUI _metacritic;
    [SerializeField] private TextMeshProUGUI _platforms;
    [SerializeField] private TextMeshProUGUI _genres;
    private Game _videoGame;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator SetAllCardData(Game game)
    {
        if (game == null) yield break;
        _videoGame = game;

        _titleTMP.text = game.name;
        yield return AssignBackground(game);
        _released.text = game.released;
        _description.text = game.description_short;
        _metacritic.text = game.metacritic.ToString();
        _platforms.text = string.Join(", ", game.platforms.Select(platform => platform.platform.name).ToArray());
        _genres.text = string.Join(", ", game.genres.Select(genre => genre.name).ToArray());

    }

    private IEnumerator AssignBackground(Game game)
    {
        yield return ApiRawgManager.Instance.ValidateBackgroundTexture(game);
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