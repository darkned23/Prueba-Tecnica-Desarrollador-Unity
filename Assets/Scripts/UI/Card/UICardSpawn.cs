using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class UICardSpawn : UICard
{
    [Header("Others Components")]
    public TextMeshProUGUI _released;
    public TextMeshProUGUI _description;
    public TextMeshProUGUI _platforms;
    public TextMeshProUGUI _genres;
    public MeshRenderer _meshRenderer;

    private void Start()
    {
        StartCoroutine(SetAllCardData(ApiRawgManager.Instance.GetGame()));
    }

    public IEnumerator SetAllCardData(Game videoGame)
    {
        if (videoGame == null || _videoGameData == videoGame) yield break;

        yield return StartCoroutine(SetShortCardData(videoGame));

        AssignMaterialByMetacritic(videoGame);
        yield return null;

        _released.text = videoGame.released;
        _description.text = videoGame.description_raw;
        yield return null;

        _platforms.text = string.Join(", ", videoGame.platforms.Select(platform => platform.platform.name).ToArray());
        yield return null;

        _genres.text = string.Join(", ", videoGame.genres.Select(genre => genre.name).ToArray());
        yield return null;
    }

    private void AssignMaterialByMetacritic(Game videoGame)
    {
        if (_meshRenderer == null || videoGame == null || metacriticColors == null || metacriticThresholds == null)
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

        _meshRenderer.material.color = metacriticColors[colorIndex];
        Debug.Log("Se ha asignado el color: " + metacriticColors[colorIndex]);
    }
}
