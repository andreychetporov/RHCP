using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UltaBackAnim : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Vector2 _values = Vector2.up;
    [SerializeField] private float _duration = 0.15f;
    [SerializeField] private Ease _ease = Ease.Linear;

    private Image _image;

    private Tween _tween;

    private void Awake()
    {
        _image = GetComponent<Image>();    
    }

    private void OnEnable()
    {
        _tween?.Kill();

        float currentAlpha = _values.x;

        _tween = DOTween.To(a => currentAlpha = a, currentAlpha, _values.y, _duration)
                        .OnUpdate(() =>
                        {
                            Color c = _image.color;
                            c.a = currentAlpha;
                            _image.color = c;
                        })
                        .SetLoops(-1, LoopType.Yoyo)
                        .SetEase(_ease);
    }

    private void OnDisable()
    {
        _tween?.Kill();
    }
}
