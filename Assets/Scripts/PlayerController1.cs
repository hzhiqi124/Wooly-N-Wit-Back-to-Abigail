using ChristinaCreatesGames.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController1 : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float boostedJumpForce = 15f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool onOtherSheep;
    private float moveInput;
    private SquashAndStretch squashAndStretch;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        squashAndStretch = GetComponent<ChristinaCreatesGames.Animations.SquashAndStretch>();
    }

    void Update()
    {
        if (Keyboard.current.aKey.isPressed) moveInput = -1;
        else if (Keyboard.current.dKey.isPressed) moveInput = 1;
        else moveInput = 0;

        if (Keyboard.current.wKey.wasPressedThisFrame && isGrounded)
        {
            float force = onOtherSheep ? boostedJumpForce : jumpForce;
            rb.AddForce(Vector2.up * force, ForceMode2D.Impulse); // was jumpForce, should be force
            squashAndStretch.PlaySquashAndStretch();
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Platform"))
        {
            isGrounded = true;
        }

        if (col.gameObject.CompareTag("Player") && !isGrounded)
        {
            if (transform.position.y > col.transform.position.y)
            {
                isGrounded = true;
                onOtherSheep = true;
            }
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Platform") || col.gameObject.CompareTag("Player"))
        {
            isGrounded = false;
            onOtherSheep = false;
        }
    }
}