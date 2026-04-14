using UnityEngine;

[RequireComponent(typeof(Collider))]
public class StartZone : MonoBehaviour
{
    [SerializeField] private RunLevelController _runLevelController;
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
        if (!other.CompareTag(_playerTag))
            return;

        if (_runLevelController == null)
        {
            Debug.LogWarning("StartZone: не назначен RunLevelController");
            return;
        }

        _runLevelController.StartRun();

        if (_disableAfterStart)
        {
            _collider.enabled = false;
        }
    }
}