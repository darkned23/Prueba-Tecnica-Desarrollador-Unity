using UnityEngine;
using System.Collections;
using TMPro; // ...nuevo using...

public class UICardDetails : UICardSpawn
{
    public static UICardDetails Instance { get; private set; }

    [Header("Animation Parameters")]
    [SerializeField] private float reverseDuration;
    [SerializeField] private float elasticDuration;
    [SerializeField] private Rotator rotator;
    private Vector3 _savedScale;

    public void Awake()
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
    private void Start()
    {
        _savedScale = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    #region Animations
    public void AnimateAndShowDetails(Game videoGame)
    {
        StartCoroutine(AnimateAndShowDetailsCoroutine(videoGame));
    }

    private IEnumerator AnimateAndShowDetailsCoroutine(Game videoGame)
    {
        if (transform.localScale != Vector3.zero)
        {
            yield return StartCoroutine(ReversePopEffect());
        }

        yield return StartCoroutine(SetAllCardData(videoGame));
        yield return StartCoroutine(ElasticPopEffect());
    }

    private IEnumerator ReversePopEffect()
    {
        rotator.ResetRotation();
        float timer = 0f;
        Vector3 startScale = _savedScale;
        while (timer < reverseDuration)
        {
            timer += Time.unscaledDeltaTime;
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, timer / reverseDuration);
            yield return null;
        }
        transform.localScale = Vector3.zero;
    }

    private IEnumerator ElasticPopEffect()
    {
        float timer = 0f;
        float duration = elasticDuration;
        float c4 = (2 * Mathf.PI) / 3;
        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;
            float t = timer / duration;
            t = t == 0f ? 0f : t == 1f ? 1f :
                Mathf.Pow(2, -10 * t) * Mathf.Sin((t * 10 - 0.75f) * c4) + 1;
            transform.localScale = Vector3.LerpUnclamped(Vector3.zero, _savedScale, t);
            yield return null;
        }
        transform.localScale = _savedScale;
    }
    #endregion
}