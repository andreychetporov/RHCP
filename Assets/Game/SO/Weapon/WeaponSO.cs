using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "Scriptable Objects/WeaponSO")]
public class WeaponSO : ScriptableObject
{
    public string weaponName;
    public GameObject model;
    public Material trailMaterial;
    public int damage;
    public AudioClip audioClip;
    public float damageRadius;
}
