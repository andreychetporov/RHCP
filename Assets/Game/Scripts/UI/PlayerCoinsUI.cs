using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCoinsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinsAmount;
    private void Start()
    {
        PlayerStatsSO playerStatsSO = GetComponentInParent<PlayerStatsSO>();
        playerStatsSO.Coins.OnValueChanged += OnCoinsAdded;
    }

    private void OnCoinsAdded(int arg1, int newValue)
    {
        coinsAmount.text = newValue.ToString();
    }

}
