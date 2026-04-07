using UnityEngine;

public class Star : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField] private GameObject finishPointWall;
    [SerializeField] private GameObject lightWall;
    [SerializeField] private AudioClip starDisappear;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            finishPointWall.SetActive(true);
            audioSource.PlayOneShot(starDisappear);
            gameObject.SetActive(false);
            lightWall.SetActive(true);
        }
    }
}
