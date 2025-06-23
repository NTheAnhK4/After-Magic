
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AudioSystem
{
    public class SoundManager : PersistentSingleton<SoundManager>
    {
       
        public readonly Queue<SoundEmitter> FrequentSoundEmitters = new();

        private readonly List<SoundEmitter> soundEmitterHolders = new();
        [SerializeField] private SoundEmitter soundEmitterPrefab;
        
        [SerializeField] private int maxSoundInstances = 30;
        [Range(0, 1)] private float soundRate = .5f;

        public float SoundRate
        {
            get => soundRate;
            set
            {
                soundRate = value;
                SetAllSoundVolume(value);
            }
        }


        private void OnEnable()
        {
            ObserverManager<SoundActionType>.Attach(SoundActionType.PauseAll, PauseAllSound);
            ObserverManager<SoundActionType>.Attach(SoundActionType.UnPauseAll, UnPauseAllSound);
            ObserverManager<SoundActionType>.Attach(SoundActionType.StopAll, StopAllSound);
        }

        private void OnDisable()
        {
            ObserverManager<SoundActionType>.Detach(SoundActionType.PauseAll, PauseAllSound);
            ObserverManager<SoundActionType>.Detach(SoundActionType.UnPauseAll, UnPauseAllSound);
            ObserverManager<SoundActionType>.Detach(SoundActionType.StopAll, StopAllSound);
        }

        public void StopAllSound(object param)
        {
            
            PerformSoundEmitter(t => t.Stop());
            FrequentSoundEmitters.Clear();
            
        }

        public void SetAllSoundVolume(float value) => PerformSoundEmitter(t => t.SetVolum(value));

        public void PauseAllSound(object param) => PerformSoundEmitter(t => t.Pause());

        public void UnPauseAllSound(object param) => PerformSoundEmitter(t => t.UnPause());

        private void PerformSoundEmitter(Action<SoundEmitter> action)
        {
            if (soundEmitterHolders == null || soundEmitterHolders.Count == 0) return;
            for (int i = soundEmitterHolders.Count - 1; i >= 0; i--)
            {
                var soundEmitter = soundEmitterHolders[i];
                if (soundEmitter == null) continue;
                action?.Invoke(soundEmitter);
            }

        }
       
        
        public SoundBuilder CreateSound() => new SoundBuilder(this);
        public void AddSoundEmitter(SoundEmitter soundEmitter) => soundEmitterHolders.Add(soundEmitter);

        public bool CanPlaySound(SoundData data)
        {
            if (!data.FrequentSound) return true;
            if (FrequentSoundEmitters.Count >= maxSoundInstances && FrequentSoundEmitters.TryDequeue(out var soundEmitter))
            {
                try
                {
                    soundEmitter.Stop();
                    return true;
                }
                catch
                {
                    Debug.Log("SoundEmitter is already released");
                }

                return false;

            }

            return true;
        }

        public SoundEmitter Get(Vector3 position) => PoolingManager.Spawn(soundEmitterPrefab.gameObject, position, default, transform).GetComponent<SoundEmitter>();

        public void ReturnToPool(SoundEmitter soundEmitter)
        {
            soundEmitterHolders.Remove(soundEmitter);
            PoolingManager.Despawn(soundEmitter.gameObject);
           
        }
    }
}