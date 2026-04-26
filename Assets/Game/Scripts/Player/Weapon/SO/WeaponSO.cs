using Game.Audio;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "Scriptable Objects/WeaponSO")]
public class WeaponSO : ScriptableObject
{
    public string WeaponName;
    public string Description;

    [Space()]

    public GameObject Model;
    public Material TrailMaterial;
    public SoundData HitSound;

    [Space()]

    public int Damage;
    public float DamageRadius;
}
