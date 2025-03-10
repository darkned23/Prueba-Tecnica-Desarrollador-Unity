using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Menu Pause")]
    [SerializeField] private Canvas _canvasPauseMenu;

    [Header("Inventory")]
    [SerializeField] private Canvas _canvasInventory;
    [SerializeField] private Canvas _canvasCardDetails;
    [SerializeField] private MeshRenderer _cardDetailsModel;

    private PlayerInputActions inputActions;
    private bool _isPaused = false;
    private bool _isInventoryOpen = false;

    void Awake()
    {
        inputActions = new PlayerInputActions();

        inputActions.UI.Pause.performed += ctx => TogglePause();
        inputActions.UI.Inventory.performed += ctx => ToggleInventory();
    }

    void OnEnable() => inputActions.Enable();
    void OnDisable() => inputActions.Disable();

    public void TogglePause()
    {
        // Si el inventario está abierto, se cierra y se sale del método
        if (_isInventoryOpen)
        {
            _isInventoryOpen = false;
            _canvasInventory.enabled = false;
            _canvasCardDetails.enabled = false;
            _cardDetailsModel.enabled = false;

            Time.timeScale = (_isPaused || _isInventoryOpen) ? 0f : 1f;
            Cursor.lockState = (_isPaused || _isInventoryOpen) ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = _isPaused || _isInventoryOpen;
            return;
        }

        // Si el inventario está cerrado, se alterna el estado de pausa
        _isPaused = !_isPaused;
        _canvasPauseMenu.enabled = _isPaused;

        Time.timeScale = (_isPaused || _isInventoryOpen) ? 0f : 1f;
        Cursor.lockState = (_isPaused || _isInventoryOpen) ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = _isPaused || _isInventoryOpen;
    }

    public void ToggleInventory()
    {
        _isInventoryOpen = !_isInventoryOpen;
        if (_isInventoryOpen)
            _isPaused = false;

        _canvasInventory.enabled = _isInventoryOpen;
        _canvasCardDetails.enabled = _isInventoryOpen;
        _cardDetailsModel.enabled = _isInventoryOpen;

        _canvasPauseMenu.enabled = _isPaused;

        Time.timeScale = (_isPaused || _isInventoryOpen) ? 0f : 1f;
        Cursor.lockState = (_isPaused || _isInventoryOpen) ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = _isPaused || _isInventoryOpen;
    }
}
