using Game.Level;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class StartZone : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string _playerTag = "Player";
    [SerializeField] private bool _disableAfterStart = true;

    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(_playerTag)) { return; }

        LevelBootstrap.Instance.StartRun();

        if (_disableAfterStart) { _collider.enabled = false; }
    }
}