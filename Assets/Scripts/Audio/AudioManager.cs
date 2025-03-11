using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; } // Nuevo singleton

    [Header("Audio Clips")]
    public AudioClip backgroundMusic;
    public AudioClip grabSound;
    public AudioClip saveSound;

    [SerializeField] private AudioSource bgAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;

    void Awake()
    {
        // Singleton: si ya hay una instancia, destruir esta
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        bgAudioSource.clip = backgroundMusic;
        bgAudioSource.loop = true;
        bgAudioSource.playOnAwake = false;
    }

    void Start()
    {
        if (backgroundMusic != null)
            bgAudioSource.Play();
    }

    public void PlayGrabSound()
    {
        if (grabSound != null)
            sfxAudioSource.PlayOneShot(grabSound);
    }

    public void PlaySaveSound()
    {
        if (saveSound != null)
            sfxAudioSource.PlayOneShot(saveSound);
    }
}
