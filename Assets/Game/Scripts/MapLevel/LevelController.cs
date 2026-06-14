using DG.Tweening;
using Game.Audio;
using Game.SceneLoaderSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private List<string> _levels;
    [SerializeField] private float _jumpPower = 2f;
    [SerializeField] private float _jumpDuration = 0.6f;
    [SerializeField] private int _numJumps = 1;
    [SerializeField] private SoundData ambient;
    public int CurrentLevel { get; set; } = 0;

    private MapLevelContianer _temp;
    private MapLevelContianer _contianer
    {
        get
        {
            if (_temp == null)
            {
                _temp = FindAnyObjectByType<MapLevelContianer>();
            }

            return _temp;
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
            DontDestroyOnLoad(gameObject);
        }
        else { Destroy(gameObject); return; }
    }

    private void PlacePlayerInstant()
    {
        if (_contianer == null) return;

        if (CurrentLevel == 0)
        {
            _contianer._player.position = _contianer._startPoint.position;
            return;
        }

        var btn = _contianer._levelButtons.Find(b => b.LevelIndex == CurrentLevel);
        if (btn != null)
        {
            _contianer._player.position = btn.transform.position;
        }
    }

    public void GoToLevel(LevelButton button)
    {
        if (button.LevelIndex != CurrentLevel + 1) return;

        SetButtonsInteractable(false);

        _contianer._player.DOJump(button.transform.position, _jumpPower, _numJumps, _jumpDuration, true)
                         .SetEase(Ease.Linear)
                         .OnComplete(() =>
                         {
                             SoundManager.Instance.StopAllSound();
                             SceneLoader.Instance.LoadScene(_levels[button.LevelIndex - 1]);
                         });
    }

    public void CompleteCurrentLevel()
    {
        CurrentLevel++;
    }

    public bool IsFinalLevel()
    {
        return CurrentLevel == (_levels.Count - 1);
    }

    private void SetButtonsInteractable(bool value)
    {
        if (_contianer == null || _contianer._levelButtons == null) return;

        foreach (var btn in _contianer._levelButtons)
        {
            if (btn != null && btn.GetComponent<UnityEngine.UI.Button>() != null)
                btn.GetComponent<UnityEngine.UI.Button>().interactable = value;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _temp = null;
        PlacePlayerInstant();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}