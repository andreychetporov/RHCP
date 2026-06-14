using UnityEngine;
using UnityEngine.UI;

public class PlayerUltaUI : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private PlayerStatsSO playerStatsSO;

    private void Start()
    {
        if (playerStatsSO != null && playerStatsSO.UltaPoints != null)
        {
            playerStatsSO.UltaPoints.OnValueChanged += OnUltaPointsChanged;

        }
    }

    private void OnUltaPointsChanged(float oldValue, float newValue)
    {
        if (image != null)
        {
            image.fillAmount = newValue;
        }
    }

    private void OnDestroy()
    {
        if (playerStatsSO != null && playerStatsSO.UltaPoints != null)
        {
            playerStatsSO.UltaPoints.OnValueChanged -= OnUltaPointsChanged;
        }
    }
}