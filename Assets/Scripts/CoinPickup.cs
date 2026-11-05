using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    private Vector3 startPosition;
    private float phase;

    void Start()
    {
        startPosition = transform.position;
        phase = Random.Range(0f, 6.28318f);
    }

    void Update()
    {
        transform.Rotate(0f, 100f * Time.deltaTime, 0f, Space.World);
        Vector3 p = startPosition;
        p.y += Mathf.Sin(Time.time * 2f + phase) * 0.15f;
        transform.position = p;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        var game = FindObjectOfType<CoinCollectorGame>();
        if (game) game.CollectCoin();
        Destroy(gameObject);
    }
}
