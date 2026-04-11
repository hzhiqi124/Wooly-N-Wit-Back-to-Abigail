using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyDustZone : MonoBehaviour
{
    [SerializeField] private float upwardForce = 0.5f;
    [SerializeField] private float forceInterval = 2f;
    [SerializeField] private float particleDuration = 0.5f;
    [SerializeField] private ParticleSystem fairyDust;

    private float timer = 0f;
    private bool zoneActive = false;
    private List<Rigidbody2D> playersInside = new List<Rigidbody2D>();

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= forceInterval)
        {
            timer = 0f;
            StartCoroutine(ActivateZone());
        }

        if (zoneActive)
        {
            foreach (Rigidbody2D rb in playersInside)
                rb.AddForce(Vector2.up * upwardForce);
        }

    }

    IEnumerator ActivateZone()
    {
        zoneActive = true;
        fairyDust.Play();
        yield return new WaitForSeconds(particleDuration);
        fairyDust.Stop();
        zoneActive = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            playersInside.Add(collision.GetComponent<Rigidbody2D>());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            playersInside.Remove(collision.GetComponent<Rigidbody2D>());
    }
}