using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Game.SceneLoaderSystem
{
    public class LoadProgressUI : MonoBehaviour
    {
        [Header("Reference")]
        [SerializeField] private TextMeshProUGUI _progressPrecent;

        private float _displayedProgress = 0.0f;

        private void Start()
        {
            SceneLoader.Instance.OnStartLoadTargetScene += SceneLoader_OnStartLoadTargetScene;
            SceneLoader.Instance.OnLoadingProgress += SceneLoader_OnLoadingProgress;
            SceneLoader.Instance.OnFinishLoadTargetScene += SceneLoader_OnFinishLoadTargetScene;

            gameObject.SetActive(false);
        }

        private void SceneLoader_OnStartLoadTargetScene()
        {
            _displayedProgress = 0.0f;

            gameObject.SetActive(true);
        }

        private void SceneLoader_OnFinishLoadTargetScene() => gameObject.SetActive(false);

        private void SceneLoader_OnLoadingProgress(float progress)
        {
            DOTween.To(
                () => _displayedProgress,
                x =>
                {
                    _displayedProgress = x;
                    _progressPrecent.text = Mathf.FloorToInt(x * 100.0f) + "%";
                },
                progress,
                0.05f)
            .SetEase(Ease.OutQuad);
        }

        private void OnDestroy()
        {
            SceneLoader.Instance.OnStartLoadTargetScene -= SceneLoader_OnStartLoadTargetScene;
            SceneLoader.Instance.OnLoadingProgress -= SceneLoader_OnLoadingProgress;
            SceneLoader.Instance.OnFinishLoadTargetScene -= SceneLoader_OnFinishLoadTargetScene;
        }
    }
}