using UnityEngine;

public class Star : MonoBehaviour
{
    [SerializeField] private GameObject finishPointWall;
    [SerializeField] private GameObject lightWall;
    [SerializeField] AudioClip starSound;
    [SerializeField] public GameObject Tutorial01;
    [SerializeField] public GameObject Tutorial02;

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
            Invoke("EnableTutorial", 0.5f);
            
        }
    }

    void DisableStar()
    {
        gameObject.SetActive(false);
    }

    void EnableTutorial()
    {
        Tutorial01.SetActive(true);
        Tutorial02.SetActive(true);
    }
}
