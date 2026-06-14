using Game.Level;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FinishZone : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string _playerTag = "Player";
    [SerializeField] private bool _requireStartedRun = true;

    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(_playerTag)) { return; }

        //if (_requireStartedRun && !LevelBootstrap.Instance.IsRunStarted) { return; }

        LevelBootstrap.Instance.FinishRun();
    }
}