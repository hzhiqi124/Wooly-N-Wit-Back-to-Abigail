using ChristinaCreatesGames.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController2 : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float boostedJumpForce = 5f;

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
        if (Keyboard.current.leftArrowKey.isPressed) moveInput = -1;
        else if (Keyboard.current.rightArrowKey.isPressed) moveInput = 1;
        else moveInput = 0;

        if (Keyboard.current.upArrowKey.wasPressedThisFrame && isGrounded)
        {
            float force = onOtherSheep ? boostedJumpForce : jumpForce;
            rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
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