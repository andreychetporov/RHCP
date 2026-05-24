using System;
using System.Collections;
using UnityEngine;

public class PlayerUlta : MonoBehaviour
{
    [SerializeField] float ultaActivatePointsAmount = 30.0f;
    [SerializeField] float ultaDuration = 5.0f;
    [SerializeField] WeaponSO ultaWeapon;
    [SerializeField] GameObject ultaVFX;

    [SerializeField] private PlayerStatsSO playerStatsSO;
    [SerializeField] private GameEvent onUltaCollected;
    [SerializeField] private GameEvent ultaStart; 
    [SerializeField] private GameEvent ultaEnd; 
    private void Start()
    {
        onUltaCollected.OnInvoked += OnUltaCollected;
    }

    private void OnUltaCollected()
    {   
        playerStatsSO.UltaPoints.Value += 1.0f / ultaActivatePointsAmount;
        playerStatsSO.UltaPoints.Value = Math.Min(playerStatsSO.UltaPoints.Value, 1.0f);
    }

    public void ApplyUlta(Action<WeaponSO> changeWeapon, WeaponSO currentWeapon)
    {
        if (playerStatsSO.UltaPoints.Value >= 1.0f)
        {
            StartCoroutine(UltaWaste(changeWeapon, currentWeapon));
        }
    }

    IEnumerator UltaWaste(Action<WeaponSO> changeWeapon, WeaponSO currentWeapon)
    {
        ultaStart.Raise();
        ultaVFX.SetActive(true);
        changeWeapon(ultaWeapon);
           
        float timer = 0f;

        while (timer < ultaDuration)
        {
            timer += Time.deltaTime;
            playerStatsSO.UltaPoints.Value = Mathf.Lerp(1.0f, 0.0f, timer/ultaDuration);
            yield return null;
        }
        ultaEnd.Raise();
        ultaVFX.SetActive(false);
        changeWeapon(currentWeapon);
    }
}
