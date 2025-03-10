using System;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;

public class Invetory : MonoBehaviour
{
    [SerializeField] private GameObject _uICardPrefab;
    [SerializeField] private Transform _cardContainer;
    [SerializeField] private float _delayUpdateInventory = 0.1f;

    private List<UICard> _cardsCanvas = new();

    private void Start()
    {
        Debug.Log("Inventario iniciado");

        if (PlayerData.Instance != null)
        {
            PlayerData.Instance.OnVideoGameAdded += HandleVideoGameAdded;
            PlayerData.Instance.OnVideoGameRemoved += HandleVideoGameRemoved;

            if (PlayerData.Instance.VideoGameData != null)
            {
                AddAllCardsAsync(PlayerData.Instance.VideoGameData);
            }
        }
    }

    private async void AddAllCardsAsync(List<Game> videoGames)
    {
        foreach (Game videoGame in videoGames)
        {
            AddCard(videoGame);
            await Task.Delay(TimeSpan.FromSeconds(_delayUpdateInventory));
        }
    }

    private void AddCard(Game videoGame)
    {
        GameObject cardCanvas = Instantiate(_uICardPrefab, _cardContainer, false);
        UICard canvasCard = cardCanvas.GetComponent<UICard>();

        if (canvasCard == null)
        {
            Debug.LogError("El prefab no incluye el componente UICard.");
            return;
        }

        StartCoroutine(canvasCard.SetShortCardData(videoGame));
        _cardsCanvas.Add(canvasCard);

        Debug.Log($"Carta añadida al inventario: {videoGame.name}");
    }

    private void RemoveCard(Game videoGame)
    {
        int idToRemove = int.Parse(videoGame.id);
        for (int i = _cardsCanvas.Count - 1; i >= 0; i--)
        {
            if (_cardsCanvas[i].CardId == idToRemove)
            {
                Destroy(_cardsCanvas[i].gameObject);
                _cardsCanvas.RemoveAt(i);
                break;
            }
        }
    }

    private void HandleVideoGameAdded(Game videoGame)
    {
        AddCard(videoGame);
    }

    private void HandleVideoGameRemoved(Game videoGame)
    {
        RemoveCard(videoGame);
    }
}