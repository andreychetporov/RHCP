using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace Game.Blood
{
    public enum BloodType 
    { 
        Small, 
        Big
    }

    public class BloodManager : MonoBehaviour
    {
        public static BloodManager Instance { get; private set; }

        [Header("Settings")]
        [SerializeField] private bool _collectionCheck = true;
        [SerializeField] private int _defaultCapacity = 10;
        [SerializeField] private int _maxPoolSize = 100;

        private IObjectPool<BloodEmitter> _damagePool;
        private IObjectPool<BloodEmitter> _deathPool;

        private List<BloodEmitter> _activeBloodEmitters;

        [Inject(Id = "SMALL")] private BloodEmitter _bloodEmitterDamagePrefab;
        [Inject(Id = "BIG")] private BloodEmitter _bloodEmitterKillPrefab;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }

            _activeBloodEmitters = new List<BloodEmitter>();

            DontDestroyOnLoad(gameObject);
        }

        private void Start() => InitializePools();

        public BloodEmitter GetForDamage() => _damagePool.Get();
        public BloodEmitter GetForDeath() => _deathPool.Get();

        public void Release(BloodEmitter emitter)
        {
            if (emitter.BloodType == BloodType.Small)
                _damagePool.Release(emitter);
            else
                _deathPool.Release(emitter);
        }

        private void InitializePools()
        {
            _damagePool = CreatePool(_bloodEmitterDamagePrefab);
            _deathPool = CreatePool(_bloodEmitterKillPrefab);
        }

        private IObjectPool<BloodEmitter> CreatePool(BloodEmitter prefab) =>
            new ObjectPool<BloodEmitter>(
                createFunc: () => CreateEmitter(prefab),
                actionOnGet: OnTake,
                actionOnRelease: OnRelease,
                actionOnDestroy: OnDestroyEmitter,
                collectionCheck: _collectionCheck,
                defaultCapacity: _defaultCapacity,
                maxSize: _maxPoolSize
            );

        private BloodEmitter CreateEmitter(BloodEmitter prefab)
        {
            BloodEmitter emitter = Instantiate(prefab, transform);
            emitter.gameObject.SetActive(false);
            return emitter;
        }

        private void OnTake(BloodEmitter emitter)
        {
            emitter.gameObject.SetActive(true);
            _activeBloodEmitters.Add(emitter);
        }

        private void OnRelease(BloodEmitter emitter)
        {
            emitter.gameObject.SetActive(false);
            _activeBloodEmitters.Remove(emitter);
        }

        private void OnDestroyEmitter(BloodEmitter emitter) => Destroy(emitter.gameObject);

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}