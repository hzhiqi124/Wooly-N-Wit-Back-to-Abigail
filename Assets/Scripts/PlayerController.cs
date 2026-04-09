using ChristinaCreatesGames.Animations;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public enum ControlScheme { WASD, Arrows }
    [SerializeField] private ControlScheme controlScheme;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float boostedJumpForce = 5f;
    [SerializeField] private Animator animator; //for character animation
    [SerializeField] private AudioClip bounceSound;

    private AudioSource audioSource;
    [HideInInspector] public Rigidbody2D rb;
    private bool isGrounded;
    private bool onOtherSheep;
    private float moveInput;
    private SquashAndStretch squashAndStretch;
    private Vector3 rideOffset;
    private Transform ridingTarget;

    public bool isBeingRidden;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        squashAndStretch = GetComponent<ChristinaCreatesGames.Animations.SquashAndStretch>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (controlScheme == ControlScheme.WASD)
        {
            if (Keyboard.current.aKey.isPressed) moveInput = -1;
            else if (Keyboard.current.dKey.isPressed) moveInput = 1;
            else moveInput = 0;

            if (Keyboard.current.wKey.wasPressedThisFrame && isGrounded)
                Jump();
        }
        else
        {
            if (Keyboard.current.leftArrowKey.isPressed) moveInput = -1;
            else if (Keyboard.current.rightArrowKey.isPressed) moveInput = 1;
            else moveInput = 0;

            if (Keyboard.current.upArrowKey.wasPressedThisFrame && isGrounded)
                Jump();
        }

        //for walk animation
        if (moveInput != 0)
        {
            animator.SetBool("IsRunning", true);
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }

        if (moveInput < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (moveInput > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        if (onOtherSheep && ridingTarget != null)
        {
            if (moveInput != 0)
            {
                ridingTarget = null;
                rb.bodyType = RigidbodyType2D.Dynamic;
            }
            else
            {
                rb.bodyType = RigidbodyType2D.Kinematic;
                transform.position = new Vector3(
                    ridingTarget.position.x + rideOffset.x,
                    ridingTarget.position.y + rideOffset.y,
                    transform.position.z);
                rb.linearVelocity = Vector2.zero;
            }
        }
        else if (rb.bodyType == RigidbodyType2D.Kinematic)
        {
            // make sure we always restore dynamic if riding stopped
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    

    void Jump()
    {
        animator.SetBool("IsJumping", true); //jump animation
        rb.bodyType = RigidbodyType2D.Dynamic;
        float force = onOtherSheep ? boostedJumpForce : jumpForce;
        rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        squashAndStretch.PlaySquashAndStretch();
        audioSource.PlayOneShot(bounceSound);
        ridingTarget = null;
        onOtherSheep = false;

        Rigidbody2D riderRb = GetRider();
        if (riderRb != null)
        {
            riderRb.bodyType = RigidbodyType2D.Dynamic;
            riderRb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        }
            
    }

    void FixedUpdate()
    {
        if (rb.bodyType == RigidbodyType2D.Dynamic)
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    Rigidbody2D GetRider()
    {
        PlayerController[] allPlayers = FindObjectsByType<PlayerController>(FindObjectsSortMode.None);
        foreach (PlayerController p in allPlayers)
        {
            if (p.ridingTarget == this.transform)
            {
                p.ridingTarget = null;
                p.onOtherSheep = false;
                p.rb.bodyType = RigidbodyType2D.Dynamic;
                return p.GetComponent<Rigidbody2D>();
            }
        }
        return null;
    }

    


    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Platform"))
        {
            isGrounded = true;
            animator.SetBool("IsJumping", false); //jump animation
        }

 
        if (col.gameObject.CompareTag("Player") && !isGrounded)
        {
            if (transform.position.y > col.transform.position.y)
            {
                isGrounded = true;
                onOtherSheep = true;

                if (moveInput == 0)
                {
                    ridingTarget = col.transform;
                    rideOffset = transform.position - col.transform.position;
                }
                
            }
        }

        if (col.gameObject.CompareTag("Disintegrating"))
        {
            float topEdge = col.transform.position.y + col.transform.localScale.y / 2;
            float sheepBottom = transform.position.y - transform.localScale.y / 2;
            Debug.Log("topEdge: " + topEdge + " sheepBottom: " + sheepBottom);
            if (sheepBottom >= topEdge - 0.1f)
            {
                col.gameObject.SetActive(false);
            }
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Platform"))
            isGrounded = false;
       

        if (col.gameObject.CompareTag("Player"))
        {

            if (transform.position.y > col.transform.position.y)
            {
                isGrounded = false;
                onOtherSheep = false;
                ridingTarget = null;
                rb.bodyType = RigidbodyType2D.Dynamic;
            }
        }
    }
}