using UnityEngine;

public class CoinsSpawner : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private float heightMultiplayer;
    [SerializeField] private float spreadAngle = 30.0f;
    [SerializeField] private float minForce = 2.0f;
    [SerializeField] private float maxForce = 3.0f;
    public void SpawnCoins(int coinsAmount)
    {
        for (int i = 0; i < coinsAmount; i++)
        {
            Vector3 spawnOffset = Random.insideUnitSphere * 0.1f;
            GameObject coin = Instantiate(coinPrefab, transform.position + spawnOffset, Quaternion.identity);
            Rigidbody rb = coin.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                float force = Random.Range(minForce, maxForce);
                rb.AddForce(GetRandomDirection() * force, ForceMode.VelocityChange);
            }
        }
    }
    private Vector3 GetRandomDirection()
    {

        float angle = Random.Range(-spreadAngle / 2.0f, spreadAngle / 2.0f) * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(angle), Mathf.Abs(Mathf.Cos(angle)) * heightMultiplayer, 0.0f);
    }
}
