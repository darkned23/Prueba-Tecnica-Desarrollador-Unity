using UnityEngine;

public class Player : MonoBehaviour
{
    // Component references
    private Rigidbody rb;
    private CapsuleCollider capsule;

    // Movement parameters
    public float speed = 5f;
    public float jumpForce = 5f;

    // Variables to store input
    private float horizontal;
    private float vertical;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        // Capture input
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        // Jump input
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        }
    }

    void FixedUpdate()
    {
        // Calculate horizontal movement and apply it while preserving vertical velocity
        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        rb.linearVelocity = new Vector3(move.x * speed, rb.linearVelocity.y, move.z * speed);
    }

    // Chequea si el jugador está tocando el suelo usando un Raycast
    bool IsGrounded()
    {
        float extraHeight = 0.1f;
        return Physics.Raycast(transform.position, Vector3.down, capsule.bounds.extents.y + extraHeight);
    }
}