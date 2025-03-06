using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 7f;
    private Rigidbody rb;
    private Vector2 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Método llamado por el sistema de Input para el movimiento
    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    // Método llamado por el sistema de Input para el salto
    private void OnJump(InputValue value)
    {
        if (value.isPressed && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y);
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }

    // Verifica si el jugador está en el suelo
    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }
}