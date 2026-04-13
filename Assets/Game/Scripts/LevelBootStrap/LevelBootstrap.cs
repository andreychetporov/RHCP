using UnityEngine;

namespace Game.Level
{
    public class LevelBootstrap : MonoBehaviour
    {
        public static LevelBootstrap Instance { get; private set; }

        public Vector3 GetPlayerPosition => transform.position;

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