using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyDustZone : MonoBehaviour
{
    [SerializeField] private float upwardForce = 0.5f;
    [SerializeField] private float forceInterval = 2f;
    [SerializeField] private float particleDuration = 0.5f;
    [SerializeField] private ParticleSystem fairyDust;
    [SerializeField] private GameObject wooly;

    private float timer = 0f;
    private bool zoneActive = false;
    private List<GameObject> playersInside = new List<GameObject>();

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= forceInterval)
        {
            timer = 0f;
            Debug.Log("Timer fired, starting coroutine");
            StartCoroutine(ActivateZone());
        }

        if (zoneActive)
        {
            Debug.Log("Zone active, players inside: " + playersInside.Count);
            foreach (Rigidbody2D rb in playersInside)
            {
                Debug.Log("Applying force to: " + rb.gameObject.name + " rb null? " + (rb == null));
                rb.AddForce(Vector2.up * upwardForce);
            }
        }

    }

    IEnumerator ActivateZone()
    {
        Debug.Log("ActivateZone called");
        zoneActive = true;
        fairyDust.Clear();
        fairyDust.Play();
        yield return new WaitForSeconds(particleDuration);
        fairyDust.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        zoneActive = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == wooly)
            playersInside.Add(collision.GetComponent<Rigidbody2D>());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Something exited zone: " + collision.gameObject.name);
        if (collision.gameObject == wooly)
        {
            Debug.Log("Wooly exited zone");
            playersInside.Remove(collision.GetComponent<Rigidbody2D>());
        }
    }
}