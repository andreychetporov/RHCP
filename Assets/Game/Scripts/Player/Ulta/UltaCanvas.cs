using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UltaCanvas : MonoBehaviour
{
    [SerializeField] private GameEvent OnUltaStart;
    [SerializeField] private GameEvent OnUltaEnd;
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();    
    }

    public void Start()
    {
        OnUltaStart.OnInvoked += Show;
        OnUltaEnd.OnInvoked += Hide;
        gameObject.SetActive(false);
    }


    public void Show()
    {
        gameObject.SetActive(true);

        _canvasGroup.DOFade(1.0f, 0.15f).From(0.0f);
    }

    public void Hide()
    {
        _canvasGroup.DOFade(0.0f, 0.15f).From(1.0f).OnComplete(() => gameObject.SetActive(false));
    }
}
