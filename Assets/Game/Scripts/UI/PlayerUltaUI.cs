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

    private void OnUltaPointsChanged(float arg1, float arg2)
    {
        image.fillAmount += 0.01f;
    }
}
