using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatsSO", menuName = "Scriptable Objects/PlayerStatsSO")]
public class PlayerStatsSO : ScriptableObject
{
    public ReactiveVariable<int> Health;
    public ReactiveVariable<int> Coins;
    public ReactiveVariable<int> UltaPoints;
}
