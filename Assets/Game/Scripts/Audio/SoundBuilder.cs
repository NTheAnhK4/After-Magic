using UnityEngine;

namespace AudioSystem
{
    public class SoundBuilder
    {
        private readonly SoundManager soundManager;
        private SoundData soundData;
        private Vector3 position = Vector3.zero;
        private bool randomPitch;



        private SoundEmitter soundEmitter;
        public SoundBuilder(SoundManager soundManager) => this.soundManager = soundManager;

        public SoundBuilder WithSoundData(SoundData soundData)
        {
            this.soundData = soundData;
            return this;
        }
        public SoundBuilder WithPosition(Vector3 position)
        {
            this.position = position;
            return this;
        }

        public SoundBuilder WithRandomPitch()
        {
            this.randomPitch = true;
            return this;
        }

        public void Play()
        {
            if (!soundManager.CanPlaySound(soundData)) return;

            soundEmitter = soundManager.Get(position);
            soundEmitter.Initialized(soundData);

            if (randomPitch) soundEmitter.WithRandomPitch();

            soundManager.AddSoundEmitter(soundEmitter);
            if (soundData.FrequentSound) soundManager.FrequentSoundEmitters.Enqueue(soundEmitter);
            soundEmitter.Play();

        }

        public void Stop()
        {
            soundEmitter.OrNull()?.Stop();
        }
    }
}