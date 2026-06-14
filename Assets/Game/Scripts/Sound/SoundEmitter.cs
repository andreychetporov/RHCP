using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Game.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundEmitter : MonoBehaviour
    {
        private AudioSource _audioSource;
        private Coroutine _playingCoroutine;
        private bool _isPaused = false;
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
        public void Pause()
        {
            _isPaused = true;

            _audioSource.Pause();
        }
        public void UnPause()
        {
            _isPaused = false;

            _audioSource.UnPause();
        }

        private IEnumerator WaitForSoundToEnd()
        {
            yield return new WaitWhile(() => _audioSource.isPlaying || _isPaused);

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