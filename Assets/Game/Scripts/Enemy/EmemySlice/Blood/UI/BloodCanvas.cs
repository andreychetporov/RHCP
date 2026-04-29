using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Game.Blood
{
    public class BloodCanvas : MonoBehaviour
    {
        public static BloodCanvas Instance { get; private set; }

        [Header("Settings")]
        [SerializeField] private List<Sprite> _bloodTypes;
        [SerializeField] private Vector2 _saturationInterval = new Vector2(0.6f, 1.0f);
        [SerializeField] private Vector2 _scaleInterval = new Vector2(0.5f, 1.0f);
        [SerializeField] private float _centerDeadZoneRadius = 0.2f;

        [Header("Pool Settings")]
        [SerializeField] private bool _collectionCheck = true;
        [SerializeField] private int _defaultCapacity = 10;
        [SerializeField] private int _maxPoolSize = 50;

        [Header("Reference")]
        [SerializeField] private BloodSpotUI _spotPrefab;

        private IObjectPool<BloodSpotUI> _spotPool;
        private List<BloodSpotUI> _activeSpots;

        private RectTransform _rectTransform;

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

            _rectTransform = (RectTransform)transform;
            _activeSpots = new List<BloodSpotUI>();
        }

        private void Start() => InitializePool();

        public void SpawnBloodSpot(Color color)
        {
            BloodSpotUI spot = _spotPool.Get();

            Sprite sprite = _bloodTypes[Random.Range(0, _bloodTypes.Count)];

            float h, s, v;
            Color.RGBToHSV(color, out h, out s, out v);
            v = Random.Range(_saturationInterval.x, _saturationInterval.y);
            Color finalColor = Color.HSVToRGB(h, s, v);
            finalColor.a = color.a;

            float randomAngle = Random.Range(0.0f, 360.0f);
            float randomScale = Random.Range(_scaleInterval.x, _scaleInterval.y);
            Vector2 randomPos = GetRandomPosition();

            spot.Initialize(sprite, finalColor, randomPos, randomAngle, randomScale).Play();
        }

        private Vector2 GetRandomPosition()
        {
            float halfW = _rectTransform.rect.width * 0.5f;
            float halfH = _rectTransform.rect.height * 0.5f;

            float deadZonePx = Mathf.Min(halfW, halfH) * _centerDeadZoneRadius;

            for (int i = 0; i < 10; i++)
            {
                Vector2 candidate = new Vector2(
                    Random.Range(-halfW, halfW),
                    Random.Range(-halfH, halfH));

                if (candidate.magnitude >= deadZonePx) { return candidate; }
            }

            float angle = Random.Range(0f, Mathf.PI * 2f);
            float radius = Mathf.Lerp(deadZonePx, Mathf.Min(halfW, halfH), Random.value);

            return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
        }

        public void Release(BloodSpotUI spot) => _spotPool.Release(spot);

        private void InitializePool()
        {
            _spotPool = new ObjectPool<BloodSpotUI>
            (
                createFunc: CreateSpot,
                actionOnGet: OnTake,
                actionOnRelease: OnRelease,
                actionOnDestroy: OnDestroySpot,
                collectionCheck: _collectionCheck,
                defaultCapacity: _defaultCapacity,
                maxSize: _maxPoolSize
            );
        }

        private BloodSpotUI CreateSpot()
        {
            BloodSpotUI spot = Instantiate(_spotPrefab, transform);
            spot.gameObject.SetActive(false);
            return spot;
        }

        private void OnTake(BloodSpotUI spot)
        {
            spot.gameObject.SetActive(true);
            _activeSpots.Add(spot);
        }

        private void OnRelease(BloodSpotUI spot)
        {
            spot.gameObject.SetActive(false);
            _activeSpots.Remove(spot);
        }

        private void OnDestroySpot(BloodSpotUI spot) => Destroy(spot.gameObject);

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}