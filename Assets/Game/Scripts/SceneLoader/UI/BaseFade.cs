using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Game.SceneLoaderSystem
{
    [RequireComponent(typeof(Image))]
    public class BaseFade : MonoBehaviour
    {
        [Header("Reference")]
        [SerializeField] private FadeSpotUI _spotPrefab;

        [Header("Spawn Settings")]
        [SerializeField] private List<Sprite> _bloodTypes;
        [SerializeField] private float _spawnDuration = 0.8f;
        [SerializeField] private float _spotBaseSize = 100.0f;
        [SerializeField] private float _overlapFactor = 0.65f;
        [SerializeField] private Vector2 _scaleInterval = new Vector2(1.5f, 3.0f);
        [SerializeField] private Vector2 _saturationInterval = new Vector2(0.6f, 1.0f);
        [SerializeField] private Color _bloodColor = Color.red;

        [Header("Background")]
        [SerializeField] private float _bgFadeDuration = 0.4f;

        private readonly List<FadeSpotUI> _activeSpots = new();
        private Image _background;

        private void Awake()
        {
            _background = GetComponent<Image>();
            _background.color = new Color(_bloodColor.r, _bloodColor.g, _bloodColor.b, 0f);
            gameObject.SetActive(false);
        }

        public void FadeOut(Action callback = null)
        {
            gameObject.SetActive(true);

            StartCoroutine(SpawnRoutine(callback));
        }

        public void FadeIn(Action callback = null) => StartCoroutine(DissolveRoutine(callback));

        private IEnumerator SpawnRoutine(Action callback)
        {
            List<Vector2> positions = BuildGrid();
            Shuffle(positions);

            float interval = _spawnDuration / positions.Count;
            int half = positions.Count / 2;

            for (int i = 0; i < positions.Count; i++)
            {
                if (i == half)
                {
                    _background.DOFade(1.0f, _bgFadeDuration).SetEase(Ease.InQuad);
                }

                SpawnSpot(positions[i]);
                yield return new WaitForSeconds(interval);
            }

            callback?.Invoke();
        }

        private IEnumerator DissolveRoutine(Action callback)
        {
            _background.DOFade(0f, _bgFadeDuration).SetEase(Ease.OutQuad);

            float maxDuration = 0f;
            foreach (var spot in _activeSpots)
            {
                float d = spot.Dissolve();
                if (d > maxDuration) maxDuration = d;
            }

            yield return new WaitForSeconds(maxDuration);

            _background.color = new Color(_bloodColor.r, _bloodColor.g, _bloodColor.b, 0f);
            _activeSpots.Clear();
            gameObject.SetActive(false);
            callback?.Invoke();
        }

        private List<Vector2> BuildGrid()
        {
            var positions = new List<Vector2>();

            float cellSize = (_scaleInterval.x + _scaleInterval.y) * 0.5f * _spotBaseSize;
            float step = cellSize * _overlapFactor;
            float margin = cellSize;

            var canvas = GetComponentInParent<Canvas>();
            var canvasRect = canvas.GetComponent<RectTransform>();
            float halfW = canvasRect.rect.width * 0.5f;
            float halfH = canvasRect.rect.height * 0.5f;

            for (float x = -halfW - margin; x <= halfW + margin; x += step)
            {
                for (float y = -halfH - margin; y <= halfH + margin; y += step)
                {
                    float jitterX = UnityEngine.Random.Range(-step * 0.3f, step * 0.3f);
                    float jitterY = UnityEngine.Random.Range(-step * 0.3f, step * 0.3f);
                    positions.Add(new Vector2(x + jitterX, y + jitterY));
                }
            }

            return positions;
        }

        private void SpawnSpot(Vector2 pos)
        {
            FadeSpotUI spot = Instantiate(_spotPrefab, transform);
            _activeSpots.Add(spot);

            Sprite sprite = _bloodTypes[UnityEngine.Random.Range(0, _bloodTypes.Count)];

            Color.RGBToHSV(_bloodColor, out float h, out float s, out _);
            float v = UnityEngine.Random.Range(_saturationInterval.x, _saturationInterval.y);
            Color color = Color.HSVToRGB(h, s, v);

            float scale = UnityEngine.Random.Range(_scaleInterval.x, _scaleInterval.y);
            float angle = UnityEngine.Random.Range(0f, 360f);

            spot.Initialize(sprite, color, pos, angle, scale);
            spot.Appear();
        }

        private void Shuffle(List<Vector2> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = UnityEngine.Random.Range(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }

        private void OnDestroy() => DOTween.Kill(_background);
    }
}