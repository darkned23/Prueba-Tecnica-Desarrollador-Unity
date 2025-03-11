using TMPro;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UICard : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private TextMeshProUGUI _titleTMP;
    [SerializeField] private TextMeshProUGUI _released;
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private TextMeshProUGUI _metacritic;
    [SerializeField] private TextMeshProUGUI _platforms;
    [SerializeField] private TextMeshProUGUI _genres;

    private int _cardId;
    private Game _videoGameData;
    public int CardId => _cardId;
    public Game VideoGameData => _videoGameData;

    public IEnumerator SetAllCardData(Game videoGame)
    {
        if (videoGame == null) yield break;
        yield return StartCoroutine(SetShortCardData(videoGame));

        _released.text = videoGame.released;
        _description.text = videoGame.description_raw;
        _platforms.text = string.Join(", ", videoGame.platforms.Select(platform => platform.platform.name).ToArray());
        _genres.text = string.Join(", ", videoGame.genres.Select(genre => genre.name).ToArray());
    }

    public IEnumerator SetShortCardData(Game videoGame)
    {
        if (videoGame == null) yield break;

        yield return StartCoroutine(AssignBackground(videoGame));

        _cardId = int.Parse(videoGame.id);
        _videoGameData = videoGame;

        _titleTMP.text = videoGame.name;
        _metacritic.text = videoGame.metacritic.ToString();
    }

    private IEnumerator AssignBackground(Game videoGame)
    {
        yield return ApiRawgManager.Instance.ValidateBackgroundTexture(_videoGameData);
        if (videoGame.background_texture != null)
        {
            _backgroundImage.sprite = Sprite.Create(
                videoGame.background_texture,
                new Rect(0, 0, videoGame.background_texture.width, videoGame.background_texture.height),
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

    // Metodo para asignar los detalles de la carta que se muestra en la pantalla utilizando un boton
    public void InitialAssingCardDetails()
    {

    }

}
