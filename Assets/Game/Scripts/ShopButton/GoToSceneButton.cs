using Game.SceneLoaderSystem;
using UnityEngine;
using UnityEngine.UI;

public class GoToSceneButton : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string _sceneName;
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => SceneLoader.Instance.LoadScene(_sceneName));
    }
}
