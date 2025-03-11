using TMPro;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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

    public IEnumerator SetAllCardData(Game videoGame)
    {
        if (videoGame == null) yield break;
        _videoGame = videoGame;

        _titleTMP.text = videoGame.name;
        _released.text = videoGame.released;
        _description.text = videoGame.description_raw;
        _metacritic.text = videoGame.metacritic.ToString();
        _platforms.text = string.Join(", ", videoGame.platforms.Select(platform => platform.platform.name).ToArray());
        _genres.text = string.Join(", ", videoGame.genres.Select(genre => genre.name).ToArray());
        yield return StartCoroutine(AssignBackground(videoGame));

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