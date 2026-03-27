using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;

    private Rigidbody2D rb;
    private InputAction moveAction;
    private InputAction jumpAction;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        PlayerInput playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
    }

    void Update()
    {
        float move = moveAction.ReadValue<Vector2>().x;
        rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);

        if (jumpAction.WasPressedThisFrame() && isGrounded)
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Platform"))
            isGrounded = true;
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Platform"))
            isGrounded = false;
    }
}