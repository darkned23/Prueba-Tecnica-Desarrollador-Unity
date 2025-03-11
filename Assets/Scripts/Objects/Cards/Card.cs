using UnityEngine;
using System.Collections;

public class Card : MonoBehaviour
{
    [SerializeField] private GameObject _cardModel;
    [SerializeField] private bool _isDetails;
    [SerializeField] private float _finalScale; // Escala final uniforme en todos los ejes
    [SerializeField] private float _growthFactor = 1.5f; // Factor de crecimiento para el efecto (por defecto 1.5)
    private Rigidbody _rigidbody;
    private UICard _uICard;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _uICard = GetComponent<UICard>();

        StartCoroutine(PopCardEffect());
    }

    public void Initialize(Game gameData)
    {
        _uICard.SetAllCardData(gameData);
    }

    private IEnumerator PopCardEffect()
    {
        Transform goTransform = gameObject.transform;
        goTransform.localScale = Vector3.zero;

        _cardModel.SetActive(true);
        StartCoroutine(_uICard.SetAllCardData(ApiRawgManager.Instance.GetGame()));

        Vector3 finalScale = new Vector3(_finalScale, _finalScale, _finalScale);
        Vector3 overshootScale = finalScale * _growthFactor;

        float durationToOvershoot = 0.12f;
        float timer = 0f;
        while (timer < durationToOvershoot)
        {
            timer += Time.deltaTime;
            goTransform.localScale = Vector3.Lerp(Vector3.zero, overshootScale, timer / durationToOvershoot);
            yield return null;
        }

        float durationToNormal = 0.08f;
        timer = 0f;
        while (timer < durationToNormal)
        {
            timer += Time.deltaTime;
            goTransform.localScale = Vector3.Lerp(overshootScale, finalScale, timer / durationToNormal);
            yield return null;
        }

        _rigidbody.useGravity = true;
    }
}
