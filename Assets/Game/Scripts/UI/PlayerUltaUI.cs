using UnityEngine;
using UnityEngine.UI;

public class PlayerUltaUI : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] PlayerStatsSO playerStatsSO;

    private void Start()
    {
        playerStatsSO.UltaPoints.OnValueChanged += OnUltaPointsChanged;
    }

    private void OnUltaPointsChanged(float oldValue, float newValue)
    {
        image.fillAmount = newValue;
    }
}
