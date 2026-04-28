using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyPopupUI : MonoBehaviour
{
    [SerializeField] private Image weaponImage;
    [SerializeField] private TextMeshProUGUI weaponDamage;
    [SerializeField] private TextMeshProUGUI weaponPrice;
    [SerializeField] private TextMeshProUGUI weaponDescription;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button buyButton;
    [SerializeField] private PlayerStatsSO playerStatsSO;

    private WeaponSO weapon;
    private Button slot;
    private void Start()
    {
        buyButton.onClick.AddListener(()=>BuyWeapon());
        closeButton.onClick.AddListener(()=>ClosePopup());
    }

    public void InitPopup(WeaponSO weaponSO, Button slot)
    {
        this.slot = slot;
        weapon = weaponSO;
        weaponImage.sprite = weapon.Icon;
        weaponPrice.text = "Цена: " + weapon.Price.ToString();
        weaponDamage.text = "Урон: " + weapon.Damage.ToString();
        weaponDescription.text = "Описание: " + weapon.Description.ToString();
    }

    private void BuyWeapon()
    {
        playerStatsSO.Coins.Value = 1000;
        if (playerStatsSO.Coins.Value < weapon.Price) return;
        playerStatsSO.Coins.Value -= weapon.Price;
        playerStatsSO.availableWeapons.Add(weapon);
        slot.gameObject.SetActive(false);
        ClosePopup();
    }
    private void ClosePopup()
    {
        gameObject.SetActive(false);
    }
}
