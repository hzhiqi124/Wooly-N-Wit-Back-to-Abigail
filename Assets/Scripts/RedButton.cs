using UnityEngine;
using ChristinaCreatesGames.Animations;
using System.Collections;

public class RedButton : MonoBehaviour
{
    [SerializeField] private GameObject buttonSprite;
    [SerializeField] private GameObject lantern;
    [SerializeField] private float lanternRiseHeight = 3f;
    [SerializeField] private float lanternSpeed = 2f;

    private SquashAndStretch squashAndStretch;
    private Vector3 lanternStartPos;
    private Vector3 lanternTargetPos;
    private bool isPressed = false;

    void Start()
    {
        squashAndStretch = buttonSprite.GetComponent<SquashAndStretch>();
        lanternStartPos = lantern.transform.position;
        lanternTargetPos = lanternStartPos + Vector3.up * lanternRiseHeight;
    }

    void Update()
    {
        if (isPressed)
            lantern.transform.position = Vector3.MoveTowards(lantern.transform.position, lanternTargetPos, lanternSpeed * Time.deltaTime);
        else
            lantern.transform.position = Vector3.MoveTowards(lantern.transform.position, lanternStartPos, lanternSpeed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("Button hit by: " + col.gameObject.name + " tag: " + col.gameObject.tag);
        if (col.gameObject.CompareTag("Player"))
        {
            isPressed = true;
            squashAndStretch.PlaySquashAndStretch();
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            isPressed = false;
            squashAndStretch.PlaySquashAndStretch();
        }
    }
}