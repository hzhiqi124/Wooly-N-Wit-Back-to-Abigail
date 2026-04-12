using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyDustZone : MonoBehaviour
{
    [SerializeField] private float upwardForce = 0.5f;
    [SerializeField] private float forceInterval = 2f;
    [SerializeField] private float particleDuration = 4f;
    [SerializeField] private ParticleSystem fairyDust;
    [SerializeField] private GameObject wooly;

    private float timer = 0f;
    private bool zoneActive = false;
    private List<GameObject> playersInside = new List<GameObject>();

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= forceInterval && !zoneActive)
        {
            timer = 0f;
            StartCoroutine(ActivateZone());
        }

        if (zoneActive)
        {
            foreach (GameObject player in playersInside)
            {
                Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
                if (rb != null && rb.bodyType == RigidbodyType2D.Dynamic)
                    rb.AddForce(Vector2.up * upwardForce);
            }
        }
    }

    IEnumerator ActivateZone()
    {
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
            playersInside.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == wooly)
            playersInside.Remove(collision.gameObject);
    }
}