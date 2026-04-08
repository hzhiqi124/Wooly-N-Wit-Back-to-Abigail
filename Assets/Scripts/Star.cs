using UnityEngine;

public class Star : MonoBehaviour
{
    [SerializeField] private GameObject finishPointWall;
    [SerializeField] private GameObject lightWall;
    [SerializeField] AudioClip starSound;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            audioSource.PlayOneShot(starSound);
            finishPointWall.SetActive(true);
            lightWall.SetActive(true);
            Invoke("DisableStar", 0.5f);
        }
    }

    void DisableStar()
    {
        gameObject.SetActive(false);
    }
}
