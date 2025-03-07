using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Card : MonoBehaviour
{
    [Header("Card Components")]
    [SerializeField] private GameObject _cardModel;
    [SerializeField] private TextMeshProUGUI _titleTMP;
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private TextMeshProUGUI _descriptionTMP;
    [SerializeField] private TextMeshProUGUI _releasedTMP;
    [SerializeField] private TextMeshProUGUI _metacriticText;
    [SerializeField] private List<string> _platformsTMP;
    [SerializeField] private List<string> _genresTMP;

    private Game videoGameData;
    private Rigidbody _rigidbody;

    public Game VideoGameData => videoGameData;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        StartCoroutine(PopCardEfecct());
    }

    // Método para asignar los datos del juego a la tarjeta
    public void SetCardData(Game game)
    {
        _titleTMP.text = game.name;

        // Asegurarse de que la descripción no sea nula
        if (game.description_short != null)
        {
            _descriptionTMP.text = game.description_short;
        }
        else
        {
            _descriptionTMP.text = "No se ha encontrado una descripción corta.";
        }

        // Asegurarse de que la imagen de fondo no sea nula
        if (game.backgroundTexture != null)
        {
            Sprite sprite = Sprite.Create(game.backgroundTexture, new Rect(0, 0, game.backgroundTexture.width, game.backgroundTexture.height), new Vector2(0.5f, 0.5f));
            _backgroundImage.sprite = sprite;
        }
        else
        {
            _backgroundImage.sprite = null;
        }

        _releasedTMP.text = game.released;
        _metacriticText.text = game.metacritic.ToString();

        // Convertir las plataformas a lista de nombres
        _platformsTMP = new List<string>();
        if (game.platforms != null)
        {
            foreach (var p in game.platforms)
            {
                if (p.platform != null && !_platformsTMP.Contains(p.platform.name))
                {
                    _platformsTMP.Add(p.platform.name);
                }
            }
        }

        // Convertir los géneros a lista de nombres
        _genresTMP = new List<string>();
        if (game.genres != null)
        {
            foreach (var g in game.genres)
            {
                if (!_genresTMP.Contains(g.name))
                {
                    _genresTMP.Add(g.name);
                }
            }
        }

        this.videoGameData = game;
    }

    // Método para animar la tarjeta al spawnear
    private IEnumerator PopCardEfecct()
    {
        Transform goTransform = gameObject.transform;
        goTransform.localScale = Vector3.zero;

        _cardModel.SetActive(true);
        SetCardData(ApiRawgManager.instance.GetGame());

        Vector3 normalScale = Vector3.one;
        Vector3 overshootScale = normalScale * 1.2f;

        float durationToOvershoot = 0.12f;
        float timer = 0f;
        while (timer < durationToOvershoot)
        {
            timer += Time.deltaTime;
            goTransform.localScale = Vector3.Lerp(Vector3.zero, overshootScale, timer / durationToOvershoot);
            yield return null;
        }

        // Animate back from the overshoot scale to the normal scale
        float durationToNormal = 0.08f;
        timer = 0f;
        while (timer < durationToNormal)
        {
            timer += Time.deltaTime;
            goTransform.localScale = Vector3.Lerp(overshootScale, normalScale, timer / durationToNormal);
            yield return null;
        }

        // After the pop effect, set the card data and enable gravity
        _rigidbody.useGravity = true;
    }
}