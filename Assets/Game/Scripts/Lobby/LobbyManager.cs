using DG.Tweening;
using Game.SceneLoaderSystem;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class LobbyManager : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private TMP_Text _continueText;
    private void OnEnable()
    {
        InputSystem.onAnyButtonPress.CallOnce(OnAnyButtonPressed);
    }

    private void Start()
    {
        float alpha = 0.0f;
        LevelController.Instance.CurrentLevel = 0;
        DOTween.To(() => alpha, a => alpha = a, 1.0f, 1.0f)
               .OnUpdate(() =>
               {
                   Color baseColor = _continueText.color;
                   baseColor.a = alpha;
                   _continueText.color = baseColor;
               })
               .SetLoops(-1, LoopType.Yoyo)
               .SetEase(Ease.InOutSine);
    }


    private void OnAnyButtonPressed(InputControl control)
    {
        SceneLoader.Instance.LoadScene(SceneEnum.MapLevel);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
