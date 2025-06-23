using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MusicType
{
    Lobby,
    Dungeon
}
public class MusicManager : PersistentSingleton<MusicManager>
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<MusicSource> MusicSources = new List<MusicSource>();
    [Range(0, 1)] private float volume = .5f;

    public float Volume
    {
        get => volume;
        set
        {
            volume = value;
            audioSource.volume = value;
        }
    }

    public override void LoadComponent()
    {
        base.LoadComponent();
        if (audioSource == null) audioSource = gameObject.GetOrAdd<AudioSource>();
        audioSource.loop = true;
        Volume = .5f;

    }

    private void OnValidate()
    {
        string[] names = Enum.GetNames(typeof(MusicType));
        while(MusicSources.Count > names.Length) MusicSources.RemoveAt(MusicSources.Count - 1);
        for (int i = 0; i < names.Length; ++i)
        {
            if(i >= MusicSources.Count) MusicSources.Add(new MusicSource());
            MusicSources[i].Name = names[i];
        }
    }

    public void PlayMusic(MusicType musicType)
    {
        audioSource.Stop();
        audioSource.clip = MusicSources[(int)musicType].Clip;
        audioSource.volume = Volume;
        audioSource.Play();

    }
    
    public void PauseMusic() => audioSource.Pause();
    public void UnPauseMusic() => audioSource.UnPause();
    public void StopMusic() => audioSource.Stop();

}

[Serializable]
public class MusicSource
{
    [HideInInspector] public string Name;
    public AudioClip Clip;
}
