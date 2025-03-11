using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Menu Pause")]
    [SerializeField] private Canvas _canvasPauseMenu;

    [Header("Inventory")]
    [SerializeField] private Canvas _canvasInventory;

    [SerializeField] private Camera _cameraCardDetails;

    private PlayerInputActions inputActions;
    private bool _isPaused = false;
    private bool _isInventoryOpen = false;

    void Awake()
    {
        inputActions = new PlayerInputActions();

        inputActions.UI.Pause.performed += ctx => TogglePause();
        inputActions.UI.Inventory.performed += ctx => ToggleInventory();
    }

    void Start()
    {
        Time.timeScale = 1;
    }

    void OnEnable() => inputActions.Enable();
    void OnDisable() => inputActions.Disable();

    public void TogglePause()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayGrabSound();

        if (_isInventoryOpen)
        {
            _isInventoryOpen = false;
            _canvasInventory.enabled = false;
            _cameraCardDetails.enabled = false;

            Time.timeScale = (_isPaused || _isInventoryOpen) ? 0f : 1f;
            Cursor.lockState = (_isPaused || _isInventoryOpen) ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = _isPaused || _isInventoryOpen;
            return;
        }

        _isPaused = !_isPaused;
        _canvasPauseMenu.enabled = _isPaused;

        Time.timeScale = (_isPaused || _isInventoryOpen) ? 0f : 1f;
        Cursor.lockState = (_isPaused || _isInventoryOpen) ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = _isPaused || _isInventoryOpen;
    }

    public void ToggleInventory()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayGrabSound();

        _isInventoryOpen = !_isInventoryOpen;
        if (_isInventoryOpen)
            _isPaused = false;

        _canvasInventory.enabled = _isInventoryOpen;
        _cameraCardDetails.enabled = _isInventoryOpen;

        _canvasPauseMenu.enabled = _isPaused;

        Time.timeScale = (_isPaused || _isInventoryOpen) ? 0f : 1f;
        Cursor.lockState = (_isPaused || _isInventoryOpen) ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = _isPaused || _isInventoryOpen;
    }
}
