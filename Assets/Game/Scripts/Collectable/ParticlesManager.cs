using UnityEngine;
using UnityEngine.Pool;

public class ParticlesManager : MonoBehaviour
{
    public static ParticlesManager Instance;

    [SerializeField] private UltaParticle particlePrefab;

    [SerializeField] private bool collectionCheck = true;
    [SerializeField] private int defaultCapacity = 10;
    [SerializeField] private int maxPoolSize = 100;
    private IObjectPool<UltaParticle> particlePool;

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
        particlePool = new ObjectPool<UltaParticle>
        (
            CreateCoin,
            OnGet,
            OnRelease,
            OnDestroyParticle,
            collectionCheck,
            defaultCapacity,
            maxPoolSize
        );

        DontDestroyOnLoad(gameObject);
    }

    private UltaParticle CreateCoin()
    {
        Vector3 spawnOffset = Random.insideUnitSphere * 0.1f;
        UltaParticle coin = Instantiate(particlePrefab, transform.position + spawnOffset, Quaternion.identity, transform);
        coin.gameObject.SetActive(false);
        return coin;
    }

    private void OnGet(UltaParticle particle)
    {
        particle.gameObject.SetActive(true);
    }

    private void OnRelease(UltaParticle particle)
    {
        particle.gameObject.SetActive(false);
    }

    private void OnDestroyParticle(UltaParticle particle)
    {
        Destroy(particle);
    }
    public void SpawnParticles(Vector3 spawnPos, int particlesAmount)
    {
        for (int i = 0; i < particlesAmount; i++)
        {
            UltaParticle particle = particlePool.Get();
            particle.transform.position = spawnPos;
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
