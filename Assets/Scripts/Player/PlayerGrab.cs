using UnityEngine;
using System.Collections;

public class PlayerGrab : MonoBehaviour
{
    public PlayerData playerData;

    [Header("Grab Settings")]
    public float grabDistance = 3f;
    public float throwForce = 5f;
    public LayerMask grabbableLayer;

    [Header("Hand Settings")]
    public GameObject Hand;
    public Vector3 handPosition = Vector3.zero;
    public Vector3 handRotation = Vector3.zero;
    [Range(0.1f, 3f)]
    public float handScale = 1f;

    private GameObject grabbedObject;
    private Vector3 originalScale;
    private Collider grabbedCollider;
    private PlayerInputActions inputActions;
    private bool grabInput;
    private bool saveInput;
    private bool isSaving; // Se agrega flag para evitar múltiples guardados

    void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.Player.Interact.performed += ctx => grabInput = true;
        inputActions.Player.Save.performed += ctx => saveInput = true;
    }
    void OnEnable()
    {
        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    void Update()
    {
        if (Time.timeScale == 0) return;

        if (grabInput)
        {
            if (grabbedObject == null)
                TryGrabObject();
            else
                ReleaseObject();

            grabInput = false;
        }
        if (saveInput)
        {
            if (grabbedObject != null)
            {
                SaveDataGame();
            }
            saveInput = false;
        }
    }

    void TryGrabObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, grabDistance, grabbableLayer))
        {
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                grabbedObject = hit.collider.gameObject;
                grabbedCollider = hit.collider;
                originalScale = grabbedObject.transform.localScale;

                // Desactivar componentes
                grabbedCollider.enabled = false;
                Destroy(rb);

                // Configurar objeto agarrado
                grabbedObject.transform.SetParent(Hand.transform);
                grabbedObject.transform.localPosition = handPosition;
                grabbedObject.transform.localRotation = Quaternion.Euler(handRotation);
                grabbedObject.transform.localScale = Vector3.one * handScale;
            }
        }
    }

    void ReleaseObject()
    {
        grabbedObject.transform.parent = null;

        // Restaurar componentes y escala
        Rigidbody rb = grabbedObject.AddComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        grabbedCollider.enabled = true;
        grabbedObject.transform.localScale = originalScale;

        // Aplicar fuerza de lanzamiento en la dirección de la cámara
        rb.AddForce(Camera.main.transform.forward * throwForce, ForceMode.Impulse);

        originalScale = Vector3.zero;
        grabbedObject = null;
        grabbedCollider = null;
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * grabDistance);
    }
#endif

    private void SaveDataGame()
    {
        if (isSaving) return;
        isSaving = true;

        Game videoGameData = grabbedObject.GetComponent<UICard>().VideoGameData;
        playerData.AddVideoGame(videoGameData);

        StartCoroutine(WaitForDestroy(videoGameData));
    }

    private IEnumerator WaitForDestroy(Game videoGameData)
    {
        grabbedObject.transform.localScale = Vector3.zero;
        yield return new WaitForSeconds(1f);

        Destroy(grabbedObject);
        grabbedObject = null;
        grabbedCollider = null;
        originalScale = Vector3.zero;
        isSaving = false; // Reinicia el flag
    }
}
