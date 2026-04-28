using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;

public class CoinsManager : MonoBehaviour
{
    public static CoinsManager Instance {  get; private set; }
    [SerializeField] private Coin coinPrefab;
    [SerializeField] private float spreadAngle = 30.0f;
    [SerializeField] private float minForce = 2.0f;
    [SerializeField] private float maxForce = 3.0f;

    [SerializeField] private bool collectionCheck = true;
    [SerializeField] private int defaultCapacity = 10;
    [SerializeField] private int maxPoolSize = 100;
    private IObjectPool<Coin> coinsPool;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
         coinsPool = new ObjectPool<Coin>
         (
                CreateCoin,
                OnGet,
                OnRelease,
                OnDestroyCoin,
                collectionCheck,
                defaultCapacity,
                maxPoolSize
         );
        DontDestroyOnLoad(gameObject);
    }

    private Coin CreateCoin()
    {
        Vector3 spawnOffset = Random.insideUnitSphere * 0.1f;
        Quaternion rotation = Quaternion.identity;
        Coin coin = Instantiate(coinPrefab, transform.position + spawnOffset, rotation, transform);
        coin.gameObject.SetActive(false);
        return coin;
    }

    public Coin GetCoin()
    {
        return coinsPool.Get();
    }

    public void Release(Coin coin)
    {
        coinsPool.Release(coin);

    }
    private void OnGet(Coin coin)
    {
        coin.gameObject.SetActive(true);
    }

    private void OnRelease(Coin coin)
    {
        coin.gameObject.SetActive(false);
        coin.Stop();
    }

    private void OnDestroyCoin(Coin coin)
    {
        coin.Stop();
        Destroy(coin);
    }
    [SerializeField] private LayerMask groundLayer; 

    public void SpawnCoins(Vector3 spawnPos, int coinsAmount)
    {
        float radius = 2.0f;
        float groundY = spawnPos.y; 

        if (Physics.Raycast(spawnPos, Vector3.down, out RaycastHit hit, 50f, groundLayer))
        {
            groundY = hit.point.y;
        }

        for (int i = 0; i < coinsAmount; i++)
        {
            Coin coin = coinsPool.Get();
            coin.transform.position = spawnPos;

            float angle = (360f / coinsAmount) * i;
            float rad = angle * Mathf.Deg2Rad;

            Vector3 targetPos = new Vector3(
                spawnPos.x + Mathf.Cos(rad) * radius,
                groundY + 0.3f, 
                spawnPos.z + Mathf.Sin(rad) * radius
            );

            coin.Play(targetPos);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
