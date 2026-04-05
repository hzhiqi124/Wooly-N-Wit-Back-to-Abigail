using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using ChristinaCreatesGames.Animations;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image buttonImage;
    private SquashAndStretch squashAndStretch;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color hoverColor = new Color(0.7f, 0.7f, 0.7f, 1f);

    void Start()
    {
        buttonImage = GetComponent<Image>();
        squashAndStretch = GetComponent<SquashAndStretch>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonImage.color = hoverColor;
        squashAndStretch.PlaySquashAndStretch();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonImage.color = normalColor;
    }
}