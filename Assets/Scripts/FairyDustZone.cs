using UnityEngine;

public class FairyDustZone : MonoBehaviour
{
    [SerializeField] private float upwardForce = 0.5f;
    [SerializeField] private float forceInterval = 2f;

    private float timer = 0f;
    private Rigidbody2D playerRb;
    private bool playerInside = false; 
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
    }
}
