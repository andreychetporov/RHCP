using UnityEngine;

public class PlayerUlta : MonoBehaviour
{
    [SerializeField] private PlayerStatsSO playerStatsSO;
    [SerializeField] private GameEvent onUltaCollected;

    private void Start()
    {
        onUltaCollected.OnInvoked += OnUltaCollected;
    }

    private void OnUltaCollected()
    {
        playerStatsSO.UltaPoints.Value += 1.0f;
    }
}
