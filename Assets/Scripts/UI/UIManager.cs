using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;

    private PlayerInputActions inputActions;
    private bool isPaused = false;

    void Awake()
    {
        inputActions = new PlayerInputActions();

        inputActions.UI.Pause.performed += ctx => TogglePause();
    }

    void OnEnable() => inputActions.Enable();
    void OnDisable() => inputActions.Disable();

    void Start()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false); // Asegurar que el menú inicie oculto
    }

    void TogglePause()
    {
        isPaused = !isPaused;

        Time.timeScale = isPaused ? 0f : 1f;
        pauseMenuUI?.SetActive(isPaused);

        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isPaused;
    }
}
