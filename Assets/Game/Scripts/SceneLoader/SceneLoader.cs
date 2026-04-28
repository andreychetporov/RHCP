using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.SceneLoaderSystem
{
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader Instance { get; private set; }

        public event Action OnStartLoadTargetScene;
        public event Action<float> OnLoadingProgress;
        public event Action OnFinishLoadTargetScene;

        [Header("References")]
        [SerializeField] private BaseFade _fade;

        [Header("Config")]
        [SerializeField] private float _minLoadTime = 2.0f;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;

                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void LoadScene(SceneEnum target) => StartCoroutine(TransitionRoutine(() => SceneManager.LoadSceneAsync((int)target)));
        public void LoadScene(int sceneIndex) => StartCoroutine(TransitionRoutine(() => SceneManager.LoadSceneAsync(sceneIndex)));
        public void LoadScene(string sceneName) => StartCoroutine(TransitionRoutine(() => SceneManager.LoadSceneAsync(sceneName)));


        private IEnumerator TransitionRoutine(Func<AsyncOperation> loadFactory)
        {
            yield return FadeOutRoutine();
            OnStartLoadTargetScene?.Invoke();

            var op = loadFactory();
            op.allowSceneActivation = false;

            float elapsed = 0f;

            while (true)
            {
                elapsed += Time.deltaTime;
                float realProgress = Mathf.Clamp01(op.progress / 0.9f);
                float timerProgress = Mathf.Clamp01(elapsed / _minLoadTime);
                float progress = Mathf.Min(realProgress, timerProgress);

                OnLoadingProgress?.Invoke(progress);

                if (progress >= 1f)
                {
                    OnLoadingProgress?.Invoke(1f);
                    break;
                }
                yield return null;
            }

            op.allowSceneActivation = true;
            yield return op;

            OnFinishLoadTargetScene?.Invoke();
            yield return FadeInRoutine();
        }

        private IEnumerator FadeOutRoutine()
        {
            bool done = false;

            _fade.FadeOut(() => done = true);

            yield return new WaitUntil(() => done);
        }

        private IEnumerator FadeInRoutine()
        {
            bool done = false;

            _fade.FadeIn(() => done = true);

            yield return new WaitUntil(() => done);
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }
    }
}