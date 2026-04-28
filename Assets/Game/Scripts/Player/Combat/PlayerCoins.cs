using UnityEngine;

public class PlayerCoins : MonoBehaviour
{
    [SerializeField] private PlayerStatsSO playerStatsSO;
    [SerializeField] private GameEvent onCoinCollected;

    private void Start()
    {
        onCoinCollected.OnInvoked += OnCoinCollected;
    }

    private void OnCoinCollected()
    {
        playerStatsSO.Coins.Value++;
    }
}
