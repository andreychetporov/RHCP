using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;

public class EquipManager : MonoBehaviour
{
    [SerializeField] private PlayerStatsSO playerStats;
    [SerializeField] private Button[] weapons;
    [SerializeField] private Image currentWeapon;

    
    private void OnEnable()
    {
        currentWeapon.sprite = playerStats.currentWeapon.Icon;

        List<WeaponSO> playerWeapons = playerStats.availableWeapons;
        for (int i = 0; i < playerWeapons.Count; i++) {
            int index = i;
            weapons[i].image.sprite = playerWeapons[i].Icon;
            weapons[i].onClick.AddListener(() => SetCurrentWeapon(playerWeapons[index]));
        }
        for (int i = playerWeapons.Count; i < weapons.Length; i++)
        {
            weapons[i].gameObject.SetActive(false);
        }
    }
    public void SetCurrentWeapon(WeaponSO newWeapon)
    {
        playerStats.currentWeapon = newWeapon;
        currentWeapon.sprite = newWeapon.Icon;
    }
}
