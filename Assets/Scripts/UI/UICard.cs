using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UICard : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private TextMeshProUGUI _titleTMP;
    [SerializeField] private TextMeshProUGUI _descriptionTMP;
    [SerializeField] private TextMeshProUGUI _releasedTMP;
    [SerializeField] private TextMeshProUGUI _metacriticText;

    private int _cardId;
    private Game _videoGameData;
    public int CardId => _cardId;
    public Game VideoGameData => _videoGameData;

    public IEnumerator SetAllCardData(Game videoGame)
    {
        if (videoGame == null) yield break;

        _cardId = int.Parse(videoGame.id);

        _videoGameData = videoGame;
        _titleTMP.text = videoGame.name;
        _descriptionTMP.text = !string.IsNullOrEmpty(videoGame.description_short) ? videoGame.description_short : "No description available.";
        _releasedTMP.text = videoGame.released;
        _metacriticText.text = videoGame.metacritic.ToString();
        yield return StartCoroutine(AssignBackground(videoGame));
    }

    public IEnumerator SetShortCardData(Game videoGame)
    {
        if (videoGame == null)
        {
            Debug.Log("Se ha limpiado el videogame data");
            Debug.LogError("Revisar porque se esta elimiando el videogame");
            yield break;
        }

        _cardId = int.Parse(videoGame.id);
        _videoGameData = videoGame;

        _titleTMP.text = videoGame.name;
        _metacriticText.text = videoGame.metacritic.ToString();
        StartCoroutine(AssignBackground(videoGame));
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
        StartCoroutine(UICardDetails.Instance.SetAllCardData(_videoGameData));
    }

}
