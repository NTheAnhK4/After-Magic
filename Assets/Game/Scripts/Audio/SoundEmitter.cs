
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AudioSystem
{
    public enum SoundActionType
    {
        PauseAll,
        UnPauseAll,
        StopAll
    }
    public class SoundEmitter : ComponentBehaviour
    {
        public SoundData Data { get; private set; }
        private AudioSource audioSource;
        private Coroutine playingCoroutine;

        private bool isPaused = false;

        protected override void Awake()
        {
            base.Awake();
            audioSource = gameObject.GetOrAdd<AudioSource>();
        }

        public void Initialized(SoundData data)
        {
            Data = data;
            audioSource.clip = data.Clip;
            audioSource.outputAudioMixerGroup = data.MixerGroup;
            audioSource.loop = data.Loop;
            audioSource.volume = data.Volume * SoundManager.Instance.SoundRate;
            audioSource.playOnAwake = data.PlayOnAwake;
        }

        public void SetVolum(float value) => audioSource.volume = Data.Volume * value;

        public void Play()
        {
            if(playingCoroutine != null) StopCoroutine(playingCoroutine);
            audioSource.Play();
            playingCoroutine = StartCoroutine(WaitForSoundToEnd());
        }

        public void Pause()
        {
            if (audioSource.isPlaying)
            {
                isPaused = true;
                audioSource.Pause();
            }
        }

        public void UnPause()
        {
            if (isPaused)
            {
                isPaused = false;
                audioSource.UnPause();
            }
        }

        IEnumerator WaitForSoundToEnd()
        {
            yield return new WaitUntil(() => !audioSource.isPlaying && !isPaused);
            SoundManager.Instance.ReturnToPool(this);
        }

        public void Stop()
        {
            if (playingCoroutine != null)
            {
                StopCoroutine(playingCoroutine);
                playingCoroutine = null;
            }
            
            audioSource.Stop();
            SoundManager.Instance.ReturnToPool(this);
        }

        public void WithRandomPitch(float min = -0.05f, float max = 0.05f)
        {
            audioSource.pitch += Random.Range(min, max);
        } 
    }
}