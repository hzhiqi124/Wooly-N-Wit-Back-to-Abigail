using UnityEngine;
using UnityEngine.EventSystems;
using ChristinaCreatesGames.Animations;

public class RedoButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private SquashAndStretch squashAndStretch;

    void Start()
    {
        squashAndStretch = GetComponent<SquashAndStretch>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SceneController.instance.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        squashAndStretch.PlaySquashAndStretch();
    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }
}