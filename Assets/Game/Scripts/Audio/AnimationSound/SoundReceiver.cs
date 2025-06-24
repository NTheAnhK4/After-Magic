
using AudioSystem;
using UnityEngine;

public class SoundReceiver : ComponentBehavior
{
    private SoundData data;
    private bool randomPitch;
    private SoundBuilder soundBuilder;
    public void SetSoundData(SoundData soundData, bool randomPitchParam)
    {
        data = soundData;
        randomPitch = randomPitchParam;
    }

    public void PlaySound()
    {
        if (data == null) return;
        Vector3 spawnPos = transform.parent == null ? transform.position : transform.parent.position;
        soundBuilder = SoundManager.Instance.CreateSound().WithSoundData(data).WithPosition(spawnPos);
        if (randomPitch) soundBuilder.WithRandomPitch();
        soundBuilder.Play();
    }

    public void StopSound()
    {
        if(soundBuilder != null) soundBuilder.Stop();
    }
}
