using System.Collections.Generic;
using UnityEngine;

public class MapLevelContianer : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] public Transform _player;
    [SerializeField] public Transform _startPoint;
    [SerializeField] public List<LevelButton> _levelButtons;
}
