using UnityEngine;
using UnityEngine.EventSystems;
using ChristinaCreatesGames.Animations;

public class ExitButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
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
        Invoke("ExitScene", 0.2f);
    }

    void ExitScene()
    {
        SceneController.instance.LoadScene("MenuScene");
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