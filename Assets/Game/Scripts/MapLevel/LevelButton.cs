using Game.SceneLoaderSystem;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LevelButton : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Image _arrowImage;

    [Header("Settings")]
    [SerializeField] public int LevelIndex;
    [SerializeField] private float _bobAmplitude = 10f;
    [SerializeField] private float _bobDuration = 0.4f;

    private Button _button;
    private Tween _bobTween;

    private Vector2 _arrowStartPos;

    private bool _isActive = false;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() => LevelController.Instance.GoToLevel(this));
        _arrowStartPos = _arrowImage.rectTransform.anchoredPosition;
        _arrowImage.gameObject.SetActive(false);
    }

    private void Start()
    {
        _isActive = LevelController.Instance.CurrentLevel + 1 == LevelIndex;

        _button.interactable = _isActive;

        _arrowImage.gameObject.SetActive(_isActive);
        if (_isActive)
        {
            _arrowImage.rectTransform.anchoredPosition = _arrowStartPos;
            _bobTween = _arrowImage.rectTransform
                                   .DOAnchorPosY(_arrowStartPos.y + _bobAmplitude, _bobDuration)
                                   .SetEase(Ease.InOutSine)
                                   .SetLoops(-1, LoopType.Yoyo);
        }
    }

    private void OnDestroy()
    {
        _bobTween?.Kill();
    }
}