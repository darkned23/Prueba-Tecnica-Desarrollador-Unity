using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using NUnit.Framework;

public class UICard : MonoBehaviour
{
    [Header("UI Components Basic")]
    public Image _backgroundImage;
    public TextMeshProUGUI _titleTMP;
    public TextMeshProUGUI _metacritic;
    public Image _bodyCardImage;
    public bool OnlyUI = false;

    private int _cardId;
    protected Game _videoGameData;
    public int CardId => _cardId;
    public Game VideoGameData => _videoGameData;


    [Header("Color Settings by Metacritic")]
    public Color[] metacriticColors;
    public int[] metacriticThresholds;

    public IEnumerator SetShortCardData(Game videoGame)
    {
        if (videoGame == null) yield break;

        if (OnlyUI)
        {
            AssignColorByMetacritic(videoGame);
        }

        yield return StartCoroutine(AssignBackground(videoGame));

        _cardId = int.Parse(videoGame.id);
        _videoGameData = videoGame;

        _titleTMP.text = videoGame.name;
        _metacritic.text = videoGame.metacritic.ToString();
    }

    private IEnumerator AssignBackground(Game videoGame)
    {
        yield return ApiRawgManager.Instance.ValidateBackgroundTexture(videoGame);
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

    public void InitialAssingCardDetails()
    {
        UICardDetails.Instance.AnimateAndShowDetails(_videoGameData);
    }

    private void AssignColorByMetacritic(Game videoGame)
    {
        if (_bodyCardImage == null || videoGame == null || metacriticColors == null || metacriticThresholds == null)
            return;

        int score = videoGame.metacritic;
        int colorIndex = 0;

        for (int i = 0; i < metacriticThresholds.Length; i++)
        {
            if (score >= metacriticThresholds[i])
                colorIndex = i + 1;
            else
                break;
        }
        if (colorIndex >= metacriticColors.Length)
            colorIndex = metacriticColors.Length - 1;

        _bodyCardImage.color = metacriticColors[colorIndex];
        Debug.Log("Se ha asignado el color: " + metacriticColors[colorIndex]);
    }
}
