using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "Scriptable Objects/WeaponSO")]
public class WeaponSO : ScriptableObject
{
    public Material trailMaterial;
    public int damage;
    public AudioClip audioClip;
    public float damageRadius;
}
