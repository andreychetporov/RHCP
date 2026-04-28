using Game.Audio;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "Scriptable Objects/WeaponSO")]
public class WeaponSO : ScriptableObject
{
    public string WeaponName;
    public string Description;

    [Space()]

    public Sprite Icon;
    public Material TrailMaterial;
    public SoundData HitSound;

    [Space()]

    public int Damage;
    public float DamageRadius;

    [Space()]

    public int Price;
}
