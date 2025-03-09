using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenuUI;
    [SerializeField] private GameObject _cameraCardDetails;

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
        if (_pauseMenuUI != null && _cameraCardDetails != null)
        {
            _pauseMenuUI.SetActive(false);
            _cameraCardDetails.SetActive(false);
        }
    }

    void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;

        _pauseMenuUI?.SetActive(isPaused);
        _cameraCardDetails?.SetActive(isPaused);

        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isPaused;
    }
}
