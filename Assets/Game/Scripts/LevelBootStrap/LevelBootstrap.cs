using UnityEngine;

namespace Game.Level
{
    public class LevelBootstrap : MonoBehaviour
    {
        [Header("Reference")]
        [SerializeField] private Transform _player;

        public static LevelBootstrap Instance { get; private set; }



        public Vector3 GetPlayerPosition => _player.transform.position;

        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
                return;
            }
        }

        public void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}