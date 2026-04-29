using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCoinsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinsAmount;
    [SerializeField] PlayerStatsSO playerStatsSO;
    private void Start()
    {
        playerStatsSO.Coins.OnValueChanged += OnCoinsAdded;
        coinsAmount.text = playerStatsSO.Coins.Value.ToString();
    }

    private void OnCoinsAdded(int arg1, int newValue)
    {
        coinsAmount.text = newValue.ToString();
    }

}
