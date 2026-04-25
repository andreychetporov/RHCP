using UnityEngine.Audio;

namespace Game.Audio
{
    [System.Serializable]
    public class SoundData
    {
        public AudioResource AudioResource;
        public AudioMixerGroup AudioMixerGroup;
        public bool IsLooping;
        public bool PlayOnAwake;
    }
}