using UnityEngine;
using UnityEngine.EventSystems;
using ChristinaCreatesGames.Animations;

public class ExitButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private SquashAndStretch squashAndStretch;

    void Start()
    {
        squashAndStretch = GetComponent<SquashAndStretch>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SceneController.instance.LoadScene("MenuScene");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        squashAndStretch.PlaySquashAndStretch();
    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }
}