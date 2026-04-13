using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using ChristinaCreatesGames.Animations;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image buttonImage;
    private SquashAndStretch squashAndStretch;
    private AudioSource audioSource;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color hoverColor = new Color(0.7f, 0.7f, 0.7f, 1f);
    [SerializeField] public AudioClip buttonHover;

    void Start()
    {
        buttonImage = GetComponent<Image>();
        squashAndStretch = GetComponent<SquashAndStretch>();
        audioSource = GetComponent<AudioSource>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonImage.color = hoverColor;
        squashAndStretch.PlaySquashAndStretch();
        audioSource.PlayOneShot(buttonHover);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonImage.color = normalColor;
    }
}