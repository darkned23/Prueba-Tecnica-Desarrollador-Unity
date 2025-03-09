using UnityEngine;
using System.Collections;

public class Card : MonoBehaviour
{
    [SerializeField] private GameObject _cardModel;
    [SerializeField] private bool _isDetails;
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

        float durationToNormal = 0.08f;
        timer = 0f;
        while (timer < durationToNormal)
        {
            timer += Time.deltaTime;
            goTransform.localScale = Vector3.Lerp(overshootScale, normalScale, timer / durationToNormal);
            yield return null;
        }

        _rigidbody.useGravity = true;
    }
}
