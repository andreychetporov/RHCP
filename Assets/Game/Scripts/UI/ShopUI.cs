using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private WeaponSO[] availableWeapons;
    [SerializeField] private Button[] slots;
    [SerializeField] private BuyPopupUI buyPopup;
    
    private void Start()
    {
        InitSlots();

    }

    private void InitSlots()
    {


        int slotsAmount = Mathf.Min(slots.Length, availableWeapons.Length);
        for (int i = 0; i < slotsAmount; i++)
        {
            if (slots[i] != null && availableWeapons[i] != null )
            {
                int index = i;
                slots[i].image.sprite = availableWeapons[i].Icon;
                slots[i].onClick.AddListener(() => ShowPopup(availableWeapons[index], slots[index])); 
            }
        }

        for (int i=0; i<slots.Length; i++)
        {
            if (slots[i].image.sprite == null)
            {
                slots[i].gameObject.SetActive(false);
            }
        }
    }

    private void ShowPopup(WeaponSO weaponSO, Button slot)
    {
        buyPopup.gameObject.SetActive(true);
        buyPopup.InitPopup(weaponSO, slot);
    }
}
