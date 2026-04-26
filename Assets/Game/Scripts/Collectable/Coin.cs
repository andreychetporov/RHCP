using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Coin : MonoBehaviour
{
    [SerializeField] private float minflyTDuration = 2.0f;
    [SerializeField] private float maxflyTDuration = 3.0f;
    [SerializeField] GameEvent gameEvent;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
        rb.linearVelocity = Vector3.zero;

        StartCoroutine(MagnetCoroutine());
    }

    private IEnumerator MagnetCoroutine()
    {
        float timer = 0.0f;
        Vector3 startPos = transform.position;
        float flyDuration = UnityEngine.Random.Range(minflyTDuration, maxflyTDuration); 
        while (timer <= flyDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / flyDuration;

            float distance = Vector3.Distance(Camera.main.transform.position, transform.position);
            Vector3 targetPos = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, distance));
            transform.position = Vector3.Lerp(startPos, targetPos, progress);
            yield return null;
        }
        gameEvent.Raise();
        Destroy(gameObject);
    }
}
