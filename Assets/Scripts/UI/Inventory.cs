using System;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;

public class Invetory : MonoBehaviour
{
    [SerializeField] private GameObject _cardCanvasPrefab;
    [SerializeField] private Transform _cardContainer;
    [SerializeField] private float _delayUpdateInventory = 0.1f;

    private List<UICard> _cardsCanvas = new();

    private void Start()
    {
        Debug.Log("Inventario iniciado");

        if (PlayerData.Instance != null && PlayerData.Instance.VideoGameData != null)
        {
            AddAllCardsAsync(PlayerData.Instance.VideoGameData);
            PlayerData.Instance.NeedsUpdateInventory = false;
        }
    }

    void OnEnable()
    {
        Debug.Log("Inventario activado");

        if (PlayerData.Instance.AddedVideoGames.Count > 0)
        {
            AddAllCardsAsync(PlayerData.Instance.AddedVideoGames);
            PlayerData.Instance.AddedVideoGames.Clear();
            Debug.Log("Añadiendo cartas al inventario");
        }
        else if (PlayerData.Instance.RemovedVideoGames.Count > 0)
        {
            RemoveAllCards(PlayerData.Instance.RemovedVideoGames);
            PlayerData.Instance.RemovedVideoGames.Clear();
            Debug.Log("Eliminando cartas del inventario");
        }
    }

    private async void AddAllCardsAsync(List<Game> videoGames)
    {
        Debug.Log("Añadiendo cartas al inventario (modo async/await)");
        List<Game> gamesCopy = new List<Game>(videoGames);
        foreach (Game videoGame in gamesCopy)
        {
            AddCard(videoGame);
            await Task.Delay(TimeSpan.FromSeconds(_delayUpdateInventory));
        }
    }

    private void AddCard(Game videoGame)
    {
        GameObject cardCanvas = Instantiate(_cardCanvasPrefab, _cardContainer, false);
        UICard canvasCard = cardCanvas.GetComponent<UICard>();
        StartCoroutine(canvasCard.SetShortCardData(videoGame));
        _cardsCanvas.Add(canvasCard);
        Debug.Log($"Carta añadida al inventario: {videoGame.name}");
    }

    private async void RemoveAllCards(List<Game> videoGames)
    {
        foreach (Game videoGame in videoGames)
        {
            RemoveCard(videoGame);
            await Task.Delay(TimeSpan.FromSeconds(_delayUpdateInventory));
        }
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
}