using ChristinaCreatesGames.Animations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    public enum ControlScheme { WASD, Arrows }
    [SerializeField] private ControlScheme controlScheme;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float boostedJumpForce = 5f;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioClip bounceSound;
    [SerializeField] private PhysicsMaterial2D normalMaterial;
    [SerializeField] private PhysicsMaterial2D slipperyMaterial;
    private Collider2D col2D;

    private AudioSource audioSource;
    [HideInInspector] public Rigidbody2D rb;
    private int groundContactCount = 0;
    private bool isGrounded => groundContactCount > 0;
    private bool isTouchingWall = false;
    public bool onOtherSheep;
    private float moveInput;
    private SquashAndStretch squashAndStretch;
    private Vector3 rideOffset;
    public Transform ridingTarget;
    private float lastFacingDirection = 1f;
    private SpriteRenderer spriteRenderer;
    private GameObject disintegratingTarget;

    public bool isBeingRidden;
    private bool isGrabbing = false;
    private Vector3 grabOffset;
    private bool canGrab = false;
    private Transform grabbedTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col2D = GetComponent<Collider2D>();
        squashAndStretch = GetComponent<ChristinaCreatesGames.Animations.SquashAndStretch>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (controlScheme == ControlScheme.WASD)
        {
            if (Keyboard.current.aKey.isPressed) moveInput = -1;
            else if (Keyboard.current.dKey.isPressed) moveInput = 1;
            else moveInput = 0;

            if (Keyboard.current.wKey.wasPressedThisFrame)
            {
                print("isGrounded:" + isGrounded + ", isTouchingWall: " + isTouchingWall + ", isGrabbing: " + isGrabbing);
                if ((isGrounded || isTouchingWall) && !isGrabbing)
                    Jump();
            }

            if (Keyboard.current.wKey.isPressed && canGrab)
                StartGrab();
    
            if (Keyboard.current.wKey.wasReleasedThisFrame)
                StopGrab();
        }
        else if (controlScheme == ControlScheme.Arrows)
        {
            if (Keyboard.current.leftArrowKey.isPressed) moveInput = -1;
            else if (Keyboard.current.rightArrowKey.isPressed) moveInput = 1;
            else moveInput = 0;

            if (Keyboard.current.upArrowKey.wasPressedThisFrame && (isGrounded || isTouchingWall))
                Jump();

            if (Keyboard.current.upArrowKey.isPressed && !isGrounded && rb.linearVelocity.y < 0)
                rb.gravityScale = 0.2f;
            else
                rb.gravityScale = 0.7f;
        }

        // follow grabbed object
        if (isGrabbing && grabbedTransform != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.linearVelocity = Vector2.zero;
            transform.position = new Vector3(
                grabbedTransform.position.x + grabOffset.x,
                grabbedTransform.position.y + grabOffset.y,
                transform.position.z);
        }

        if (moveInput != 0)
            animator.SetBool("IsRunning", true);
        else
            animator.SetBool("IsRunning", false);

        if (moveInput < 0) lastFacingDirection = -1f;
        else if (moveInput > 0) lastFacingDirection = 1f;

        spriteRenderer.flipX = lastFacingDirection < 0;

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
        else if (rb.bodyType == RigidbodyType2D.Kinematic && !isGrabbing && ridingTarget == null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }

        // safety reset if stuck as grounded while in the air
        if (groundContactCount > 0 && rb.linearVelocity.y > 2f)
         groundContactCount = 0;
    }

    void Jump()
    {
        groundContactCount = 0;
        isTouchingWall = false;
        animator.SetBool("IsJumping", true);
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

    void DisableDisintegrating()
    {
        if (disintegratingTarget != null)
        {
            if (isGrounded && groundContactCount > 0)
                groundContactCount = Mathf.Max(0, groundContactCount - 1);
            disintegratingTarget.SetActive(false);
            disintegratingTarget = null;
        }
    }

    //check collision enter for different numbers, maybe upon collision one object doesnt add up
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Platform") || col.gameObject.CompareTag("Lantern"))
        {
            foreach (ContactPoint2D contact in col.contacts)
            {
                if (contact.normal.y > 0.5f)
                {
                    groundContactCount++;
                    animator.SetBool("IsJumping", false);
                    animator.SetBool("IsRunning", false);
                    break;
                }
                else if (Mathf.Abs(contact.normal.x) > 0.5f)
                {
                    isTouchingWall = true;
                }
            }
        }

        if (col.gameObject.CompareTag("Player") && !isGrounded)
        {
            if (transform.position.y > col.transform.position.y)
            {
                groundContactCount++;
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
            col2D.sharedMaterial = slipperyMaterial;
            rb.sharedMaterial = slipperyMaterial;
            float topEdge = col.transform.position.y + col.transform.localScale.y / 2;
            float sheepBottom = transform.position.y - transform.localScale.y / 2;
            if (sheepBottom >= topEdge - 0.1f)
            {
                groundContactCount++;
                animator.SetBool("IsJumping", false);
                disintegratingTarget = col.gameObject;
                Invoke("DisableDisintegrating", 0.8f);
            }
        }

        if (controlScheme == ControlScheme.WASD && col.gameObject.CompareTag("Lantern"))
        {
            foreach (ContactPoint2D contact in col.contacts)
            {
                if (contact.normal.y < -0.5f)
                {
                    canGrab = true;
                    grabbedTransform = col.transform;
                    grabOffset = transform.position - col.transform.position;
                    break;
                }
            }
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        int before = groundContactCount;
        if (col.gameObject.CompareTag("Platform") || col.gameObject.CompareTag("Lantern"))
        {
            Debug.Assert(groundContactCount > 0);
            groundContactCount = Mathf.Max(0, groundContactCount - 1);
            isTouchingWall = false;
        }

        if (col.gameObject.CompareTag("Disintegrating"))
        {
            if (col.gameObject.activeSelf)
                groundContactCount = Mathf.Max(0, groundContactCount - 1);
            col2D.sharedMaterial = normalMaterial;
            rb.sharedMaterial = normalMaterial;
        }

        if (col.gameObject.CompareTag("Player"))
        {
            if (onOtherSheep)
                groundContactCount = Mathf.Max(0, groundContactCount - 1);
            onOtherSheep = false;
            ridingTarget = null;
            rb.bodyType = RigidbodyType2D.Dynamic;
        }

        if (controlScheme == ControlScheme.WASD && col.gameObject.CompareTag("Lantern"))
        {
            canGrab = false;
            if (!isGrabbing)
                grabbedTransform = null;
        }
        print("Exiting " + col.gameObject.name + " before " + before + " after " + groundContactCount);
    }

    void StartGrab()
    {
        if (canGrab)
            isGrabbing = true;
    }

    void StopGrab()
    {
        isGrabbing = false;
        rb.bodyType = RigidbodyType2D.Dynamic;
        grabbedTransform = null;
    }
}