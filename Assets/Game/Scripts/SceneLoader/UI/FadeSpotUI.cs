using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Game.SceneLoaderSystem
{
    [RequireComponent(typeof(Image), typeof(CanvasGroup))]
    public class FadeSpotUI : MonoBehaviour
    {
        [Header("Appear")]
        [SerializeField] private float _appearDuration = 0.3f;
        [SerializeField] private float _overshoot = 1.15f;

        [Header("Dissolve")]
        [SerializeField] private float _dissolveDuration = 0.5f;

        private Image _image;
        private CanvasGroup _canvasGroup;
        private RectTransform _rectTransform;
        private float _targetScale;
        private Sequence _sequence;

        private void Awake()
        {
            _image = GetComponent<Image>();
            _canvasGroup = GetComponent<CanvasGroup>();
            _rectTransform = (RectTransform)transform;
        }

        public void Initialize(Sprite sprite, Color color, Vector2 pos, float angle, float scale)
        {
            _image.sprite = sprite;
            _image.color = color;
            _rectTransform.anchoredPosition = pos;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
            transform.localScale = Vector3.zero;
            _canvasGroup.alpha = 0f;
            _targetScale = scale;
        }

        public void Appear()
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence()
                .Append(transform.DOScale(_targetScale * _overshoot, _appearDuration).SetEase(Ease.OutQuad))
                .Join(_canvasGroup.DOFade(1f, _appearDuration).SetEase(Ease.OutQuad))
                .Append(transform.DOScale(_targetScale, 0.1f).SetEase(Ease.InOutQuad));
        }

        public float Dissolve()
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence()
                .Append(_canvasGroup.DOFade(0f, _dissolveDuration).SetEase(Ease.InQuad))
                .OnComplete(() => Destroy(gameObject));
            return _dissolveDuration;
        }

        private void OnDestroy() => _sequence?.Kill();
    }
}