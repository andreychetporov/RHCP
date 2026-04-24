using Game.Enemy.Slice;
using UnityEngine;
using Zenject;

namespace Game.Level
{
    public class LevelBootstrap : MonoBehaviour
    {
        public static LevelBootstrap Instance { get; private set; }

        private PlayerController _playerController;
        public Transform GetPlayerTransform
        {
            get
            {
                if (_playerController == null)
                {
                    _playerController = FindAnyObjectByType<PlayerController>();
                }

                return _playerController.transform;
            }
        }

        [Inject] public IEnemySliceFactory EnemySliceFactory { get; private set; }

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