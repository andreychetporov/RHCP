using Game.SceneLoaderSystem;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    //private const string LEVEL_SAVE = "LEVEL_INDEX_DATA";
    public static LevelController Instance { get; private set; }

    [Header("Reference")]
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _startPoint;
    [SerializeField] private List<LevelButton> _levelButtons;

    [Header("Settings")]
    [SerializeField] private List<string> _levels;
    [SerializeField] private float _jumpPower = 2f;
    [SerializeField] private float _jumpDuration = 0.6f;
    [SerializeField] private int _numJumps = 1;

    public int CurrentLevel { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else { Destroy(gameObject); return; }

        //CurrentLevel = PlayerPrefs.HasKey(LEVEL_SAVE) ? PlayerPrefs.GetInt(LEVEL_SAVE) : 0;
    }

    private void Start()
    {
        PlacePlayerInstant();
    }

    private void PlacePlayerInstant()
    {
        if (CurrentLevel == 0)
        {
            _player.position = _startPoint.position;
            return;
        }

        var btn = _levelButtons.Find(b => b.LevelIndex == CurrentLevel);
        if (btn != null)
            _player.position = btn.transform.position;
    }

    public void GoToLevel(LevelButton button)
    {
        if (button.LevelIndex != CurrentLevel + 1) return;

        SetButtonsInteractable(false);

        _player.DOJump(button.transform.position, _jumpPower, _numJumps, _jumpDuration, true)
                         .SetEase(Ease.Linear)
                         .OnComplete(() =>
                         {
                             CurrentLevel++;
                             //PlayerPrefs.SetInt(LEVEL_SAVE, CurrentLevel);
                             SceneLoader.Instance.LoadScene(_levels[CurrentLevel - 1]);
                         });
    }

    private void SetButtonsInteractable(bool value)
    {
        foreach (var btn in _levelButtons)
            btn.GetComponent<UnityEngine.UI.Button>().interactable = value;
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }
}