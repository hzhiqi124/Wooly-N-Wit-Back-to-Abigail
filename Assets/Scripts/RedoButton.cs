using UnityEngine;
using UnityEngine.EventSystems;
using ChristinaCreatesGames.Animations;

public class RedoButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private SquashAndStretch squashAndStretch;
    private AudioSource audioSource;
    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private AudioClip clickSound;

    void Start()
    {
        squashAndStretch = GetComponent<SquashAndStretch>();
        audioSource = GetComponent<AudioSource>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        audioSource.PlayOneShot(clickSound);
        Invoke("ReloadScene", 0.2f);
    }
    
    void ReloadScene()
    {
        SceneController.instance.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        squashAndStretch.PlaySquashAndStretch();
        audioSource.PlayOneShot(hoverSound);
    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }
}