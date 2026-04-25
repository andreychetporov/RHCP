using System.Collections;
using UnityEngine;

namespace Game.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundEmitter : MonoBehaviour
    {
        private AudioSource _audioSource;
        private Coroutine _playingCoroutine;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void Play()
        {
            if (_playingCoroutine != null) { StopCoroutine(_playingCoroutine); }
            
            _audioSource.Play();
            _playingCoroutine = StartCoroutine(WaitForSoundToEnd());
        }

        public void Stop()
        {
            if (_playingCoroutine != null) { StopCoroutine(_playingCoroutine); }
            _playingCoroutine = null;

            _audioSource.Stop();
            SoundManager.Instance.Realise(this);
        }

        private IEnumerator WaitForSoundToEnd()
        {
            yield return new WaitWhile(() => _audioSource.isPlaying);

            SoundManager.Instance.Realise(this);
        }

        public SoundEmitter Initialize(SoundData soundData)
        {
            _audioSource.resource = soundData.AudioResource;
            _audioSource.outputAudioMixerGroup = soundData.AudioMixerGroup;
            _audioSource.loop = soundData.IsLooping;
            _audioSource.playOnAwake = soundData.PlayOnAwake;

            return this;
        }
    }
}