using System.Collections;
using UnityEngine;

namespace Game.Blood
{
    [RequireComponent(typeof(ParticleSystem))]
    public class BloodEmitter : MonoBehaviour
    {
        [SerializeField] private BloodType _bloodType;
        public BloodType BloodType => _bloodType;

        private ParticleSystem _particleSystem;
        private Coroutine _playingCoroutine;

        private void Awake() => _particleSystem = GetComponent<ParticleSystem>();

        public void Play()
        {
            if (_playingCoroutine != null) { StopCoroutine(_playingCoroutine); }

            _particleSystem.Play();

            _playingCoroutine = StartCoroutine(WaitForVFXToEnd());
        }

        public void Stop()
        {
            if (_playingCoroutine != null) { StopCoroutine(_playingCoroutine); }

            _playingCoroutine = null;
            _particleSystem.Stop();
            _particleSystem.Clear();

            BloodManager.Instance.Release(this);
        }

        public BloodEmitter Initialize(Vector3 position, Color color)
        {
            transform.position = position;
            var main = _particleSystem.main;
            main.startColor = color;
            return this;
        }

        private IEnumerator WaitForVFXToEnd()
        {
            yield return new WaitWhile(() => _particleSystem.isPlaying);

            BloodManager.Instance.Release(this);
        }
    }
}