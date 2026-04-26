using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Blood
{
    [RequireComponent(typeof(Image), typeof(CanvasGroup))]
    public class BloodSpotUI : MonoBehaviour
    {
        [Header("Lifetime")]
        [SerializeField] private float _lifeTime = 2.0f;

        [Header("Appear")]
        [SerializeField] private float _appearDuration = 0.25f;
        [SerializeField] private float _scaleOvershoot = 1.2f;
        [SerializeField] private float _settleDuration = 0.1f;

        [Header("Disappear")]
        [SerializeField] private float _fadeDuration = 0.4f;

        private Image _image;
        private CanvasGroup _canvasGroup;
        private RectTransform _rectTransform;

        private Coroutine _coroutine;
        private Sequence _sequence;

        private float _targetScale = 1.0f;

        private void Awake()
        {
            _image = GetComponent<Image>();
            _canvasGroup = GetComponent<CanvasGroup>();
            _rectTransform = (RectTransform)transform;
        }

        public BloodSpotUI Initialize(Sprite sprite, Color color, Vector2 pos, float angle = 0f, float scale = 1.0f)
        {
            _image.sprite = sprite;
            _image.color = color;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
            transform.localScale = Vector3.one * scale;
            _rectTransform.anchoredPosition = pos;

            _targetScale = scale;

            return this;
        }

        public void Play()
        {
            StopAll();
            _coroutine = StartCoroutine(LifeCoroutine());
        }

        public void Stop()
        {
            StopAll();
            BloodCanvas.Instance.Release(this);
        }

        private IEnumerator LifeCoroutine()
        {
            _sequence = DOTween.Sequence();

            _sequence
                .Append(transform
                    .DOScale(_scaleOvershoot * _targetScale, _appearDuration)
                    .From(0f)
                    .SetEase(Ease.OutQuad))
                .Append(transform
                    .DOScale(_targetScale, _settleDuration)
                    .SetEase(Ease.InOutQuad))
                .Join(_canvasGroup
                    .DOFade(1f, _appearDuration)
                    .From(0f)
                    .SetEase(Ease.OutQuad));

            yield return _sequence.WaitForCompletion();

            yield return new WaitForSeconds(_lifeTime);

            Tween fadeTween = _canvasGroup
                .DOFade(0f, _fadeDuration)
                .SetEase(Ease.InQuad);

            yield return fadeTween.WaitForCompletion();

            BloodCanvas.Instance.Release(this);
        }

        private void StopAll()
        {
            if (_coroutine != null) { StopCoroutine(_coroutine); _coroutine = null; }

            _sequence?.Kill();
        }
    }
}