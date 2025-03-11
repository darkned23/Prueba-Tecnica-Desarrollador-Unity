using UnityEngine;

public class FrameRateLimiter : MonoBehaviour
{
    [Tooltip("Tasa de refresco deseada (0 para ilimitado)")]
    public int targetFrameRate = 60;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFrameRate;
    }
}