using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "PlayerStatsSO", menuName = "Scriptable Objects/PlayerStatsSO")]
public class PlayerStatsSO : ScriptableObject
{
    public ReactiveVariable<int> Health = new ReactiveVariable<int>();
    public ReactiveVariable<int> Coins = new ReactiveVariable<int>();
    public ReactiveVariable<float> UltaPoints = new ReactiveVariable<float>();

    [Space()]

    public List<WeaponSO> availableWeapons = new();
    public WeaponSO currentWeapon;
}
